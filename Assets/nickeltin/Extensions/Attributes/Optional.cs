using System;
using UnityEngine;

namespace nickeltin.Extensions.Types
{
    [Serializable]
    public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool Enabled => _enabled;
        public T Value => _value;

        public static implicit operator T(Optional<T> source) => source._value;
        public static implicit operator bool(Optional<T> source) => source._enabled;
        public static implicit operator Optional<T>(T value) => new Optional<T>{_value = value, _enabled = true};
        
        
#if UNITY_EDITOR
        public static string enabled_prop_name => nameof(_enabled);
        public static string value_prop_name => nameof(_value);
#endif
    }
}