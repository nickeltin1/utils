using System.Collections.Generic;
using System.Runtime.InteropServices;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Events
{
    public abstract class GenericEventObject<T> : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private T m_invokeData;
        [Button("Invoke", EButtonEnableMode.Playmode)]
        public void Invoke_Editor() => Invoke(m_invokeData);
#endif
        private readonly List<GenericEventListener<T>> m_listeners = new List<GenericEventListener<T>>();

        public void Invoke([Optional] T data)
        {
            for(int i = m_listeners.Count -1; i >= 0; i--) m_listeners[i].OnInvoke(data);
        }

        public void RegisterListener(GenericEventListener<T> listener)
        {
            if (!m_listeners.Contains(listener)) m_listeners.Add(listener);
        }

        public void UnregisterListener(GenericEventListener<T> listener)
        {
            if (m_listeners.Contains(listener)) m_listeners.Remove(listener);
        }
    }
}