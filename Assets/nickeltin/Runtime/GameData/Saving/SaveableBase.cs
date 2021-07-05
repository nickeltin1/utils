using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Saving
{
    /// <summary>
    /// Savable without type, scriptable object. 
    /// </summary>
    public abstract class SaveableBase : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private SaveID _saveID;

        public SaveID SaveID => _saveID;
        
        public bool SuccessfulyLoaded { get; private set; } = false;

        public virtual bool Register(SaveSystem saveSystem) => saveSystem.AddSavedItem(this);
        /// <param name="loadDefault">Force to apply default state to save</param>
        /// <returns>Is load successful</returns>
        public abstract bool Load(SaveSystem saveSystem);
        public abstract void Save(SaveSystem saveSystem);
        public abstract void LoadDefault();

        public virtual void SetLoadState(bool state) => SuccessfulyLoaded = state;
        public abstract void SetFileWithoutType(SaveSystem saveSystem, object file);
        public abstract object GetFileWithoutType();

        public void OnBeforeSerialize()
        {
            if (_saveID != null && !_saveID.SaveIDGenerated)  _saveID.GenerateSaveID(name);
        }
        public void OnAfterDeserialize() { }
        
#if UNITY_EDITOR
        /// <summary>Method for editor, do not use it</summary>
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
#endif
    }
}