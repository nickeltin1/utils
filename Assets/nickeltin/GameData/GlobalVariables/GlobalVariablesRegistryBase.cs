using nickeltin.NestedAssets;

namespace nickeltin.GameData.GlobalVariables
{
    public abstract class GlobalVariablesRegistryBase : NestedAsset
    {
        public abstract string[] Keys { get; }
        
        public abstract int Count { get; }
    }
}