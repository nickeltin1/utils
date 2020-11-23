#pragma warning disable

using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent m_event;
        [SerializeField] private UnityEvent m_response;

        private void OnEnable() => m_event.RegisterListener(this);

        private void OnDisable() => m_event.UnregisterListener(this);

        public void OnEventRaised() => m_response.Invoke();
    }
}