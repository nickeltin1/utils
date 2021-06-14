using System;
using UnityEngine;

namespace nickeltin.NestedAssets
{
    public abstract class NestedAssetRoot<T> : NestedAssetParent<T> where T : NestedAsset { }
    public abstract class NestedAssetParent<T> : NestedAssetParentBase where T : NestedAsset
    {
        [SerializeField, HideInInspector] private T [] _childs;

        protected T[] Childs => _childs;
    }
    public abstract class NestedAssetParentBase : ScriptableObject { }
}