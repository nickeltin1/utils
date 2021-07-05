using nickeltin.Extensions;
using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.GameData.Events
{
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventObject[] _events;
        [SerializeField] private UnityEvent _response;

        private void OnEnable()
        {
            _events.ForEach(e => e.BindEvent(OnInvoke));
        }

        private void OnDisable()
        {
            _events.ForEach(e => e.UnbindEvent(OnInvoke));
        }

        public void OnInvoke() => _response.Invoke();
    }
    
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventObject<T>[] _events;
        [SerializeField] private UnityEvent<T> _response;

        private void OnEnable()
        {
            _events.ForEach(e => e.BindEvent(OnInvoke));
        }

        private void OnDisable()
        {
            _events.ForEach(e => e.UnbindEvent(OnInvoke));
        }

        public void OnInvoke(T data) => _response.Invoke(data);
    }
}