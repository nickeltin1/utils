using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    [CreateAssetMenu(menuName = MenuPathsUtility.otherMenu + nameof(ComponentReference))]
    public class ComponentReference : ScriptableObject
    {
        [SerializeField] private Component _reference;

        public Component Reference => _reference;

        public void SetReference(Component component) => _reference = component;

        public T GetReference<T>() where T : Component => _reference as T;

        public bool TryGetReference<T>(out T obj) where T : Component
        {
            if (_reference.GetType().IsAssignableFrom(typeof(T)))
            {
                obj = _reference as T;
                return true;
            }

            obj = null;
            return false;
        }
    }
}