using System;
using System.Collections;
using nickeltin.Editor.Attributes;
using nickeltin.Extensions;
using nickeltin.GameData.DataObjects;
using UnityEngine;
using UnityEngine.UI;

namespace nickeltin.UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBar : MonoBehaviour
    {
        [Serializable]
        public enum BarType
        {
            Linear, ImageFill
        }

        [SerializeField] private BarType m_type;
        [SerializeField, HideIf("m_usesSlider")] private Image m_graphic;
        [SerializeField] private NumberObject m_source;
        [SerializeField, ShowIf("m_usesSource"), Tooltip("Use source raw value or nomalized value")] private bool m_useSourceRawValue;
        [SerializeField] private float m_interpolationTime = 1;
        [SerializeField] private bool m_useColorLerp;
        [SerializeField, ShowIf("m_useColorLerp")] private Color m_minColor;
        [SerializeField, ShowIf("m_useColorLerp")] private Color m_maxColor;
        
        private Slider m_slider;

        private Slider slider
        {
            get
            {
                if (m_slider == null) m_slider = GetComponent<Slider>();
                return m_slider;
            } 
        }
        
        private Coroutine m_interpolation;
        
        
        private bool m_usesSlider => m_type == BarType.Linear;
        private bool m_usesSource => m_source != null;
        

        public float progress { get; private set; } = 0;

        public event Action onFill; 
        
        private void Awake()
        {
            if(m_source != null) UpdateValueFromSource(m_source.Value);
        }
        
        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValueNonInterpolate(float normalizedValue)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);
            progress = normalizedValue;

            switch (m_type)
            {
                case BarType.Linear:
                    slider.normalizedValue = normalizedValue;
                    UpdateSliderColor(normalizedValue);
                    break;
                case BarType.ImageFill:
                    m_graphic.fillAmount = normalizedValue;
                    if(m_useColorLerp) m_graphic.color = Color.Lerp(m_minColor, m_maxColor, normalizedValue);
                    break;
            }
            
            if (Mathf.Approximately(normalizedValue, 1)) onFill?.Invoke();
        }

        public void UpdateSliderColor(float normalizedValue)
        {
            if(m_useColorLerp) slider.targetGraphic.color = Color.Lerp(m_minColor, m_maxColor, normalizedValue);
        }

        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValue(float normalizedValue)
        {
            if(m_interpolation != null) StopCoroutine(m_interpolation);

            if (m_interpolationTime > 0) m_interpolation = StartCoroutine(Interpolation());
            else UpdateValueNonInterpolate(normalizedValue);

            IEnumerator Interpolation()
            {
                for (float t = 0; t < m_interpolationTime; t+=Time.deltaTime)
                {
                    UpdateValueNonInterpolate(Mathf.Lerp(progress, normalizedValue,
                        t.To01Ratio(0, m_interpolationTime)));
                    
                    yield return null;
                }
            }
        }

        private void UpdateValueFromSource(float _)
        {
            UpdateValue(m_useSourceRawValue ? m_source.Value : m_source.NormalizedValue);
        }

        private void OnEnable()
        {
            if (m_source != null) m_source.onValueChanged += UpdateValueFromSource;
        }

        private void OnDisable()
        {
            if (m_source != null) m_source.onValueChanged -= UpdateValueFromSource;
        }
    }

}