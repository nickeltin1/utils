using Game.Scripts.nickeltin.GameData.Saving;

namespace Game.Scripts.nickeltin.GameData.VariablesRefrences
{
    public abstract class VariableBase : IValueWithoutTypeProvider
    {
        public abstract object GetValueWithoutType();

        public abstract void SetValueWithoutType(object value);
    }
}