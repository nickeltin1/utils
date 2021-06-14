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
        [SerializeField] private Event m_event;

        public void Invoke() => m_event.Invoke();
        
        public void BindEvent(Action onEventInvoke) => m_event.Bind(onEventInvoke);

        public void UnbindEvent(Action onEventInvoke) => m_event.Unbind(onEventInvoke);
    }
    
    [Serializable]
    public abstract class EventObject<T> : ScriptableObject, IEventBinder<T>
    {
        [SerializeField] protected Event<T> m_event;

        public void Invoke(T invokeData) => m_event.Invoke(invokeData);
        
        public void BindEvent(Action<T> onEventInvoke) => m_event.Bind(onEventInvoke);

        public void UnbindEvent(Action<T> onEventInvoke) => m_event.Unbind(onEventInvoke);

        public virtual void Invoke() => m_event.InvokeWithDefaultData();
    }
}