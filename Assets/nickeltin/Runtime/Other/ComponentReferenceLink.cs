using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.Other
{
    public class ComponentReferenceLink : MonoBehaviour
    {
        [SerializeField] private Component _target;
        [SerializeField] private ComponentReference _source;

        private void OnValidate() => ComponentExt.Cache(ref _target, gameObject);

        private void Awake() => _source.SetReference(_target);
    }
}