using System;
using UnityEngine;

namespace nickeltin.Extensions.Types
{
    [Serializable]
    public class Optional<T>
    {
        [SerializeField] private bool _enabled = true;
        [SerializeField] private T _value;

        public bool Enabled => _enabled;
        public T Value => _value;

        public static implicit operator T(Optional<T> source) => source._value;

        public static implicit operator Optional<T>(T value) => new Optional<T>() {_value = value};
    }
}