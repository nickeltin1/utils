using System;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.GameData.GlobalVariables;
using UnityEngine;
using Event = nickeltin.Runtime.GameData.Events.Event;

namespace nickeltin.Runtime.GameData.VariablesRefrences
{
    [Serializable]
    public sealed class EventRef<T> : EventReferenceBase, IEventBinder<T>
    {
        [SerializeField] private EventObject<T> _eventObject;
        [SerializeField] private GlobalVar<Event<T>> _globalEvent;

        public override bool HasSource
        {
            get
            {
                if (_referenceType == ReferenceType.GlobalEvent) return _globalEvent.HasRegistry;
                return _eventObject != null;
            }
        }

        public void Invoke(T invokeData)
        {
            if(!HasSource) return;
            
            if(_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Invoke(invokeData);
            else if (_referenceType == ReferenceType.EventObject) _eventObject.Invoke(invokeData);
        }

        public void BindEvent(Action<T> onValueChanged)
        {
            if(_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Bind(onValueChanged);
            else if (_referenceType == ReferenceType.EventObject) _eventObject.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if(_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Bind(onValueChanged);
            else if (_referenceType == ReferenceType.EventObject) _eventObject.BindEvent(onValueChanged);
        }

    }
    
    [Serializable]
    public sealed class EventRef : EventReferenceBase, IEventBinder
    {
        [SerializeField] private EventObject _eventObject;
        [SerializeField] private GlobalVar<Event> _globalEvent;

        public override bool HasSource
        {
            get
            {
                if (_referenceType == ReferenceType.GlobalEvent) return _globalEvent.HasRegistry;
                return _eventObject != null;
            }
        }


        public void Invoke()
        {
            if(!HasSource) return;
            
            if(_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Invoke();
            else if (_referenceType == ReferenceType.EventObject) _eventObject.Invoke();
        }

        public void BindEvent(Action onValueChanged)
        {
            if (_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Bind(onValueChanged);
            else if (_referenceType == ReferenceType.EventObject) _eventObject.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action onValueChanged)
        {
            if(_referenceType == ReferenceType.GlobalEvent) _globalEvent.Value.Bind(onValueChanged);
            else if (_referenceType == ReferenceType.EventObject) _eventObject.BindEvent(onValueChanged);
        }
    }
    
    [Serializable]
    public abstract class EventReferenceBase
    {
        [Serializable] public enum ReferenceType { EventObject, GlobalEvent }
        
        [SerializeField] protected ReferenceType _referenceType = ReferenceType.EventObject;
        
        public abstract bool HasSource { get; }
    }
}