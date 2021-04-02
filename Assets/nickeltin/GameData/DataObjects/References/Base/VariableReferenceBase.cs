namespace nickeltin.GameData.DataObjects
{
    public abstract class VariableReferenceBase
    {
        public abstract object GetValueWithoutType();
        public abstract void SetValueWithoutType(object value);
    }
}