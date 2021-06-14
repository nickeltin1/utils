using System;
using System.Collections.Generic;
using nickeltin.Editor.Types;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.Events;
using nickeltin.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nickeltin.SceneManagment
{
    [DisallowMultipleComponent, AddComponentMenu("ScenesManagment/SceneManager")]
    public class SceneManager : MonoSingleton<SceneManager>
    {
        private enum EventType
        {
            Regural,
            Error,
            Warning
        }
        
        [Serializable]
        private struct Events
        {
            public EventRef afterInitialization;
            public EventRef afterUILoaded;
            public EventRef<int> afterLevelLoad;
            public EventRef<int> afterLevelCompleted;
            public EventRef<int> beforeLevelReload;
        }
        
        [Serializable]
        private class Settings
        {
            public bool logEventsToConsole = true;
            public bool saveProgress = true;
            public int startFromLevelId;
            public bool loadTutorialAfterInitialization = true;
            public bool loadUI = true;
        }
        
        [Serializable]
        private struct BaseScenes
        {
            public SceneReference initializeScene;
            public SceneReference uiScene;
            public SceneReference tutorialScene;
        }
        
        [SerializeField] private Settings m_settings;
        [SerializeField] private Events m_events;
        [SerializeField] private BaseScenes m_baseScenes;
        [SerializeField] private List<SceneReference> m_levels;

        public static event Action afterInitialization;
        public static event Action afterUILoaded;
        
        /// <summary>Level Id </summary>
        public static event Action<int> afterLevelLoad;
        /// <summary>Level Id </summary>
        public static event Action<int> afterLevelCompleted;
        /// <summary>Level Id </summary>
        public static event Action<int> beforeLevelReload;

        public static int currentLevelId { get; private set; } = 0;
        public static int levelsCompleted { get; private set; } = 0;
        public static bool tutorialCompleted { get; private set; } = false;
        public static string currentScene { get; private set; }

        private const string levelId_key = "level_id";
        private const string levelsCompleted_key = "levels_completed_count";
        private const string tutorialState_key = "tutorial_state";

        // private Scene _originScene;
        

        private void Load()
        {
            if (m_settings.saveProgress)
            {
                m_settings.startFromLevelId = PlayerPrefs.GetInt(levelId_key, m_settings.startFromLevelId);
                levelsCompleted = PlayerPrefs.GetInt(levelsCompleted_key, 0);
                tutorialCompleted = PlayerPrefs.GetInt(tutorialState_key, 0) >= 1;
            }
        }

        private void Save(int? levelId = 0)
        {
            if (m_settings.saveProgress)
            {
                PlayerPrefs.SetInt(levelId_key, Mathf.Clamp(levelId ?? currentLevelId, 0, m_levels.Count - 1));
                PlayerPrefs.SetInt(levelsCompleted_key, levelsCompleted);
                PlayerPrefs.SetInt(tutorialState_key, tutorialCompleted ? 1 : 0);
            }
        }

        readonly List<string> _loadedScenes = new List<string>();
        
        protected override void Awake()
        {
            if (Awake_Internal())
            {
               
                int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
                for (int i = 0; i < sceneCount; i++)
                {
                    _loadedScenes.Add(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path);
                }

                Load();

                if (m_baseScenes.initializeScene != null)
                {
                    if (!_loadedScenes.Contains(m_baseScenes.initializeScene))
                    {
                        LoadScene(m_baseScenes.initializeScene);
                    }
                    LogEvent("Initailzation scene loaded");
                    afterInitialization?.Invoke();
                    m_events.afterInitialization.Invoke();
                }
                
                if (m_settings.loadUI && m_baseScenes.uiScene != null)
                {
                    if (!_loadedScenes.Contains(m_baseScenes.uiScene))
                    {
                        LoadScene(m_baseScenes.uiScene, LoadSceneMode.Additive);
                    }
                    LogEvent("UI loaded");
                    afterUILoaded?.Invoke();
                    m_events.afterUILoaded.Invoke();
                }
            }
        }

        private void Start()
        {
            if (m_settings.loadTutorialAfterInitialization && !tutorialCompleted && m_baseScenes.tutorialScene != null)
            {
                currentLevelId = -1;
                LoadScene(m_baseScenes.tutorialScene);
                afterLevelLoad?.Invoke(-1);
                instance.m_events.afterLevelLoad.Invoke(-1);
            }
            else LoadLevel(m_settings.startFromLevelId);
        }

        private void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
            if (mode.Equals(LoadSceneMode.Single))
            {
                currentScene = sceneName;
            }
        }

        private void LogEvent(string text, EventType type = EventType.Regural)
        {
            if (m_settings.logEventsToConsole)
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
            int nextId = currentLevelId + 1;
            if (nextId >= m_levels.Count) nextId = 0;
            return nextId;
        }

        public void CompleteLevel()
        {
            levelsCompleted++;
            Save(GetNextLevelId());
            afterLevelCompleted?.Invoke(currentLevelId);
            m_events.afterLevelCompleted.Invoke(currentLevelId);
        }

        public void CompleteTutorial()
        {
            tutorialCompleted = true;
            Save(m_settings.startFromLevelId);
            afterLevelCompleted?.Invoke(-1);
            m_events.afterLevelCompleted.Invoke(-1);
        }

        public void ReloadLevel()
        {
            beforeLevelReload?.Invoke(currentLevelId);
            m_events.beforeLevelReload.Invoke(currentLevelId);
            LoadLevel(currentLevelId);
        }

        public void LoadLevel(int levelId)
        {
            void Load(string sceneName)
            {
                LoadScene(sceneName);
                currentLevelId = levelId;
                LogEvent($"Level with id {levelId} loaded");
                afterLevelLoad?.Invoke(levelId);
                m_events.afterLevelLoad.Invoke(levelId);
            }

            if (levelId < 0) Load(m_baseScenes.tutorialScene);
            else if (m_levels[levelId] != null) Load(m_levels[levelId]);
            else LogEvent($"Level with id {levelId}", EventType.Error);
        }

        public void LoadNextLevel() => LoadLevel(GetNextLevelId());
    }
}
