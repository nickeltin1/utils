using System;
using nickeltin.Events;
using nickeltin.Experimental.GlobalVariables;
using nickeltin.Experimental.GlobalVariables.Types;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public class EventReference<T>
    {
        public enum ReferenceType
        {
            EventObject, GlobalEvent    
        }
        
        [SerializeField] private GenericEventObject<T> m_eventObject;
        [SerializeField] private GlobalVariable<Event<T>> m_globalEvent;
    }
}