using System;
using nickeltin.Editor.Utility;
using nickeltin.GameData.Events.Types;
using UnityEngine;
using Event = nickeltin.GameData.Events.Types.Event;

namespace nickeltin.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(EventObject))]
    public sealed class EventObject : ScriptableObject, IEventBinder
    {
        [SerializeField] private Event _event;

        public void Invoke() => _event.Invoke();
        
        public void BindEvent(Action onEventInvoke) => _event.Bind(onEventInvoke);

        public void UnbindEvent(Action onEventInvoke) => _event.Unbind(onEventInvoke);
    }
    
    [Serializable]
    public abstract class EventObject<T> : ScriptableObject, IEventBinder<T>
    {
        [SerializeField] protected Event<T> _event;

        public void Invoke(T invokeData) => _event.Invoke(invokeData);
        
        public void BindEvent(Action<T> onEventInvoke) => _event.Bind(onEventInvoke);

        public void UnbindEvent(Action<T> onEventInvoke) => _event.Unbind(onEventInvoke);

        public virtual void Invoke() => _event.InvokeWithDefaultData();
    }
}