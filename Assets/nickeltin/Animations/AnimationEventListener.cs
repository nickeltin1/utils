using System;
using System.Collections.Generic;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Animations
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimationEventListener : MonoBehaviour
    {
        [Serializable]
        private struct AnimationEvent
        {
            public AnimationEventIdentifier identifer;
            public UnityEvent callback;
        }

        [SerializeField] private AnimationEvent[] _events;

        private Dictionary<AnimationEventIdentifier, UnityEvent> _eventsDictionary;
        
        private void Awake()
        {
            _eventsDictionary = new Dictionary<AnimationEventIdentifier, UnityEvent>();
            _events.ForEach(e =>
            {
                if (_eventsDictionary.ContainsKey(e.identifer))
                {
                    throw new Exception($"{nameof(AnimationEventListener)} on gameobject {gameObject.name} " +
                                        $"already contains {nameof(AnimationEvent)} with identifier {e.identifer.name}");
                }
                _eventsDictionary.Add(e.identifer, e.callback);
            });
        }

        public void AnimationEventInvoke(AnimationEventIdentifier identifier)
        {
            if (!_eventsDictionary.ContainsKey(identifier))
            {
                throw new Exception($"{nameof(AnimationEventIdentifier)} with name {identifier.name} not found");
            }
            _eventsDictionary[identifier].Invoke();
        }
        
#if UNITY_EDITOR
        
        [ContextMenu("Copy invokable method name")]
        private void CopyMethodName() => EditorGUIUtility.systemCopyBuffer = nameof(AnimationEventInvoke);
#endif
    }
}