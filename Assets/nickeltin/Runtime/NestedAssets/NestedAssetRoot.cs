﻿using System.Collections.Generic;
using UnityEngine;

namespace nickeltin.Runtime.NestedAssets
{
    public abstract class NestedAssetRoot<T> : NestedAssetParent<T> where T : NestedAsset { }
    public abstract class NestedAssetParent<T> : NestedAssetParentBase where T : NestedAsset
    {
        [SerializeField, HideInInspector] private List<T> _childs;

        public IList<T> Childs => _childs;

#if UNITY_EDITOR
        public static string childs_prop_name => nameof(_childs);
#endif
    }

    public abstract class NestedAssetParentBase : ScriptableObject
    {
        public const char DEFAULT_NAME_SEPARATOR = '.';
    }
}