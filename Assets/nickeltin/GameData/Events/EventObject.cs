using System;
using nickeltin.Editor.Utility;
using nickeltin.GameData.Types;
using UnityEngine;
using Event = nickeltin.GameData.Types.Event;

namespace nickeltin.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(EventObject))]
    public sealed class EventObject : ScriptableObject
    {
        [SerializeField] private Event m_event;

        public Event Source => m_event;
        
        public void Invoke() => m_event.Invoke();
    }
    
    [Serializable]
    public abstract class EventObject<T> : ScriptableObject
    {
        [SerializeField] private Event<T> m_event;

        public Event<T> Source => m_event;
        
        public void Invoke(T invokeData) => m_event.Invoke(invokeData);
    }
}