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

        private LTDescr ApplySettingsToTween(LTDescr tween)
        {
            if (_data.useCurve) tween.setEase(_data.curve);
            if (_loopPingPong) tween.setLoopPingPong();
            return tween;
        }
        
        public void Move() => ApplySettingsToTween(LeanTween.move(_target, _target.anchoredPosition + _data.offset, _duration));

        public void Scale() => ApplySettingsToTween(LeanTween.scale(_target, _data.scale, _duration));

        public void Cancel() => LeanTween.cancel(_target);

        public void CancelAndReset()
        {
            ResetTween();
            Cancel();
        }

        public void ResetTween()
        {
            _target.anchoredPosition = _initPos;
            _target.localScale = _initScale;
        }
    }
}