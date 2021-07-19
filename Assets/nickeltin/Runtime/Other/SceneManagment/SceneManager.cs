using System;
using System.Collections.Generic;
using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.GameData.VariablesRefrences;
using nickeltin.Runtime.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nickeltin.Runtime.SceneManagment
{
    [DisallowMultipleComponent, AddComponentMenu("ScenesManagment/SceneManager")]
    public partial class SceneManager : MonoSingleton<SceneManager>
    {
        private enum EventType
        {
            Regural,
            Error,
            Warning
        }

        [Serializable]
        private struct Values
        {
            public VariableRef<int> currentLevelId;
            public VariableRef<int> levelsCompleted;
        }
        
        [SerializeField] private Settings _settings;
        [SerializeField] private Events _events;
        [SerializeField] private BaseScenes _baseScenes;
        [SerializeField, Scene] private List<string> _levels;
        [SerializeField] private Values _values;


        readonly List<string> _loadedScenes = new List<string>();
        
        protected override void Awake()
        {
            if (Awake_Internal())
            {
                int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
                for (int i = 0; i < sceneCount; i++)
                {
                    _loadedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).name);
                }
                
                if (_baseScenes.initializeScene != null)
                {
                    if (!_loadedScenes.Contains(_baseScenes.initializeScene))
                    {
                        LoadScene(_baseScenes.initializeScene);
                    }
                    LogEvent("Initailzation scene loaded");
                }
                
                if (_settings.loadUI && _baseScenes.uiScene != null)
                {
                    if (!_loadedScenes.Contains(_baseScenes.uiScene))
                    {
                        LoadScene(_baseScenes.uiScene, LoadSceneMode.Additive);
                    }
                    LogEvent("UI loaded");
                }
            }
        }

        private void Start()
        {
            LoadLevel(_values.currentLevelId);
        }

        private void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
        }

        private void LogEvent(string text, EventType type = EventType.Regural)
        {
            if (_settings.logEventsToConsole)
            {
                switch (type)
                {
                    case EventType.Regural: Debug.Log(text); break;
                    case EventType.Error: Debug.LogError(text); break;
                    case EventType.Warning: Debug.LogWarning(text); break;
                }
            }
        }

        private int GetNextLevelId()
        {
            int nextId = _values.currentLevelId + 1;
            if (nextId >= _levels.Count) nextId = 0;
            return nextId;
        }

        public void CompleteLevel()
        {
            if (_events.beforeLevelCompleted != null) _events.beforeLevelCompleted.Invoke();
            _values.levelsCompleted.Value++;
            if (_events.afterLevelCompleted != null) _events.afterLevelCompleted.Invoke();
        }

        public void ReloadLevel()
        {
            if (_events.beforeLevelReload != null) _events.beforeLevelReload.Invoke();
            LoadLevel(_values.currentLevelId);
        }

        public void LoadLevel(int levelId)
        {
            void Load(string sceneName)
            {
                LoadScene(sceneName);
                _values.currentLevelId.Value = levelId;
                LogEvent($"Level with id {levelId} loaded");
                if (_events.afterLevelLoad != null) _events.afterLevelLoad.Invoke();
            }
            
            if (_levels[levelId] != null) Load(_levels[levelId]);
            else LogEvent($"Level with id {levelId}", EventType.Error);
        }

        public void LoadNextLevel() => LoadLevel(GetNextLevelId());
    }
}
