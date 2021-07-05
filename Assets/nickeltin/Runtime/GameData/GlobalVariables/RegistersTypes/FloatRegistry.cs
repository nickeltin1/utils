using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(FloatRegistry), order = 2)]
    public class FloatRegistry : GlobalVariablesRegistry<float> { }
}