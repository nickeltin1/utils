using nickeltin.Runtime.NestedAssets;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    public abstract class GlobalVariablesRegistryBase : NestedAsset
    {
        public abstract bool Assigned { get; }
        
        public abstract string[] Keys { get; }
        
        public abstract int Count { get; }
    }
}