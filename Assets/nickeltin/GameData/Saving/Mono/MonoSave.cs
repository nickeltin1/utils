using System.Collections.Generic;
using nickeltin.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Saving
{
    /// <summary>
    /// State saver for MonoBehaviour's, can save <see cref="object"/>.
    /// Use <see cref="beforeSave"/> event to assign data object before saving.
    /// </summary>
    public class MonoSave : MonoBehaviour
    {
        private readonly static List<string> allMonoSaveGUIDs = new List<string>();
        
        [SerializeField] private SaveID _saveId;
        [SerializeField] private GenericSave _save;
        [SerializeField, Tooltip("Load and Save methods on target will be called automatically")] 
        private MonoSaveable _target;
        private object _data;
        
        public UnityEvent<MonoSave> afterLoad; 
        public UnityEvent<MonoSave> beforeSave;
        
        private bool _loaded = false;
        public bool SuccessfulyLoaded { get; private set; } = false;
        
        public object Data => _data;

        public SaveID SaveID => _saveId;
        
        private void OnEnable()
        {
            if (!_loaded) LoadValues();
        }
        private void Awake() => SaveSystem.onBeforeSave += SaveValues;
        private void OnDestroy() => SaveSystem.onBeforeSave -= SaveValues;
        
        private void SaveValues()
        {
            if (_target != null) _data = _target.Save();
            beforeSave.Invoke(this);
            _save.SetMonoSave(this);
        }
        private void LoadValues()
        {
            if (_saveId.ToString().IsNullOrEmpty())
            {
                Debug.LogError($"{name} doesn't have SaveID, generate it with context menu");
                return;
            } 
            
            if (allMonoSaveGUIDs.Contains(_saveId))
            {
                Debug.LogError($"{name} have same GUID as other item, regenerate it with context menu");
                return;
            }

            allMonoSaveGUIDs.Add(_saveId);

            if (_save.TryGetMonoSave(_saveId, out _data)) SuccessfulyLoaded = true;
            else
            {
                SuccessfulyLoaded = false;
            }
            
            if (_target != null) _target.Load(_data);
            afterLoad?.Invoke(this);
            _loaded = true;
        }
        
        public void SetData(object data) => _data = data;
        

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_saveId.SaveIDGenerated) _saveId.GenerateSaveID(name);
        }
#endif
    }
}