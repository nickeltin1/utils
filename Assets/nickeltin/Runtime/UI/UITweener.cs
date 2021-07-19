using System;
using nickeltin.Extensions;
using nickeltin.Extensions.Attributes;
using nickeltin.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.Runtime.UI
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
            public bool useCurve;
            [AllowNesting, ShowIf("useCurve")] public AnimationCurve curve;
            public float alpha;

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

        public float duration
        {
            get => _duration;
            set => _duration = value;
        }

        private void OnEnable() => _events.onEnable.Invoke();

        private void OnDisable() => _events.onDisable.Invoke();

        private CanvasGroup _cachedCanvasGroup;
        
        private Vector2 _initPos;
        private Vector2 _initScale;
        private float _initAlpha;

        private bool _initialized;

        private void Awake()
        {
            if (_target.TryGetComponent(out CanvasGroup canvasGroup))
            {
                _initAlpha = canvasGroup.alpha;
                _cachedCanvasGroup = canvasGroup;
            }
        }

        private void Start()
        {
            _initPos = _target.anchoredPosition;
            _initScale = _target.localScale;
            
            _initialized = true;
        }

        private LTDescr ApplyTweenSettings(LTDescr tween)
        {
            if (_data.useCurve) tween.setEase(_data.curve);
            if (_loopPingPong) tween.setLoopPingPong();
            return tween;
        }

        public void Move()
        {
            if(!_initialized) return;
            ApplyTweenSettings(LeanTween.move(_target, _target.anchoredPosition + _data.offset, duration));
        }

        public void Scale()
        {
            if(!_initialized) return;
            ApplyTweenSettings(LeanTween.scale(_target, _data.scale, duration));
        }

        public void Alpha()
        {
            ApplyTweenSettings(LeanTween.alphaCanvas(_cachedCanvasGroup, _data.alpha, duration));
        }

        public void Cancel() => LeanTween.cancel(_target);

        public void CancelAndReset()
        {
            ResetTween();
            Cancel();
        }

        public void ResetTween()
        {
            if(!_initialized) return;
            _target.anchoredPosition = _initPos;
            _target.localScale = _initScale;
            if (_cachedCanvasGroup != null) _cachedCanvasGroup.alpha = _initAlpha;
        }
    }
}