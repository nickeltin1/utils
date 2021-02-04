using System;
using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = "Events/EventObject")]
    public sealed class EventObject : ScriptableObject
    {
        public event Action onInvoke;

        public void Invoke() => onInvoke?.Invoke();
        
#if UNITY_EDITOR
        [Button("Invoke", EButtonEnableMode.Playmode)]
        public void Invoke_Editor() => Invoke();
#endif
    }
}