using nickeltin.GameData.DataObjects;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Events
{
    [AddComponentMenu("Events/EventListener")]
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventRef m_event;
        [SerializeField] private UnityEvent m_response;

        private void OnEnable()
        {
            if (m_event.Source != null) m_event.Source.onInvoke += OnInvoke;
        }

        private void OnDisable()
        {
            if (m_event.Source != null) m_event.Source.onInvoke -= OnInvoke;
        }

        public void OnInvoke() => m_response.Invoke();
    }
    
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventRef<T> m_event;
        [SerializeField] private UnityEvent<T> m_response;

        private void OnEnable()
        {
            if (m_event.Source != null) m_event.Source.onInvoke += OnInvoke;
        }

        private void OnDisable()
        {
            if (m_event.Source != null) m_event.Source.onInvoke -= OnInvoke;
        }

        public void OnInvoke(T data) => m_response.Invoke(data);
    }
}