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
        [SerializeField, ShowIf("m_usesSource"), Tooltip("Use source raw value or nomalized value")] 
        private bool m_useSourceRawValue;
        [SerializeField] private float m_interpolationTime = 1;
        
        private Slider m_slider;
        private Coroutine m_interpolation;
        
        
        private bool m_usesSlider => m_type == BarType.Linear;
        private bool m_usesSource => m_source != null;
        

        public float progress { get; private set; } = 0;

        public event Action onFill; 
        
        private void Awake()
        {
            m_slider = GetComponent<Slider>();
            
            if(m_source != null) UpdateValueFromSource(m_source.Value);
        }
        
        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValueNonInterpolate(float normalizedValue)
        {
            if(!gameObject.activeInHierarchy) return;

            normalizedValue = Mathf.Clamp01(normalizedValue);
            progress = normalizedValue;
            
            switch (m_type)
            {
                case BarType.Linear:
                    m_slider.normalizedValue = normalizedValue;
                    break;
                case BarType.ImageFill:
                    m_graphic.fillAmount = normalizedValue;
                    break;
            }
            
            if (Mathf.Approximately(normalizedValue, 1)) onFill?.Invoke();
        }

        /// <param name="normalizedValue">[0 - 1]</param>
        public void UpdateValue(float normalizedValue)
        {
            if(!gameObject.activeInHierarchy) return;
            
            if(m_interpolation != null) StopCoroutine(m_interpolation);
            m_interpolation = StartCoroutine(Interpolation());

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

        private void UpdateValueFromSource(float rawValue)
        {
            UpdateValue(m_useSourceRawValue ? rawValue : m_source.NormalizedValue);
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