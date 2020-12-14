namespace nickeltin.GameData.DataObjects
{
    public abstract class DataObjectReferenceBase
    {
        public abstract object GetValueWithoutType();
        public abstract void SetValueWithoutType(object value);
    }
}