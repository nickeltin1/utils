using System;
using System.Collections.Generic;
using nickeltin.Extensions;
using nickeltin.Editor.Utility;
using nickeltin.GameData.References;
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
        public class Entry<T> //where T : IValueWithoutTypeProvider
        {
            public VarRef<T> defaultValue;
            public VarRef<T> value;
        }

        [SerializeField, TextArea] private string _description;
        [SerializeField] private Entry<float>[] _numbers;
        [SerializeField] private Entry<string>[] _strings;
        [SerializeField] private Entry<bool>[] _bools;
        
        private object[] _objects;
        private Dictionary<string, object> _monoSavesDictionary;
        
        /// <summary>
        /// Realocates memory for new array with new size, copying old values to temporary array, and back
        /// </summary>
        public void SetObjectsBufferSize(int size)
        {
            object[] temp = new object[_objects.Length];
            _objects.CopyTo(temp, 0);
            
            _objects = new object[size];
            temp.CopyTo(_objects, 0);
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

            if (id > _objects.Length - 1) SetObjectsBufferSize(id + 1);
            
            _objects[id] = obj;
        }
        public void SetMonoSave(MonoSave save)
        {
            if (!save.Data.IsSerializable())
            {
                Debug.Log($"Data of {save} is not serializable");
                return;
            }

            if (_monoSavesDictionary.ContainsKey(save.SaveID)) _monoSavesDictionary[save.SaveID] = save.Data;
            else _monoSavesDictionary.Add(save.SaveID, save.Data);
        }
        public bool SetNumber(float n, int id) => TryToSetArrayValue(_numbers, n, id);
        public bool SetString(string s, int id) => TryToSetArrayValue(_strings, s, id);
        public bool SetBool(bool b, int id) => TryToSetArrayValue(_bools, b, id);

        private bool TryToSetArrayValue<T>(Entry<T>[] array, object value, int index)
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

        public T GetObject<T>(int id) => (T)_objects[id];
        public bool TryGetMonoSave(string key, out object data) => _monoSavesDictionary.TryGetValue(key, out data);
        public float GetNumber(int id) => _numbers[id].value;
        public string GetString(int id) => _strings[id].value;
        public bool GetBool(int id) => _bools[id].value;
        
        public int Length
        {
            //1 = first object in saves array, saves the length of custom objects array
            //1 = second object in saves array, monoSaves array length
            get => 1 + 1 + _numbers.Length + _strings.Length + _bools.Length + _objects.Length + 
                   _monoSavesDictionary.Count;
        }
        
        private void DeserializeCustomObjectsArray(object[] from, int startingIndex, out int lastItemIndex)
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                _objects[i] = from[startingIndex + i];
            }

            lastItemIndex = startingIndex + _objects.Length;
        }
        
        private void SerializeCustomObjectsArray(object[] to, int startingIndex, out int lastItemIndex)
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                to[startingIndex + i] = _objects[i];
            }

            lastItemIndex = startingIndex + _objects.Length;
        }
        
        
        private void DeserializeMonoSaves(object[] from, int length, int startingIndex)
        {
            for (int i = 0; i < length; i++)
            {
                MonoSaveEntry pair = (MonoSaveEntry) from[startingIndex + i];
                _monoSavesDictionary.Add(pair.key, pair.value);
            }
        }
        
        private void SerializeMonoSaves(object[] to, int startingIndex)
        {
            int i = 0;
            foreach (var keyValuePair in _monoSavesDictionary)
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
        {
            for (int i = 0; i < from.Length; i++)
            {
                to[startingIndex + i] = from[i].value.GetValueWithoutType();
            }

            lastItemIndex = startingIndex + from.Length;
        }

        private static void DeserializeArray<T>(object[] from, Entry<T>[] to, int startingIndex, out int lastItemIndex)
        {
            for (int i = 0; i < to.Length; i++)
            {
                to[i].value.SetValueWithoutType(from[startingIndex + i]);
            }

            lastItemIndex = startingIndex + to.Length;
        }
        
        private static void LoadDefaultToArray<T>(Entry<T>[] array)
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                array[i].value.SetValueWithoutType(array[i].defaultValue.GetValueWithoutType());
            }
        }
        
        public override void LoadDefault()
        {
            _objects = new object[0];
            _monoSavesDictionary = new Dictionary<string, object>();

            LoadDefaultToArray(_numbers);
            LoadDefaultToArray(_strings);
            LoadDefaultToArray(_bools);
        }

        protected override void Deserialize(SaveSystem saveSystem, object[] obj)
        {
            _objects = new object[(int)obj[0]];
            _monoSavesDictionary = new Dictionary<string, object>();
            int startingIndex = 2;
            
            DeserializeArray(obj, _numbers, startingIndex, out startingIndex);
            DeserializeArray(obj, _strings, startingIndex, out startingIndex);
            DeserializeArray(obj, _bools, startingIndex, out startingIndex);
            DeserializeCustomObjectsArray(obj, startingIndex, out startingIndex);
            DeserializeMonoSaves(obj, (int)obj[1], startingIndex);
        }

        protected override object[] Serialize()
        {
            object[] obj = new object[Length];
            obj[0] = _objects.Length;
            obj[1] = _monoSavesDictionary.Count;
            int startingIndex = 2;
            
            SerializeArray(_numbers, obj, startingIndex, out startingIndex);
            SerializeArray(_strings, obj, startingIndex, out startingIndex);
            SerializeArray(_bools, obj, startingIndex, out startingIndex);
            SerializeCustomObjectsArray(obj, startingIndex, out startingIndex);
            SerializeMonoSaves(obj, startingIndex);

            return obj;
        }
    }
}