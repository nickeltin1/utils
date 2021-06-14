using System;
using nickeltin.GameData.Events.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables
{
    [Serializable]
    public sealed class GlobalVar<T> : IEventBinder<T>
    {
        [SerializeField] private GlobalVariablesRegistry<T> _registry;
        [SerializeField] private int _entryIndex;
        
        private event Action<T> _onValueChanged;
        
        public bool HasRegistry => _registry != null;

        
        public T Value
        {
            get => _registry[_entryIndex]; 
            set => _registry[_entryIndex] = value;
        }

        public static implicit operator T(GlobalVar<T> obj) => obj.Value;

        public override string ToString() => Value.ToString();
        
        private void OnRegistryUpdate(int id, T value)
        {
            if(id == _entryIndex) _onValueChanged?.Invoke(Value);
        }

        public void BindEvent(Action<T> onValueChanged)
        {
            if (HasRegistry)
            {
                if (_onValueChanged == null) _registry.onEntryChanged += OnRegistryUpdate;
                _onValueChanged += onValueChanged;
            }
        }

        public void UnbindEvent(Action<T> onValueChanged)
        {
            if (HasRegistry)
            {
                _registry.onEntryChanged -= OnRegistryUpdate;
                _onValueChanged -= onValueChanged;
            }
        }

        public void UnbindAllEvents() => _onValueChanged = null;
    }
}