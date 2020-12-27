using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Events
{
    public abstract class GenericEventListener<T> : MonoBehaviour
    {
        [SerializeField] private GenericEventObject<T> m_event;
        [SerializeField] private UnityEvent<T> m_response;

        private void OnEnable() => m_event.RegisterListener(this);

        private void OnDisable() => m_event.UnregisterListener(this);

        public void OnInvoke([Optional] T data) => m_response.Invoke(data);
    }
}