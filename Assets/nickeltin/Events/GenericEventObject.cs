using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Events
{
    public abstract class GenericEventObject<T> : ScriptableObject
    {
        [SerializeField] private T m_invokeData;
        [SerializeField] protected List<GenericEventListener<T>> m_listeners;
        public event Action<T> onInvoke; 

        public virtual void Invoke([Optional] T data)
        {
            onInvoke?.Invoke(data);
            for (int i = m_listeners.Count - 1; i >= 0; i--) m_listeners[i].OnInvoke(data);
        }

        public virtual void RegisterListener(GenericEventListener<T> listener)
        {
            if (!m_listeners.Contains(listener)) m_listeners.Add(listener);
        }

        public virtual void UnregisterListener(GenericEventListener<T> listener)
        {
            if (m_listeners.Contains(listener)) m_listeners.Remove(listener);
        }
        
#if UNITY_EDITOR
        [Button("Invoke", EButtonEnableMode.Playmode)]
        public void Invoke_Editor() => Invoke(m_invokeData);
#endif
    }
}