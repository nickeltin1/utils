using nickeltin.Extensions;
using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.GameData.Events
{
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventRef[] _events;
        [SerializeField] private UnityEvent _response;

        private void OnEnable()
        {
            _events.ForEach(e => { if(e.HasSource) e.BindEvent(OnInvoke); });
        }

        private void OnDisable()
        {
            _events.ForEach(e => { if(e.HasSource) e.UnbindEvent(OnInvoke); });
        }

        public void OnInvoke() => _response.Invoke();
    }
    
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private EventRef<T>[] _events;
        [SerializeField] private UnityEvent<T> _response;

        private void OnEnable()
        {
            _events.ForEach(e => { if(e.HasSource) e.BindEvent(OnInvoke); });
        }

        private void OnDisable()
        {
            _events.ForEach(e => { if(e.HasSource) e.UnbindEvent(OnInvoke); });
        }

        public void OnInvoke(T data) => _response.Invoke(data);
    }
}