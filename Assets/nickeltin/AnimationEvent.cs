using System.Collections.Generic;
using nickeltin.Editor.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace NPCs
{
    public class AnimationEvent : MonoBehaviour
    {
        [ReorderableList] public List<UnityEvent> events;

        public void AnimationEventInvoke(int eventIndex) => events[eventIndex].Invoke();
        
        #if UNITY_EDITOR

        [ContextMenu("Copy invokable method name")]
        private void CopyMethodName()
        {
            EditorGUIUtility.systemCopyBuffer = "AnimationEventInvoke";
        }
        
        #endif
    }
}