using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using nickeltin.GameData.DataObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// State saver for MonoBehaviour's, can save one <see cref="string"/>, one <see cref="bool"/> and one <see cref="float"/>.
    /// Use <see cref="beforeSave"/> event to assign data with <see cref="SetData(nickeltin.GameData.Saving.MonoSave.Values)"/>
    /// before saving.
    /// </summary>
    [DisallowMultipleComponent]
    public class MonoSave : MonoBehaviour
    {
        private static class GUIDs
        {
            public readonly static List<string> list;
            static GUIDs() => list = new List<string>();
        }
        
        [Serializable]
        public struct Values
        {
            public bool Bool;
            public string String;
            public float Number;
        }
        
        [Serializable]
        private struct DefaultValues
        {
            public BoolReference Bool;
            public StringReference String;
            public NumberReference Number;
        }
        
        [SerializeField, ReadOnly] private string m_guid;
        [SerializeField] private GenericSave m_save;
        [SerializeField] private DefaultValues m_defaultFile;
        [SerializeField, ReadOnly] private Values m_file;
        
        public UnityEvent<MonoSave> afterLoad; 
        public UnityEvent<MonoSave> beforeSave;
        
        private bool m_loaded = false;

        public bool SuccessfulyLoaded { get; private set; } = false;
        
        public Values Data => m_file;
        public string GUID => m_guid;
        
        private void OnEnable()
        {
            if (!m_loaded) LoadValues();
        }
        private void Awake() => SaveSystem.onBeforeSave += SaveValues;
        private void OnDestroy() => SaveSystem.onBeforeSave -= SaveValues;
        
        private void SaveValues()
        {
            beforeSave.Invoke(this);
            m_save.SetMonoSave(this);
        }
        private void LoadValues()
        {
            if (m_guid.IsNullOrEmpty())
            {
                Debug.LogError($"{name} doesn't have GUID, generate it with context menu");
                return;
            } 
            
            if (GUIDs.list.Contains(m_guid))
            {
                Debug.LogError($"{name} have same GUID as other item, regenerate it with context menu");
                return;
            }

            GUIDs.list.Add(m_guid);

            if (m_save.TryGetMonoSave(m_guid, out m_file)) SuccessfulyLoaded = true;
            else
            {
                m_file = new Values()
                {
                    Bool = m_defaultFile.Bool,
                    String = m_defaultFile.String,
                    Number = m_defaultFile.Number
                };
            }
            
            afterLoad?.Invoke(this);
            m_loaded = true;
        }

        public void SetData([Optional] bool? b, [Optional] string s, [Optional] float? n)
        {
            Values old = m_file;
            m_file = new Values()
            {
                Bool = b ?? old.Bool,
                String = s.IsNullOrEmpty() ? old.String : s,
                Number = n ?? old.Number
            };
        }
        public void SetData(Values data) => m_file = data;

#if UNITY_EDITOR
        [ContextMenu("Generate GUID")]
        private void GenerateGUID_Context()
        {
            Undo.RecordObject(this, "GUID generation");
            m_guid = SaveableBase.GenerateGUID();
        }

        private void OnValidate()
        {
            if (m_guid.IsNullOrEmpty()) m_guid = SaveableBase.GenerateGUID();
        }
#endif
    }
}