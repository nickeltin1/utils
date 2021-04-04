using System;
using UnityEngine;

namespace nickeltin.GameData.Types
{
    [Serializable]
    public class Event : EventBase
    {
        //Empty variable for making it objects Serializable
        [SerializeField] private int _; 
        
        public event Action onInvoke;

        public void Invoke() => onInvoke?.Invoke();
        
        public override void Invoke_Editor() => Invoke();
    }
    
    [Serializable]
    public class Event<T> : EventBase
    {
        [SerializeField] private T invokeData;
        
        public event Action<T> onInvoke;
        
        public void Invoke(T invokeData) => onInvoke?.Invoke(invokeData);
        public override void Invoke_Editor() => Invoke(invokeData);
    }
    
    public abstract class EventBase
    {
        public abstract void Invoke_Editor();
    }
}