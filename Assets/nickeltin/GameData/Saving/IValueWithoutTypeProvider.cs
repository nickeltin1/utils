namespace nickeltin.GameData.Saving
{
    public interface IValueWithoutTypeProvider
    {
        object GetValueWithoutType();
        void SetValueWithoutType(object value);
    }
}