namespace nickeltin.GameData.DataObjects
{
    public abstract class DataObjectReference<ObjectType, ValueType> : DataObjectReferenceBase where ObjectType : DataObject<ValueType>
    {
        public bool UseConstant = true;
        public ValueType ConstantValue;
        public ObjectType DataObject;
        
        public ValueType Value
        {
            get { return UseConstant ? ConstantValue : DataObject.Value; }
        }
        
        public override object GetValueWithoutType() => Value;
        public override void SetValueWithoutType(object value)
        {
            ConstantValue = (ValueType) value;
            if(DataObject != null) DataObject.Value = (ValueType) value;
        }

        public static implicit operator ValueType(DataObjectReference<ObjectType, ValueType> reference)
        {
            return reference.Value;
        }
    }
}