using System;
using System.Collections;
using nickeltin.Extensions.Attributes;
using nickeltin.Extensions;
using nickeltin.Extensions.Types;
using nickeltin.Runtime.GameData.DataObjects;
using nickeltin.Runtime.GameData.VariablesRefrences;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.Runtime.UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBar : ValueObserverBase
    {
        [Serializable]
        public enum BarType { Linear, ImageFill }

        [SerializeField] private BarType _type;
        [SerializeField, HideIf("_usesSlider")] private Image _graphic;
        [SerializeField, ShowIf("_usesSlider")] private Slider _slider;
        [SerializeField] private FloatObject _source;
        [SerializeField] private bool _useSourceRawValue;
        [SerializeField] private Optional<float> _interpolationTime = 1;
        [SerializeField] private bool _useColorLerp;
        [SerializeField, ShowIf("_useColorLerp")] private Color _minColor;
        [SerializeField, ShowIf("_useColorLerp")] private Color _maxColor;
        
        
        private Coroutine _interpolation;
        
        private bool _usesSlider => _type == BarType.Linear;
        private bool _hasSource => _source != null;
        
        public float progress { get; private set; } = 0;

        private void OnValidate()
        {
            if(_usesSlider) ComponentExt.Cache(ref _slider, gameObject);
        }

        public void UpdateValueNonInterpolate(float normalizedValue)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);
            progress = normalizedValue;

            if (_type == BarType.Linear)
            {
                _slider.normalizedValue = normalizedValue;
                UpdateSliderColor(normalizedValue);
            }
            else if (_type == BarType.ImageFill)
            {
                _graphic.fillAmount = normalizedValue;
                if (_useColorLerp) _graphic.color = Color.Lerp(_minColor, _maxColor, normalizedValue);
            }

            InvokeUnityEvent();
        }

        private void UpdateSliderColor(float normalizedValue)
        {
            if(_useColorLerp) _slider.targetGraphic.color = Color.Lerp(_minColor, _maxColor, normalizedValue);
        }
        
        public void UpdateValue(float normalizedValue)
        {
            if(_interpolation != null) StopCoroutine(_interpolation);

            if (_interpolationTime.Enabled) _interpolation = StartCoroutine(Interpolation());
            else UpdateValueNonInterpolate(normalizedValue);

            IEnumerator Interpolation()
            {
                for (float t = 0; t < _interpolationTime; t+=Time.deltaTime)
                {
                    UpdateValueNonInterpolate(Mathf.Lerp(progress, normalizedValue,
                        t.To01Ratio(0, _interpolationTime)));
                    
                    yield return null;
                }
            }
        }

        private void UpdateValueFromSource(float sourceValue)
        {
            if(_useSourceRawValue) UpdateValue(sourceValue);
            else UpdateValue(_source.NormalizedValue);
        }

        private void OnEnable()
        {
            if (_hasSource)
            {
                _source.BindEvent(UpdateValueFromSource);
                UpdateValueFromSource(_source.Value);
            }
        }

        private void OnDisable()
        {
            if (_hasSource) _source.UnbindEvent(UpdateValueFromSource);
        }
    }
}