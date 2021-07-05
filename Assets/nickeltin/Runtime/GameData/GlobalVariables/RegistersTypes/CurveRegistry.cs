using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(CurveRegistry), order = 6)]
    public class CurveRegistry : GlobalVariablesRegistry<AnimationCurve> { }
}