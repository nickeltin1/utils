using nickeltin.Extensions;
using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.GameData.DataObjects;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.GameData.Events
{
    public sealed class EventListener : MonoBehaviour
    {
        [SerializeField] private EventObject[] _events;
        [SerializeField] private UnityEvent _response;

        private void OnEnable() => _events.ForEach(e => e.BindEvent(OnInvoke));

        private void OnDisable() => _events.ForEach(e => e.UnbindEvent(OnInvoke));

        public void OnInvoke() => _response.Invoke();
    }
    
    public abstract class EventListener<T> : MonoBehaviour
    {
        public enum EventBinderType { EventObject, DataObject }

        [SerializeField] private EventBinderType _binderType;
        
        [SerializeField, ShowIf("_eventValidator")] private EventObject<T>[] _eventObjects;
        [SerializeField, HideIf("_eventValidator")] private DataObject<T>[] _dataObjects;
        [SerializeField] private UnityEvent<T> _response;

        private bool _eventValidator => _binderType == EventBinderType.EventObject;
        
        private void OnEnable()
        {
            if (_binderType == EventBinderType.EventObject)
            {
                for (int i = 0; i < _eventObjects.Length; i++) _eventObjects[i].BindEvent(OnInvoke);
            }
            else
            {
                for (int i = 0; i < _dataObjects.Length; i++) _dataObjects[i].BindEvent(OnInvoke);
            }
        }

        private void OnDisable()
        {
            if (_binderType == EventBinderType.EventObject)
            {
                for (int i = 0; i < _eventObjects.Length; i++) _eventObjects[i].UnbindEvent(OnInvoke);
            }
            else
            {
                for (int i = 0; i < _dataObjects.Length; i++) _dataObjects[i].UnbindEvent(OnInvoke);
            }
        }

        public virtual void OnInvoke(T data) => _response.Invoke(data);
    }
}