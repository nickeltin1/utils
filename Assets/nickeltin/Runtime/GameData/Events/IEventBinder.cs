using System;

namespace nickeltin.Runtime.GameData.Events
{
    public interface IEventBinder
    {
        void BindEvent(Action onEventInvoke);
        void UnbindEvent(Action onEventInvoke);
    }
    
    public interface IEventBinder<out T>
    {
        void BindEvent(Action<T> onEventInvoke);
        void UnbindEvent(Action<T> onEventInvoke);
    }
}