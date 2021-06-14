using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(BoolRegistry), order = 3)]
    public class BoolRegistry : GlobalVariablesRegistry<bool> { }
}