using nickeltin.Editor.Utility;
using nickeltin.Experimental.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(IntRegistry))]
    public class IntRegistry : GlobalVariablesRegistry<int> { }
}