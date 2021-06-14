using System;
using nickeltin.Extensions;
using nickeltin.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UITweener : MonoBehaviour
    {
        [Serializable]
        private struct Events
        {
            public UnityEvent onEnable;
            public UnityEvent onDisable;
        }
        
        [Serializable]
        private class Data
        {
            public Vector2 offset;
            public Vector2 scale;
            public AnimationCurve curve;

            public Data()
            {
                this.curve = AnimationCurve.Linear(0,0,1,1);
            }
        }
        
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _duration;
        [SerializeField] private bool _loopPingPong;
        [SerializeField] private Events _events;
        [SerializeField] private Data _data;
        
        private void OnEnable() => _events.onEnable.Invoke();

        private void OnDisable() => _events.onDisable.Invoke();

        private Vector2 _initPos;
        private Vector2 _initScale;
        
        private void Awake()
        {
            _initPos = _target.anchoredPosition;
            _initScale = _target.localScale;
        }

        public void Move()
        {
            var tween = LeanTween.move(_target, _target.anchoredPosition + _data.offset, _duration);
            if (_loopPingPong) tween.setLoopPingPong();
        }

        public void Scale()
        {
            var tween = LeanTween.scale(_target, _data.scale, _duration);
            if (_loopPingPong) tween.setLoopPingPong();
        }

        public void Cancel()
        {
            LeanTween.cancel(_target);
            ResetTween();
        }

        public void ResetTween()
        {
            _target.anchoredPosition = _initPos;
            _target.localScale = _initScale;
        }
        
    }
}