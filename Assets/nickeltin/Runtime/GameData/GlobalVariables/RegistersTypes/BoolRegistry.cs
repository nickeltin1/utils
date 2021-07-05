using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(BoolRegistry), order = 3)]
    public class BoolRegistry : GlobalVariablesRegistry<bool> { }
}