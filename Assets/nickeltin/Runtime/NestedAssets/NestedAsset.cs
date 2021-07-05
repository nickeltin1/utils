using UnityEngine;

namespace nickeltin.Runtime.NestedAssets
{
    public abstract class NestedAsset : ScriptableObject
    {
        [SerializeField, Delayed, HideInInspector] private string _name;
    }
}