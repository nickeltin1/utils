namespace nickeltin.GameData.DataObjects
{
    public abstract class DataObjectReference<ObjectType, ValueType> where ObjectType: DataObject<ValueType>
    {
        public bool UseConstant = true;
        public ValueType ConstantValue;
        public ObjectType DataObject;
        
        public ValueType Value
        {
            get { return UseConstant ? ConstantValue : DataObject.Value; }
        }

        public static implicit operator ValueType(DataObjectReference<ObjectType, ValueType> reference)
        {
            return reference.Value;
        }
    }
}