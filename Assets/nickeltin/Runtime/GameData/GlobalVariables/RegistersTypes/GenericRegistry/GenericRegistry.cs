using nickeltin.Runtime.Utility;
using nickeltin.Runtime.NestedAssets;
using nickeltin.Runtime.NestedAssets.Attributes;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(GenericRegistry), order = 0)]
    [NestedAssetParentSettings(true, true)]
    public class GenericRegistry : NestedAssetParent<GlobalVariablesRegistryBase> { }
}