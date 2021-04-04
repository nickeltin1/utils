using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.GameData.Events
{
    public class AnimationEvent : MonoBehaviour
    {
        public List<UnityEvent> events;

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