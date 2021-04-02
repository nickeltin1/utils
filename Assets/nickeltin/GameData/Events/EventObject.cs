using nickeltin.Editor.Utility;
using UnityEngine;
using Event = nickeltin.Experimental.GlobalVariables.Types.Event;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(EventObject))]
    public sealed class EventObject : ScriptableObject
    {
        [SerializeField] private Event m_event;

        public Event Source => m_event;
        
        public void Invoke() => m_event.Invoke();
    }
}