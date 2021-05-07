using System;
using System.Collections.Generic;
using nickeltin.Extensions;
using nickeltin.GameData.DataObjects;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Generic save for all types of data.
    /// By default can store arrays of: <see cref="BoolReference"/>, <see cref="NumberReference"/>, <see cref="StringReference"/>
    /// which can be assigned in inspector.
    /// Also if its provided inside <see cref="MonoSave"/> will hold its data.
    /// Can store any <see cref="object"/>, assign them with <see cref="SetObject{T}"/>, or get with <see cref="GetObject{T}"/>.
    /// </summary>
    [CreateAssetMenu(menuName = MenuPathsUtility.savingMenu + nameof(GenericSave))]
    public sealed class GenericSave : Saveable<object[]>
    {
        [Serializable]
        public class MonoSaveEntry
        {
            public string key;
            public object value;
        }
        
        [Serializable]
        public class Entry<T> where T : VariableReferenceBase
        {
            public T defaultValue;
            public T value;
        }
        
        [SerializeField] private Entry<VarRef<float>>[] m_numbers;
        [SerializeField] private Entry<VarRef<string>>[] m_strings;
        [SerializeField] private Entry<VarRef<bool>>[] m_bools;
        
        private object[] m_objects;
        private Dictionary<string, object> m_monoSavesDictionary;
        
        /// <summary>
        /// Realocates memory for new array with new size, copying old values to temporary array, and back
        /// </summary>
        public void SetObjectsBufferSize(int size)
        {
            object[] temp = new object[m_objects.Length];
            m_objects.CopyTo(temp, 0);
            
            m_objects = new object[size];
            temp.CopyTo(m_objects, 0);
        }

        /// <summary>
        /// Before setting objects, its better alocate required objects buffer size with <see cref="SetObjectsBufferSize"/>
        /// </summary>
        public void SetObject<T>(int id, T obj)
        {
            if (!typeof(T).IsSerializable)
            {
                Debug.LogError($"Custom object of type {typeof(T)} is not serializable, and won't be saved");
            }

            if (id > m_objects.Length - 1) SetObjectsBufferSize(id + 1);
            
            m_objects[id] = obj;
        }
        public void SetMonoSave(MonoSave save)
        {
            if (!save.Data.IsSerializable())
            {
                Debug.Log($"Data of {save} is not serializable");
                return;
            }

            if (m_monoSavesDictionary.ContainsKey(save.GUID)) m_monoSavesDictionary[save.GUID] = save.Data;
            else m_monoSavesDictionary.Add(save.GUID, save.Data);
        }
        public bool SetNumber(float n, int id) => TryToSetArrayValue(m_numbers, n, id);
        public bool SetString(string s, int id) => TryToSetArrayValue(m_strings, s, id);
        public bool SetBool(bool b, int id) => TryToSetArrayValue(m_bools, b, id);

        private bool TryToSetArrayValue<T>(Entry<T>[] array, object value, int index) where T : VariableReferenceBase
        {
            if (array.InRange(index))
            {
                array[index].value.SetValueWithoutType(value);
                return true;
            }

            Debug.LogError($"Setter index for {array} array in {name} object is out of range. " +
                           "Assign array capacity in the inspector");
            return false;
        }

        public T GetObject<T>(int id) => (T)m_objects[id];
        public bool TryGetMonoSave(string key, out object data) => m_monoSavesDictionary.TryGetValue(key, out data);
        public float GetNumber(int id) => m_numbers[id].value;
        public string GetString(int id) => m_strings[id].value;
        public bool GetBool(int id) => m_bools[id].value;
        
        public int Length
        {
            //1 = first object in saves array, saves the length of custom objects array
            //1 = second object in saves array, monoSaves array length
            get => 1 + 1 + m_numbers.Length + m_strings.Length + m_bools.Length + m_objects.Length + 
                   m_monoSavesDictionary.Count;
        }
        
        private void DeserializeCustomObjectsArray(object[] from, int startingIndex, out int lastItemIndex)
        {
            for (int i = 0; i < m_objects.Length; i++)
            {
                m_objects[i] = from[startingIndex + i];
            }

            lastItemIndex = startingIndex + m_objects.Length;
        }
        
        private void SerializeCustomObjectsArray(object[] to, int startingIndex, out int lastItemIndex)
        {
            for (int i = 0; i < m_objects.Length; i++)
            {
                to[startingIndex + i] = m_objects[i];
            }

            lastItemIndex = startingIndex + m_objects.Length;
        }
        
        
        private void DeserializeMonoSaves(object[] from, int length, int startingIndex)
        {
            for (int i = 0; i < length; i++)
            {
                MonoSaveEntry pair = (MonoSaveEntry) from[startingIndex + i];
                m_monoSavesDictionary.Add(pair.key, pair.value);
            }
        }
        
        private void SerializeMonoSaves(object[] to, int startingIndex)
        {
            int i = 0;
            foreach (var keyValuePair in m_monoSavesDictionary)
            {
                to[startingIndex + i] = new MonoSaveEntry()
                {
                    key = keyValuePair.Key,
                    value = keyValuePair.Value
                };
                i++;
            }
        }
        
        private static void SerializeArray<T>(Entry<T>[] from, object[] to, int startingIndex, out int lastItemIndex) 
            where T : VariableReferenceBase
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[startingIndex + i] = from[i].value.GetValueWithoutType();
            }

            lastItemIndex = startingIndex + from.Length;
        }

        private static void DeserializeArray<T>(object[] from, Entry<T>[] to, int startingIndex, out int lastItemIndex)
            where T : VariableReferenceBase
        {
            for (int i = 0; i < to.Length; i++)
            {
                to[i].value.SetValueWithoutType(from[startingIndex + i]);
            }

            lastItemIndex = startingIndex + to.Length;
        }
        
        private static void LoadDefaultToArray<T>(Entry<T>[] array) where T : VariableReferenceBase
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i].value.SetValueWithoutType(array[i].defaultValue.GetValueWithoutType());
            }
        }
        
        protected override void LoadDefault()
        {
            m_objects = new object[0];
            m_monoSavesDictionary = new Dictionary<string, object>();

            LoadDefaultToArray(m_numbers);
            LoadDefaultToArray(m_strings);
            LoadDefaultToArray(m_bools);
        }

        protected override void Deserialize(object[] obj)
        {
            m_objects = new object[(int)obj[0]];
            m_monoSavesDictionary = new Dictionary<string, object>();
            int startingIndex = 2;
            
            DeserializeArray(obj, m_numbers, startingIndex, out startingIndex);
            DeserializeArray(obj, m_strings, startingIndex, out startingIndex);
            DeserializeArray(obj, m_bools, startingIndex, out startingIndex);
            DeserializeCustomObjectsArray(obj, startingIndex, out startingIndex);
            DeserializeMonoSaves(obj, (int)obj[1], startingIndex);
        }

        protected override object[] Serialize()
        {
            object[] obj = new object[Length];
            obj[0] = m_objects.Length;
            obj[1] = m_monoSavesDictionary.Count;
            int startingIndex = 2;
            
            SerializeArray(m_numbers, obj, startingIndex, out startingIndex);
            SerializeArray(m_strings, obj, startingIndex, out startingIndex);
            SerializeArray(m_bools, obj, startingIndex, out startingIndex);
            SerializeCustomObjectsArray(obj, startingIndex, out startingIndex);
            SerializeMonoSaves(obj, startingIndex);

            return obj;
        }
    }
}