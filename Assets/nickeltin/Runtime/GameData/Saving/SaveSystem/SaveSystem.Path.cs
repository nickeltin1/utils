using System;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        [Serializable]
        public class Path
        {
            public const string SAVE_FOLDER = "/saves/"; 
            
            public enum PathType
            {
                PersistentDataPath,
                DataPath
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


#if UNITY_EDITOR
            public static string type_prop_name => nameof(_type);
#endif
        }
    }
}