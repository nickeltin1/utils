using UnityEngine;

namespace nickeltin.NestedAssets
{
    public abstract class NestedAsset : ScriptableObject
    {
        [SerializeField, Delayed, HideInInspector] private string _name;
    }
}