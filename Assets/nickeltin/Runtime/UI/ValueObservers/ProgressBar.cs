using System;
using System.Collections;
using nickeltin.Extensions.Attributes;
using nickeltin.Extensions;
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
        [SerializeField] private VarObjRef<float> _source;
        [SerializeField] private float _interpolationTime = 1;
        [SerializeField] private bool _useColorLerp;
        [SerializeField, ShowIf("_useColorLerp")] private Color _minColor;
        [SerializeField, ShowIf("_useColorLerp")] private Color _maxColor;
        
        private Slider _slider;

        private Slider slider
        {
            get
            {
                if (_slider == null) _slider = GetComponent<Slider>();
                return _slider;
            } 
        }
        
        private Coroutine _interpolation;
        
        private bool _usesSlider => _type == BarType.Linear;
        
        public float progress { get; private set; } = 0;

        public event Action onFill;

        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValueNonInterpolate(float normalizedValue)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);
            progress = normalizedValue;

            switch (_type)
            {
                case BarType.Linear:
                    slider.normalizedValue = normalizedValue;
                    UpdateSliderColor(normalizedValue);
                    break;
                case BarType.ImageFill:
                    _graphic.fillAmount = normalizedValue;
                    if(_useColorLerp) _graphic.color = Color.Lerp(_minColor, _maxColor, normalizedValue);
                    break;
            }
            
            if (Mathf.Approximately(normalizedValue, 1)) onFill?.Invoke();
            
            InvokeUnityEvent();
        }

        public void UpdateSliderColor(float normalizedValue)
        {
            if(_useColorLerp) slider.targetGraphic.color = Color.Lerp(_minColor, _maxColor, normalizedValue);
        }

        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValue(float normalizedValue)
        {
            if(_interpolation != null) StopCoroutine(_interpolation);

            if (_interpolationTime > 0) _interpolation = StartCoroutine(Interpolation());
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

        private void OnEnable()
        {
            if (_source.HasSource)
            {
                _source.BindEvent(UpdateValue);
                UpdateValueNonInterpolate(_source.Value);
            }
        }

        private void OnDisable()
        {
            if (_source.HasSource) _source.UnbindEvent(UpdateValue);
        }
    }

}