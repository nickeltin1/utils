using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Events
{
    [AddComponentMenu("Events/EventListener")]
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventObject m_event;
        [SerializeField] private UnityEvent m_response;

        private void OnEnable() => m_event.onInvoke += OnInvoke;

        private void OnDisable() => m_event.onInvoke -= OnInvoke;

        private void OnInvoke() => m_response.Invoke();
    }
}