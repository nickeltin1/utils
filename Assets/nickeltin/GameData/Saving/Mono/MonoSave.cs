using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// State saver for MonoBehaviour's, can save <see cref="object"/>.
    /// Use <see cref="beforeSave"/> event to assign data object before saving.
    /// </summary>
    [DisallowMultipleComponent]
    public class MonoSave : MonoBehaviour
    {
        private static class GUIDs
        {
            public readonly static List<string> list;
            static GUIDs() => list = new List<string>();
        }
        
        [SerializeField, ReadOnly] private string m_guid;
        [SerializeField] private GenericSave m_save;
        [SerializeField, Tooltip("Load and Save methods on target will be called automatically")] 
        private MonoSaveable m_target;
        private object m_data;
        
        public UnityEvent<MonoSave> afterLoad; 
        public UnityEvent<MonoSave> beforeSave;
        
        private bool m_loaded = false;

        public bool SuccessfulyLoaded { get; private set; } = false;
        
        public object Data => m_data;
        public string GUID => m_guid;
        
        private void OnEnable()
        {
            if (!m_loaded) LoadValues();
        }
        private void Awake() => SaveSystem.onBeforeSave += SaveValues;
        private void OnDestroy() => SaveSystem.onBeforeSave -= SaveValues;
        
        private void SaveValues()
        {
            if (m_target != null) m_data = m_target.Save();
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

            if (m_save.TryGetMonoSave(m_guid, out m_data)) SuccessfulyLoaded = true;
            else
            {
                m_data = new object();
                SuccessfulyLoaded = false;
            }
            
            if (m_target != null) m_target.Load(m_data);
            afterLoad?.Invoke(this);
            m_loaded = true;
        }
        
        public void SetData(object data) => m_data = data;
        
        public void GenerateGUID() => m_guid = SaveableBase.GenerateGUID();

#if UNITY_EDITOR
        [ContextMenu("Generate GUID")]
        private void GenerateGUID_Context()
        {
            Undo.RecordObject(this, "GUID generation");
            GenerateGUID();
        }
        
        private void OnValidate()
        {
            if (m_guid.IsNullOrEmpty()) m_guid = SaveableBase.GenerateGUID();
        }
#endif
    }
}