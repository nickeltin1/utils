using System;
using nickeltin.Extensions;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(NumberObject))]
    public class NumberObject : DataObject<float>
    {
        public enum NumberType { Int, Float }
        
        [SerializeField] NumberType m_type = NumberType.Float;
        
        private float m_minValue = float.MinValue;
        private float m_maxValue = float.MaxValue;
        
        /// <summary>
        /// Sets values to maximum possible
        /// </summary>
        public void SetMinMax() => SetMinMax(float.MinValue, float.MaxValue);
        public void SetMinMax(float min, float max)
        {
            this.m_minValue = min;
            this.m_maxValue = max;
        }
        
        public void SetNumberType(NumberType type) => m_type = type;
        
        public override float Value
        {
            get
            {
                if (m_type.Equals(NumberType.Int)) return (int)_value;
                return _value;
            }
            set
            {
                float v = Mathf.Clamp(value, m_minValue, m_maxValue);
                if (m_type.Equals(NumberType.Int)) v = (int)v;

                TrySetValue(v);
            }
        }

        private void OnValidate()
        {
            if (m_type.Equals(NumberType.Int)) _value = (int) _value;
        }

        /// <summary>
        /// Value from 0 to 1, depends on min/max values
        /// </summary>
        public float NormalizedValue => _value.To01Ratio(m_minValue, m_maxValue);
    }
}