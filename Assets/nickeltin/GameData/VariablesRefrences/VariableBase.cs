using nickeltin.GameData.Saving;

namespace nickeltin.GameData.VariablesRefrences
{
    public abstract class VariableBase : IValueWithoutTypeProvider
    {
        public abstract object GetValueWithoutType();

        public abstract void SetValueWithoutType(object value);
    }
}