using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Events
{
    [AddComponentMenu("Events/VoidEventListener")]
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventObject m_event;
        [SerializeField] private UnityEvent m_response;

        private void OnEnable() => m_event.RegisterListener(this);

        private void OnDisable() => m_event.UnregisterListener(this);

        public void OnInvoke() => m_response.Invoke();
    }
}