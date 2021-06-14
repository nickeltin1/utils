using System;

namespace nickeltin.GameData.Events.Types
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