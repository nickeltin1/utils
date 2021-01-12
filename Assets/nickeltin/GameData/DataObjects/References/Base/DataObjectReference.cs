using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    public abstract class DataObjectReference<ObjectType, ValueType> : DataObjectReferenceBase where ObjectType : DataObject<ValueType>
    {
        [SerializeField] protected bool m_useConstant = true;
        [SerializeField] protected ValueType m_constantValue;
        [SerializeField] protected ObjectType m_dataObject;

        public ValueType Value
        {
            get { return m_useConstant ? m_constantValue : m_dataObject.Value; }
        }
        
        public override object GetValueWithoutType() => Value;
        public override void SetValueWithoutType(object value)
        {
            m_constantValue = (ValueType) value;
            if (m_dataObject != null) m_dataObject.Value = m_constantValue;
        }

        public static implicit operator ValueType(DataObjectReference<ObjectType, ValueType> reference)
        {
            return reference.Value;
        }
    }
}