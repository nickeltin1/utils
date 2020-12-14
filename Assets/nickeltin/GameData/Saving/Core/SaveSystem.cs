using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using nickeltin.Editor.Attributes;
using nickeltin.GameData.Saving.SerializationSurrogates;
using nickeltin.Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Controls all saves in the game, handles autosave events, contains all saves database, get it with
    /// <see cref="GetSavedItem{T}"/>. All entries can be assigned in Inspector, or with <see cref="AddSavedItem"/>
    /// </summary>
    [CreateAssetMenu(menuName = "GameData/Saving/SaveSystem")]
    public sealed class SaveSystem : SOSingleton<SaveSystem>
    {
        private class SaveSystemAutoSaver : MonoBehaviour
        {
            [HideInInspector] public float autosaveInterval;
            public event Action onBeforeSave_internal;

            private Coroutine saveCycle;
            
            public void Initialize()
            {
                DontDestroyOnLoad(gameObject);
                RestartSaveCycle();
            }
            
            IEnumerator AutoSave()
            {
                yield return new WaitForSeconds(autosaveInterval);
                this.onBeforeSave_internal?.Invoke();
                saveCycle = StartCoroutine(AutoSave());
            }

            private void RestartSaveCycle()
            {
                if(saveCycle != null) StopCoroutine(saveCycle);
                saveCycle = StartCoroutine(AutoSave());
            }

            private void InvokeSaveEvent()
            {
                RestartSaveCycle();
                onBeforeSave_internal?.Invoke();
            }
            
            private void OnApplicationFocus(bool hasFocus)
            {
                if (!hasFocus && !Application.isEditor) InvokeSaveEvent();
            }


            private void OnApplicationPause(bool pauseStatus)
            {
                if (pauseStatus) InvokeSaveEvent();
            } 

            private void OnApplicationQuit() => InvokeSaveEvent();
        }
        
        private static string path => Application.persistentDataPath + "/saves/";
        private const string extention = ".save";
        private static Dictionary<string, SaveableBase> allSaves;
        public static event Action onBeforeSave;

        [SerializeField] private bool m_enabled = true;
        [SerializeField] private bool m_logEvents = true;
        [SerializeField, Range(10, 900), Tooltip("In seconds")] private float m_autosaveInterval = 120;
        [SerializeField, ReorderableList("SAVE")] private List<SaveableBase> m_saves;
        [SerializeField] private UnityEvent onBeforeSaveEvent; 

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                allSaves = new Dictionary<string, SaveableBase>();

                foreach (var save in m_saves) save.Register();

                if (Application.isPlaying)
                {
                    LoadAll();
                    var worldInstance = new GameObject("autosaver").AddComponent<SaveSystemAutoSaver>();
                    worldInstance.autosaveInterval = m_autosaveInterval;
                    worldInstance.Initialize();
                    worldInstance.onBeforeSave_internal += SaveAll;
                }
                
                return true;
            }

            return false;
        }

        public static void LoadAll()
        {
            if(!Instance.m_enabled) return;
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Next Saves is loaded: ");
            foreach (var save in Instance.m_saves)
            {
                if (save is RegistryItem) continue;
                
                if (save.Load()) sb.AppendLine(save.SaveID);
                else sb.AppendLine(save.SaveID + " WITH DEFAULT VALUE");
            }
            
            if(Instance.m_logEvents) Debug.Log(sb.ToString());
        }
        
        public static void SaveAll()
        {
            if(!Instance.m_enabled) return;
            
            onBeforeSave?.Invoke();
            Instance.onBeforeSaveEvent.Invoke();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Next Saves is saved: ");
            foreach (var save in Instance.m_saves)
            {
                if(save is RegistryItem) continue;
                
                save.Save();
                sb.AppendLine(save.SaveID);
            }
            
            if(Instance.m_logEvents) Debug.Log(sb.ToString());
        }
        
        public static bool AddSavedItem(SaveableBase entery)
        {
            if (allSaves.ContainsKey(entery.SaveID))
            {
                Debug.LogError($"The saveID {entery.SaveID} of {entery} are already exists, it must be unique!");
                return false;
            }

            allSaves.Add(entery.SaveID, entery);
            return true;
        }

        public static bool GetSavedItem<T>(string key, out T save) where T : SaveableBase
        {
            if (allSaves.TryGetValue(key, out var s))
            {
                save = s as T;
                return true;
            }
            
            save = null;
            return false;
        }

        public static bool ContainsSave(string key) => allSaves.ContainsKey(key);

        public static void Save<T>(T obj, string key)
        {
            if(!Instance.m_enabled) return;
            
            if (!IsObjectSerializeable<T>(key, "SAVED")) return;

            if (!allSaves.ContainsKey(key))
            {
                Debug.Log($"Save with key {key} is saved explicitly, and not contained in {typeof(SaveSystem).Name} Saves list");
            }
            
            Directory.CreateDirectory(path);
            BinaryFormatter formatter = GetBinaryFormater();
            using (FileStream fileStream = new FileStream(path + key + extention, FileMode.Create))
            {
                formatter.Serialize(fileStream, obj);
            }
        }
        
        public static T Load<T>(string key)
        {
            if(!Instance.m_enabled) return default;
            
            if(!IsObjectSerializeable<T>(key, "LOADED")) return default;

            BinaryFormatter formatter = GetBinaryFormater();
            T loadedObject;
            using (FileStream fileStream = new FileStream(path + key + extention, FileMode.Open))
            {
                loadedObject = (T)formatter.Deserialize(fileStream);
            }
            
            return loadedObject;
        }
        
        public static bool SaveExists(string key)
        {
            return File.Exists(path + key + extention);
        }

#if UNITY_EDITOR
        [Button("Open Saves Folder")]
        private static void OpenSavesFolder_Button()
        {
            Directory.CreateDirectory(path);
            EditorUtility.RevealInFinder(path);
        }
        
        [Button("Delete all saves", EButtonEnableMode.Editor)]
        private static void DeleteSaves_Button()
        {
            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete all saves?", 
                "Yes", "No"))
            {
                DeleteSaves();
            }
        }
#endif
        
        public static void DeleteSaves()
        {
            foreach (var save in Instance.m_saves) save.Load(true);

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                
                if (directory.GetFiles().Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Next save files was deleted:");
                    foreach (var file in directory.EnumerateFiles())
                    {
                        sb.AppendLine(file.FullName);
                    }
                    if(Instance.m_logEvents) Debug.Log(sb.ToString());
                }
                
                directory.Delete(true);
                Directory.CreateDirectory(path);
            }
        }
        
        private static bool IsObjectSerializeable<T>(string key, string actionLable)
        {
            if (!typeof(T).IsSerializable)
            {
                Debug.LogError(
                    $"Save with key {key} cannot be {actionLable}, it doesn't have {typeof(SerializableAttribute)}");
                return false;
            }

            return true;
        }

        private static BinaryFormatter GetBinaryFormater()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            SurrogateSelector selector = new SurrogateSelector();
            Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
            QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();
            
            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}