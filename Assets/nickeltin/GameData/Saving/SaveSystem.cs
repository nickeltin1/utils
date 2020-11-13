using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nickeltin.GameData.Saving.SerializationSurrogates;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    public static class SaveSystem
    {
        private static readonly string path;
        private const string extention = ".save";
        private static readonly Dictionary<string, ISaveable> saves;

        static SaveSystem()
        {
            saves = new Dictionary<string, ISaveable>();
            path = Application.persistentDataPath + "/saves/";
        }
        
        public static bool AddSavedItem(string key, ISaveable entery)
        {
            if (saves.ContainsKey(key))
            {
                Debug.LogWarning($"The saveID {key} of {entery} are already exists, it must be unique!");
                return false;
            }

            saves.Add(key, entery);
            return true;
        }

        public static bool GetSavedItem<T>(string key, out T save) where T : class, ISaveable
        {
            if (saves.TryGetValue(key, out var s))
            {
                save = s as T;
                return true;
            }
            
            save = null;
            return false;
        }

        public static void Save<T>(T obj, string key)
        {
            Directory.CreateDirectory(path);
            BinaryFormatter formatter = GetBinaryFormater();
            using (FileStream fileStream = new FileStream(path + key + extention, FileMode.Create))
            {
                formatter.Serialize(fileStream, obj);
            }
        }

        public static T Load<T>(string key)
        {
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

        public static void DeleteSaves()
        {
            foreach (var save in saves.Values) save.Load(true);

            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Delete(true);
            Directory.CreateDirectory(path);

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