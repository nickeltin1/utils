using nickeltin.Editor.Utility;
using nickeltin.NestedAssets;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(GenericRegistry), order = 0)]
    public class GenericRegistry : NestedAssetParent<GlobalVariablesRegistryBase> { }
}