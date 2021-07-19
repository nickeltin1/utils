using System;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Events
{
    [Serializable]
    public class Event : EventBase
    {
        //Empty variable for making object Serializable
        [SerializeField] private int _; 
        
        private event Action _onInvoke;

        public void Bind(Action onInvoke) => _onInvoke += onInvoke;

        public void Unbind(Action onInvoke) => _onInvoke -= onInvoke;

        public void Invoke() => _onInvoke?.Invoke();
        
        public override void InvokeWithDefaultData() => Invoke();
    }
    
    [Serializable]
    public class Event<T> : EventBase
    {
        [SerializeField] public T invokeData;
        
        private event Action<T> _onInvoke;

        public void Bind(Action<T> onInvoke) => _onInvoke += onInvoke;

        public void Unbind(Action<T> onInvoke) => _onInvoke -= onInvoke;

        public void Invoke(T invokeData) => _onInvoke?.Invoke(invokeData);
        public override void InvokeWithDefaultData() => Invoke(invokeData);

#if UNITY_EDITOR
        public static string invoke_data_prop_name => nameof(invokeData);
#endif
    }
    
    public abstract class EventBase
    {
        public abstract void InvokeWithDefaultData();
    }
}