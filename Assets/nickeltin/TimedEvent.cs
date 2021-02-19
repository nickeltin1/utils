using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.Other
{
    public class TimedEvent
    {
        private class TimedEventInstance : MonoBehaviour { }

        private static TimedEventInstance m_instance;
        private static TimedEventInstance Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new GameObject("timed_events", typeof(TimedEventInstance))
                        .GetComponent<TimedEventInstance>();
                    m_instance.gameObject.hideFlags = HideFlags.HideAndDontSave;
                    Object.DontDestroyOnLoad(m_instance.gameObject);
                }
                
                return m_instance;
            }
        }

        private readonly float m_time;
        private readonly Action m_action;
        private readonly MonoBehaviour m_owner;
        public bool IsRunning { get; private set; } = false;
        public float Progress { get; private set; }= 1f;
        
        /// <summary>
        /// Returns 0 to 1 ratio, displaying timer progress
        /// </summary>
        public event Action<float> onTimerTick;
        
        private Coroutine timerTicker;
        
        public TimedEvent(MonoBehaviour owner, float time, Action onTimesUp = null)
        {
            this.m_time = time;
            this.m_action = onTimesUp;
            this.m_owner = owner;
        }
        
        public void Start([Optional][DefaultParameterValue(-1)] float t, [Optional][DefaultParameterValue(null)] Action onTimesUp)
        {
            if(IsRunning) return;
            timerTicker = m_owner.StartCoroutine(Execute(t == -1 ? m_time : t, onTimesUp ?? m_action));
        }

        public void Stop()
        {
            if (timerTicker != null) m_owner.StopCoroutine(timerTicker);
            timerTicker = null;
            IsRunning = false;
            onTimerTick?.Invoke(1);
            Progress = 1;
        }
        
        private IEnumerator Execute(float t, Action onTimesUp)
        {
            IsRunning = true;
            Progress = 0;
            
            float cooldown = 0;
            float rate = (1 / t);
            while (cooldown <= 1)
            {
                cooldown += Time.deltaTime * rate;
                onTimerTick?.Invoke(cooldown);
                Progress = cooldown;
                yield return null;
            }
            
            IsRunning = false;
            onTimesUp?.Invoke();
            Progress = 1;
        }

        public static TimedEvent Start(float t, Action onTimesOut = null, MonoBehaviour owner = null)
        {
            MonoBehaviour o = owner == null ? TimedEvent.Instance : owner;
            var i = new TimedEvent(o, t, onTimesOut);
            i.Start();
            return i;
        }
    }
}