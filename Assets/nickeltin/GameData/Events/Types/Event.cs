using System;
using UnityEngine;

namespace nickeltin.GameData.Events.Types
{
    [Serializable]
    public class Event : EventBase
    {
        //Empty variable for making its object Serializable
        [SerializeField] private int _; 
        
        private event Action m_onInvoke;

        public void Bind(Action onInvoke) => m_onInvoke += onInvoke;

        public void Unbind(Action onInvoke) => m_onInvoke -= onInvoke;

        public void Invoke() => m_onInvoke?.Invoke();
        
        public override void InvokeWithDefaultData() => Invoke();
    }
    
    [Serializable]
    public class Event<T> : EventBase
    {
        [SerializeField] public T invokeData;
        
        private event Action<T> m_onInvoke;

        public void Bind(Action<T> onInvoke) => m_onInvoke += onInvoke;

        public void Unbind(Action<T> onInvoke) => m_onInvoke -= onInvoke;

        public void Invoke(T invokeData) => m_onInvoke?.Invoke(invokeData);
        public override void InvokeWithDefaultData() => Invoke(invokeData);
    }
    
    public abstract class EventBase
    {
        public abstract void InvokeWithDefaultData();
    }
}