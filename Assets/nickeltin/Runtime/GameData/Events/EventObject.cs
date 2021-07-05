using System;
using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.NestedAssets;
using UnityEngine;
using Event = nickeltin.Runtime.GameData.Events.Event;

namespace nickeltin.Runtime.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(EventObject))]
    public sealed class EventObject : EventObjectBase, IEventBinder
    {
        [SerializeField] private Events.Event _event;

        public void Invoke() => _event.Invoke();
        
        public void BindEvent(Action onEventInvoke) => _event.Bind(onEventInvoke);

        public void UnbindEvent(Action onEventInvoke) => _event.Unbind(onEventInvoke);
    }
    
    [Serializable]
    public abstract class EventObject<T> : EventObjectBase, IEventBinder<T>
    {
        [SerializeField] protected Event<T> _event;

        public void Invoke(T invokeData) => _event.Invoke(invokeData);
        
        public void BindEvent(Action<T> onEventInvoke) => _event.Bind(onEventInvoke);

        public void UnbindEvent(Action<T> onEventInvoke) => _event.Unbind(onEventInvoke);

        public virtual void Invoke() => _event.InvokeWithDefaultData();
    }
    
    public abstract class EventObjectBase : NestedAsset {}
}