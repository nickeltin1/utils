using UnityEngine;

namespace nickeltin.Runtime.NestedAssets
{
    public abstract class NestedAsset : ScriptableObject
    {
        [SerializeField, Delayed, HideInInspector] private string _name;

#if UNITY_EDITOR
        public static string name_prop_name => nameof(_name);
#endif
    }
}