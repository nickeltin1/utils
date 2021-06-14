using System;
using nickeltin.GameData.Events;
using nickeltin.GameData.Events.Types;
using nickeltin.GameData.GlobalVariables;
using UnityEngine;
using Event = nickeltin.GameData.Events.Types.Event;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public sealed class EventRef<T> : EventReferenceBase, IEventBinder<T>
    {
        [SerializeField] private EventObject<T> m_eventObject;
        [SerializeField] private GlobalVar<Event<T>> m_globalEvent;

        public override bool HasSource
        {
            get
            {
                if (m_referenceType == ReferenceType.GlobalEvent) return m_globalEvent.HasRegistry;
                return m_eventObject != null;
            }
        }

        public void Invoke(T invokeData)
        {
            if(!HasSource) return;
            
            if(m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Invoke(invokeData);
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.Invoke(invokeData);
        }

        public void BindEvent(Action<T> onValueChanged)
        {
            if(m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Bind(onValueChanged);
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if(m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Bind(onValueChanged);
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.BindEvent(onValueChanged);
        }

    }
    
    [Serializable]
    public sealed class EventRef : EventReferenceBase, IEventBinder
    {
        [SerializeField] private EventObject m_eventObject;
        [SerializeField] private GlobalVar<Event> m_globalEvent;

        public override bool HasSource
        {
            get
            {
                if (m_referenceType == ReferenceType.GlobalEvent) return m_globalEvent.HasRegistry;
                return m_eventObject != null;
            }
        }


        public void Invoke()
        {
            if(!HasSource) return;
            
            if(m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Invoke();
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.Invoke();
        }

        public void BindEvent(Action onValueChanged)
        {
            if (m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Bind(onValueChanged);
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.BindEvent(onValueChanged);
        }

        public void UnbindEvent(Action onValueChanged)
        {
            if(m_referenceType == ReferenceType.GlobalEvent) m_globalEvent.Value.Bind(onValueChanged);
            else if (m_referenceType == ReferenceType.EventObject) m_eventObject.BindEvent(onValueChanged);
        }
    }
    
    [Serializable]
    public abstract class EventReferenceBase
    {
        [Serializable] public enum ReferenceType { EventObject, GlobalEvent }
        
        [SerializeField] protected ReferenceType m_referenceType = ReferenceType.EventObject;
        
        public abstract bool HasSource { get; }
    }
}