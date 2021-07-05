using nickeltin.Runtime.GameData.Saving;

namespace nickeltin.Runtime.GameData.VariablesRefrences
{
    public abstract class VariableBase : IValueWithoutTypeProvider
    {
        public abstract object GetValueWithoutType();

        public abstract void SetValueWithoutType(object value);
    }
}