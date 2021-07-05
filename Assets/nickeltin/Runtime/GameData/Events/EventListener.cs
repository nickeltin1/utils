using System;
using System.Collections.Generic;
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
                ForEachEventBinder(_eventObjects, b => b.BindEvent(OnInvoke));
            }
            else
            {
                ForEachEventBinder(_dataObjects, b => b.BindEvent(OnInvoke));
            }
        }

        private void OnDisable()
        {
            if (_binderType == EventBinderType.EventObject)
            {
                ForEachEventBinder(_eventObjects, b => b.UnbindEvent(OnInvoke));
            }
            else
            {
                ForEachEventBinder(_dataObjects, b => b.UnbindEvent(OnInvoke));
            }
        }

        private void ForEachEventBinder(IReadOnlyList<IEventBinder<T>> binders, Action<IEventBinder<T>> action)
        {
            for (int i = 0; i < binders.Count; i++) action.Invoke(binders[i]);
        }

        public void OnInvoke(T data) => _response.Invoke(data);
    }
}