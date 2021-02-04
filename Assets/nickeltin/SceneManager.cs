using System;
using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using nickeltin.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nickeltin.SceneManagment
{
    public class SceneManager : MonoSingleton<SceneManager>
    {
        private enum EventType
        {
            Regural,
            Error,
            Warning
        }

        [SerializeField] private bool m_logEvents = true;
        [SerializeField] private bool m_saveProgress = true;
        [SerializeField, Scene] private string m_uiScene;
        [SerializeField, Scene] private string m_initializeScene;
        [SerializeField] private int m_startFromLevelId;
        [SerializeField, Scene] private List<string> m_levels;

        public static event Action afterInitialization;
        public static event Action afterUILoaded;
        public static event Action<int> afterLevelLoad;
        public static event Action<int> afterLevelCompleted;
        public static event Action<int> beforeLevelReload;

        public static int currentLevelId { get; private set; } = 0;

        private const string levelId_key = "level_id";

        private void Load()
        {
            if (m_saveProgress) m_startFromLevelId = PlayerPrefs.GetInt(levelId_key, m_startFromLevelId);
        }

        private void Save(int? levelId = 0)
        {
            if (m_saveProgress)
            {
                PlayerPrefs.SetInt(levelId_key, Mathf.Clamp(levelId ?? currentLevelId, 0, m_levels.Count - 1));
            }
        }

        protected override void Awake()
        {
            if (Awake_Internal())
            {
                Load();

                if (m_uiScene != null)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(m_uiScene, LoadSceneMode.Additive);
                    LogEvent("UI loaded");
                    afterUILoaded?.Invoke();
                }

                if (m_initializeScene != null)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(m_initializeScene);
                    LogEvent("Initailzation scene loaded");
                    afterInitialization?.Invoke();
                }
                
                LoadLevel(m_startFromLevelId);
            }
        }

        private static void LogEvent(string text, EventType type = EventType.Regural)
        {
            if (instance.m_logEvents)
            {
                switch (type)
                {
                    case EventType.Regural:
                        Debug.Log(text);
                        break;
                    case EventType.Error:
                        Debug.LogError(text);
                        break;
                    case EventType.Warning:
                        Debug.LogWarning(text);
                        break;
                }
            }
        }

        private static int GetNextLevelId()
        {
            int nextId = currentLevelId + 1;
            if (nextId >= instance.m_levels.Count) nextId = 0;
            return nextId;
        }

        public static void CompleteLevel()
        {
            instance.Save(GetNextLevelId());
            afterLevelCompleted?.Invoke(currentLevelId);
        }

        public static void ReloadLevel()
        {
            beforeLevelReload?.Invoke(currentLevelId);
            LoadLevel(currentLevelId);
        }

        public static void LoadLevel(int levelId)
        {
            if (instance.m_levels[levelId] != null)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(instance.m_levels[levelId]);
                currentLevelId = levelId;
                LogEvent($"Level with id {levelId} loaded");
                afterLevelLoad?.Invoke(levelId);
            }
            else LogEvent($"Level with id {levelId}", EventType.Error);
        }

        public static void LoadNextLevel()
        {
            LoadLevel(GetNextLevelId());
        }
    }
}
