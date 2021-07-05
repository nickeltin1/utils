using nickeltin.Extensions.Attributes;
using nickeltin.Runtime.GameData.VariablesRefrences;
using TMPro;
using UnityEngine;

namespace nickeltin.Runtime.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class NumberLabel : ValueObserverBase
    {
        protected enum ValueType { Int, Float }

        [SerializeField] protected ValueType _type;
        [SerializeField, ShowIf("_floatTypeValidator")] protected VarObjRef<float> _floatSource;
        [SerializeField, ShowIf("_floatTypeValidator")] [Range(0, 4)] protected int _digitsAfterPoint = 0; 
        [SerializeField, HideIf("_floatTypeValidator")] protected VarObjRef<int> _intSource;
        [SerializeField] protected string _prefix;
        [SerializeField] protected string _postfix;

        private bool _floatTypeValidator => _type == ValueType.Float;
        
        private TMP_Text m_value;

        private void Awake() => m_value = GetComponent<TMP_Text>();

        public void UpdateValue(float newValue) => UpdateValue_Internal(newValue);
        public void UpdateValue(int newValue) => UpdateValue_Internal(newValue);

        private void UpdateValue_Internal(float newValue)
        {
            m_value.text = _prefix + newValue.ToString("F" + _digitsAfterPoint) + _postfix;
            InvokeUnityEvent();
        }
        
        private void OnEnable()
        {
            if (_floatTypeValidator)
            {
                _floatSource.BindEvent(UpdateValue);
                UpdateValue(_floatSource);
            }
            else
            {
                _intSource.BindEvent(UpdateValue);
                UpdateValue(_intSource);
            }
        }

        private void OnDisable()
        {
            if (_floatTypeValidator) _floatSource.UnbindEvent(UpdateValue);
            else _intSource.UnbindEvent(UpdateValue);
        }
    }
}