using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(IntRegistry), order = 1)]
    public class IntRegistry : GlobalVariablesRegistry<int> { }
}