using System;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable]
        public class Path
        {
            public const string SAVE_FOLDER = "/saves/"; 
            
            public enum PathType
            {
                DataPath,
                PersistentDataPath
            }


            [SerializeField] private PathType _type;
            
            public static implicit operator string (Path p)
            {
                if (p._type == PathType.DataPath) return GetPath(Application.dataPath);
                if (p._type == PathType.PersistentDataPath) return GetPath(Application.persistentDataPath);
                
                throw new Exception("Save path not found");
            }

            
            /// <returns> Returns path of Main folder + sub folder</returns>
            private static string GetPath(string mainFolder) => mainFolder + SAVE_FOLDER;
        }
    }
}