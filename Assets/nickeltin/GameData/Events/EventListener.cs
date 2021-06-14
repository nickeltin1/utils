using nickeltin.Extensions;
using nickeltin.GameData.DataObjects;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Events
{
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventRef[] m_events;
        [SerializeField] private UnityEvent m_response;

        private void OnEnable()
        {
            m_events.ForEach(e => { if(e.HasSource) e.BindEvent(OnInvoke); });
        }

        private void OnDisable()
        {
            m_events.ForEach(e => { if(e.HasSource) e.UnbindEvent(OnInvoke); });
        }

        public void OnInvoke() => m_response.Invoke();
    }
    
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventRef<T>[] m_events;
        [SerializeField] private UnityEvent<T> m_response;

        private void OnEnable()
        {
            m_events.ForEach(e => { if(e.HasSource) e.BindEvent(OnInvoke); });
        }

        private void OnDisable()
        {
            m_events.ForEach(e => { if(e.HasSource) e.UnbindEvent(OnInvoke); });
        }

        public void OnInvoke(T data) => m_response.Invoke(data);
    }
}