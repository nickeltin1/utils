using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = "Events/VoidEvent")]
    public sealed class EventObject : ScriptableObject
    {
#if UNITY_EDITOR
        [Button("Invoke", EButtonEnableMode.Playmode)]
        public void Invoke_Editor() => Invoke();
#endif
        private readonly List<EventListener> m_listeners = new List<EventListener>();

        public void Invoke()
        {
            for(int i = m_listeners.Count -1; i >= 0; i--) m_listeners[i].OnInvoke();
        }

        public void RegisterListener(EventListener listener)
        {
            if (!m_listeners.Contains(listener)) m_listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            if (m_listeners.Contains(listener)) m_listeners.Remove(listener);
        }
    }
}