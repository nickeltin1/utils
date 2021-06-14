using System;
using UnityEngine;

namespace nickeltin.GameData.Saving
{
    public sealed partial class SaveSystem
    {
        private class AutoSaver : MonoBehaviour
        {
            private AutosavingSettings _settings;
            public event Action onBeforeAutoSave;

            private float _timePassed;

            public void Initialize(AutosavingSettings settings)
            {
                _settings = settings;
                DontDestroyOnLoad(gameObject);
            }

            private void Update()
            {
                if(!_settings.autosaveEnabled) return;
                
                _timePassed += Time.deltaTime;
                
                if (_timePassed >= _settings.autosaveInterval)
                {
                    _timePassed = 0;
                    InvokeSave();
                }
            }
            
            private void InvokeSave() => onBeforeAutoSave?.Invoke();

            private void OnApplicationFocus(bool hasFocus)
            {
                if (_settings.onApplicationLooseFocus && !hasFocus && !Application.isEditor) InvokeSave();
            }
            
            private void OnApplicationPause(bool pauseStatus)
            {
                if (_settings.onApplicationPause && pauseStatus) InvokeSave();
            }

            private void OnApplicationQuit()
            {
                if(_settings.onApplicationQuit) InvokeSave();
            }
        }
    }
}