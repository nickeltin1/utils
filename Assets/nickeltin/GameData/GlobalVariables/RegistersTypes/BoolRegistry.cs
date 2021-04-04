using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(BoolRegistry))]
    public class BoolRegistry : GlobalVariablesRegistry<bool> { }
}