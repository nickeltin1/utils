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
        public event Action<T> onInvoke; 

        public virtual void Invoke([Optional] T data) => onInvoke?.Invoke(data);


#if UNITY_EDITOR
        [Button("Invoke", EButtonEnableMode.Playmode)]
        public void Invoke_Editor() => Invoke(m_invokeData);
#endif
    }
}