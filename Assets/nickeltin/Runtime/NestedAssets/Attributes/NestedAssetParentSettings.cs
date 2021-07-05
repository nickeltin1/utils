using System;

namespace nickeltin.Runtime.NestedAssets.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NestedAssetParentSettings : Attribute
    {
        public readonly bool allowExport;
        public readonly bool allowImport;
        public readonly Type[] excludeChildTypes;
        public readonly bool excludeChilds;

        public char nameSeparator { get; set; } = NestedAssetParentBase.DEFAULT_NAME_SEPARATOR;

        public NestedAssetParentSettings(bool allowExport, bool allowImport)
        {
            this.allowExport = allowExport;
            this.allowImport = allowImport;
        }
        
        public NestedAssetParentSettings(bool allowExport, bool allowImport, params Type[] excludeChildTypes) : this(allowExport, allowImport)
        {
            this.excludeChilds = true;
            this.excludeChildTypes = excludeChildTypes;
        }
    }
}