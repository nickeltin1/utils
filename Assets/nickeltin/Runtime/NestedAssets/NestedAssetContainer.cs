﻿namespace nickeltin.Runtime.NestedAssets
{
    /// <summary>
    /// Container for nested assets with drowpdown for creating childs deriving from base type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NestedAssetContainer<T> : NestedAssetParent<T> where T : NestedAsset { }
}