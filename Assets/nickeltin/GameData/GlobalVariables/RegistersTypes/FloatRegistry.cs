using nickeltin.Experimental.GlobalVariables;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(FloatRegistry))]
    public class FloatRegistry : GlobalVariablesRegistry<float> { }
}