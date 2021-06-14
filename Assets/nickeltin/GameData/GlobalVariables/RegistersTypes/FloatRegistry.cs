using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(FloatRegistry), order = 2)]
    public class FloatRegistry : GlobalVariablesRegistry<float> { }
}