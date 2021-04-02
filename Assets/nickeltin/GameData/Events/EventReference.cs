using System;
using nickeltin.Experimental.GlobalVariables;
using nickeltin.Experimental.GlobalVariables.Types;
using UnityEngine;

namespace nickeltin.Events
{
    [Serializable]
    public class EventReference<T>
    {
        [SerializeField] private GlobalVariable<Event<T>> m_globalEvent;
        [SerializeField] private GenericEventObject<T> m_eventObject;
    }
}