using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(IntRegistry), order = 1)]
    public class IntRegistry : GlobalVariablesRegistry<int> { }
}