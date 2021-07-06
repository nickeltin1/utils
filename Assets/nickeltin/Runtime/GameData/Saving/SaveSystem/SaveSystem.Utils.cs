using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using nickeltin.Runtime.GameData.Saving.SerializationSurrogates;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        public static class Core
        {
            public static readonly BinaryFormatter binaryFormatter;
            
            static Core()
            {
                binaryFormatter = GetBinaryFormater();
            }

            public static bool CheckIsObjectSerializeable<T>(string key, string actionLable)
            {
                if (!typeof(T).IsSerializable)
                {
                    Debug.LogError(
                        $"Save with key {key} cannot be {actionLable}, it doesn't have {typeof(SerializableAttribute)}");
                    return false;
                }

                return true;
            }

            public static BinaryFormatter GetBinaryFormater()
            {
                BinaryFormatter formatter = new BinaryFormatter();

                SurrogateSelector selector = new SurrogateSelector();
                Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
                QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

                selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
                selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All),
                    quaternionSurrogate);

                formatter.SurrogateSelector = selector;

                return formatter;
            }
            public static string GenerateUniqueName()
            {
                //return System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName());
                return Guid.NewGuid().ToString();
            }

            public static void CreateFolder(string path)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}