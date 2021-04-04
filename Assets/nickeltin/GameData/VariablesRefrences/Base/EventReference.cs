﻿using System;
using nickeltin.GameData.Events;
using nickeltin.GameData.GlobalVariables;
using nickeltin.GameData.Types;
using UnityEngine;
using Event = nickeltin.GameData.Types.Event;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public sealed class EventReference<T> : EventReferenceBase
    {
        [SerializeField] private EventObject<T> m_eventObject;
        [SerializeField] private GlobalVariable<Event<T>> m_globalEvent;

        public Event<T> Source
        {
            get
            {
                if (m_referenceType == ReferenceType.GlobalEvent) return m_globalEvent.Value;
                if (m_eventObject != null) return m_eventObject.Source;
                return null;
            }
        } 
    }
    
    [Serializable]
    public sealed class EventReference : EventReferenceBase
    {
        [SerializeField] private EventObject m_eventObject;
        [SerializeField] private GlobalVariable<Event> m_globalEvent;
        
        public Event Source
        {
            get
            {
                if (m_referenceType == ReferenceType.GlobalEvent) return m_globalEvent.Value;
                if (m_eventObject != null) return m_eventObject.Source;
                return null;
            }
        } 
    }
    
    [Serializable]
    public abstract class EventReferenceBase
    {
        [Serializable] public enum ReferenceType { EventObject, GlobalEvent }
        
        [SerializeField] protected ReferenceType m_referenceType = ReferenceType.EventObject;
    }
    
    
}