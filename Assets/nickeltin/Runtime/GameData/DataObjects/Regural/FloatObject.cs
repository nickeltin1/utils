using System;
using nickeltin.Extensions;
using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(FloatObject))]
    public class FloatObject : DataObject<float>
    {
        [SerializeField] private float _minValue = Single.MinValue;
        [SerializeField] private float _maxValue = Single.MaxValue;
        
        /// <summary>
        /// Sets values to maximum possible
        /// </summary>
        public void SetMinMax() => SetMinMax(Single.MinValue, Single.MaxValue);
        public void SetMinMax(float min, float max)
        {
            this._minValue = min;
            this._maxValue = max;
        }

        protected override bool TrySetValue(float value)
        {
            value.Clamp(_minValue, _maxValue);
            return base.TrySetValue(value);
        }


        /// <summary>
        /// Value from 0 to 1, depends on min/max values
        /// </summary>
        public float NormalizedValue => _value.To01Ratio(_minValue, _maxValue);
        
        
#if UNITY_EDITOR
        private void OnValidate() => _value.Clamp(_minValue, _maxValue);

        [ContextMenu("Reset MinMax")]
        private void ResetMinMax_Context() => SetMinMax();
#endif
    }
}