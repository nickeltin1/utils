using System;
using System.Collections.Generic;
using System.Text;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// Savable without type, scriptable object. 
    /// </summary>
    public abstract class SaveableBase : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector, Tooltip("Use automatically generated ID")] protected bool m_useGuid = true;
        [SerializeField, HideInInspector, Tooltip("ID should be unique")] protected string m_saveId;
        [SerializeField, HideInInspector, Tooltip("Globally Unique Identifier")] protected string m_guid = null;
        
        public bool UseGUID
        {
            get => m_useGuid;
            set => m_useGuid = value;
        }
        public string SaveID
        {
            get => m_useGuid ? m_guid : m_saveId;
        }
        
        public bool SuccessfulyLoaded { get; private set; } = false;

        public virtual bool Register() => SaveSystem.AddSavedItem(this);
        /// <param name="loadDefault">Force to apply default state to save</param>
        /// <returns>Is load successful</returns>
        public abstract bool Load(bool loadDefault = false);
        public abstract void Save();
        protected abstract void LoadDefault();

        public virtual void SetLoadState(bool state) => SuccessfulyLoaded = state;
        public abstract void SetFileWithoutType(object file);
        public abstract object GetFileWithoutType();
        
        public static string GenerateGUID() => Guid.NewGuid().ToString();
        public static string GenerateSaveID(string objectName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < objectName.Length; i++)
            {
                if (char.IsWhiteSpace(objectName[i])) sb.Append("_");
                else if (char.IsUpper(objectName[i]))
                {
                    if (i == 0 || char.IsWhiteSpace(objectName[(i - 1).Clamp0()])) sb.Append(char.ToLower(objectName[i]));
                    else sb.Append("_" + char.ToLower(objectName[i]));
                }
                else sb.Append(objectName[i]);
            }

            return sb.ToString().Trim();
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Method for editor, do not use it
        /// </summary>
        public static bool ContainsNestedSavables(IEnumerable<SaveableBase> source, SaveableBase self, out bool containsItself)
        {
            if (source == null)
            {
                containsItself = false;
                return false;
            } 
            
            bool value = false;
            foreach (var savable in source)
            {
                if (savable == null) continue;

                if (savable.Equals(self))
                {
                    containsItself = true;
                    return true;
                }
                
                if (savable is SavePackage || savable is SaveRegistry) value = true;
            }

            containsItself = false;
            return value;
        }

        [ContextMenu("Load Default")]
        private void LoadDefault_Context()
        {
            Undo.RecordObject(this, $"{name} defaulting");
            LoadDefault();
        }

        [ContextMenu("Generate GUID")]
        private void GenerateGUID_Context()
        {
            Undo.RecordObject(this, "GUID generation");
            m_guid = GenerateGUID();
        }
        
        [ContextMenu("Generate SaveID")]
        private void GenerateSaveID_Context()
        {
            Undo.RecordObject(this, "SaveID generation");
            m_saveId = GenerateSaveID(name);
        }
        
        public void OnBeforeSerialize()
        {
            if (m_guid.IsNullOrEmpty()) m_guid = GenerateGUID();
            if (m_saveId.IsNullOrEmpty()) m_saveId = GenerateSaveID(name);
        }

        public void OnAfterDeserialize() { }
#endif
    }
}