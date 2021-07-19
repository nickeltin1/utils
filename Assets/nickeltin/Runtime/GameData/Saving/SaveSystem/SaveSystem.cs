using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using nickeltin.Runtime.Utility;
using nickeltin.Extensions;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.Singletons;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    /// <summary>
    /// Controls all saves in the game, handles autosave events, contains all saves database, get it with
    /// <see cref="GetSavedItem{T}"/>. All entries can be assigned in Inspector, or with <see cref="AddSavedItem"/>
    /// </summary>
    [CreateAssetMenu(menuName = MenuPathsUtility.savingMenu + nameof(SaveSystem))]
    public sealed partial class SaveSystem : SOSingleton<SaveSystem>
    {
        private const string DEFAULT_SUB_FOLDER_NAME = "save/";
        
        public static event Action onBeforeSave;

        //Custom editor variables
        [SerializeField] private bool _enabled = true;
        [SerializeField] private bool _logEvents = true;
        [SerializeField] private DirectorySettings _directorySettings;
        [SerializeField] private List<SubFolder> _subFolders = new List<SubFolder>(){new SubFolder(DEFAULT_SUB_FOLDER_NAME)};

        //Regural editor variables
        [SerializeField] private AutosavingSettings _autosavingSettings;
        [SerializeField] private EventObject[] _saveTriggers;
        [SerializeField] private SaveableBase[] _saves;

        private Dictionary<string, SaveableBase> _allSaves;
        private int _currentSubFolderId = 0;
        
        public string RootSavePath => _directorySettings.path;
        public string CurrentSavePath => GetSubFolderPath(_currentSubFolderId);
        public int CurrentSubFolderId => _currentSubFolderId;

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                _allSaves = new Dictionary<string, SaveableBase>();

                _saves.ForEach(save => save.Register(this));

                if (Application.isPlaying)
                {
                    LoadAll();
                    var worldInstance = new GameObject("autosaver").AddComponent<AutoSaver>();
                    worldInstance.Initialize(_autosavingSettings);
                    worldInstance.onBeforeAutoSave += SaveAll;
                }

                _saveTriggers.ForEach(e => e.BindEvent(SaveAll));
                return true;
            }

            return false;
        }

        public override bool Destruct()
        {
            if (base.Destruct())
            {
                _saveTriggers.ForEach(e => e.UnbindEvent(SaveAll));
                return true;
            }

            return false;
        }
        
        public void Save<T>(T obj, string key)
        {
            if (!_enabled) return;

            if (!Core.CheckIsObjectSerializeable<T>(key, "SAVED")) return;

            if (!_allSaves.ContainsKey(key))
            {
                Debug.Log($"Save with key {key} is saved explicitly, and not contained in {nameof(SaveSystem)} Saves list");
            }
            
            Core.CreateFolder(CurrentSavePath);
            using (FileStream fileStream = new FileStream(GetFilePath(key), FileMode.Create))
            {
                Core.binaryFormatter.Serialize(fileStream, obj);
            }
        }

        public T Load<T>(string key)
        {
            if (!_enabled) return default;

            if (!Core.CheckIsObjectSerializeable<T>(key, "LOADED")) return default;
            
            T loadedObject;
            using (FileStream fileStream = new FileStream(GetFilePath(key), FileMode.Open))
            {
                loadedObject = (T) Core.binaryFormatter.Deserialize(fileStream);
            }

            return loadedObject;
        }

        public void LoadAll()
        {
            if (!_enabled) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Next Saves is loaded: ");
            _saves.ForEach(save =>
            {
                if (save is RegistryItem) return;

                if (save.Load(this)) sb.AppendLine(save.SaveID);
                else sb.AppendLine(save.SaveID + " WITH DEFAULT VALUE");
            });

            if (_logEvents) Debug.Log(sb.ToString());
        }

        public void SaveAll()
        {
            if (!_enabled) return;

            onBeforeSave?.Invoke();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Next Saves is saved: ");
            _saves.ForEach(save =>
            {
                if (save is RegistryItem) return;
                save.Save(this);
                sb.AppendLine(save.SaveID);
            });

            if (_logEvents) Debug.Log(sb.ToString());
        }
        
        public bool AddSavedItem(SaveableBase entery)
        {
            if (_allSaves.ContainsKey(entery.SaveID))
            {
                Debug.LogError($"The saveID {entery.SaveID} of {entery} are already exists, it must be unique!");
                return false;
            }

            _allSaves.Add(entery.SaveID, entery);
            return true;
        }

        public bool GetSavedItem<T>(string key, out T save) where T : SaveableBase
        {
            if (_allSaves.TryGetValue(key, out var s))
            {
                save = s as T;
                return true;
            }

            save = null;
            return false;
        }

        public bool ContainsSave(string key) => _allSaves.ContainsKey(key);

        public bool SaveExists(string key) => File.Exists(GetFilePath(key));

        public void DeleteCurrentSave() => DeleteSavesAt(CurrentSavePath);

        public void DeleteAllSaves() => DeleteSavesAt(RootSavePath);

        public void DeleteSavesAt(string path)
        {
            _saves.ForEach(save => save.LoadDefault());
            
            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                if (directory.GetFiles().Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Next save files was deleted:");
                    foreach (var file in directory.EnumerateDirectories())
                    {
                        sb.AppendLine(file.FullName);
                    }

                    if (_logEvents) Debug.Log(sb.ToString());
                }

                directory.Delete(true);
            }
        }
        
        public void SwitchSubFolder(int id)
        {
            _currentSubFolderId = id;
            if (Application.isPlaying) LoadAll();
        }

        public string GetSubFolderPath(int subFolderId) => 
            _directorySettings.GetSubFolderPath(_subFolders[subFolderId]);

        private string GetFilePath(string fileName) => 
            _directorySettings.GetFilePath(_subFolders[_currentSubFolderId], fileName);


#if UNITY_EDITOR
        public static string enabled_prop_name => nameof(_enabled);
        public static string directory_settings_prop_name => nameof(_directorySettings);
        public static string sub_folders_prop_name => nameof(_subFolders);
        public static string log_events_prop_name => nameof(_logEvents);
        public static string saves_prop_name => nameof(_saves);
#endif
    }
}