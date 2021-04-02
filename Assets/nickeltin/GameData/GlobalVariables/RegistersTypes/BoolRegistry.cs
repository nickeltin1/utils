using nickeltin.Editor.Utility;
using nickeltin.Experimental.GlobalVariables;
using UnityEngine;

namespace Testing.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(BoolRegistry))]
    public class BoolRegistry : GlobalVariablesRegistry<bool> { }
}