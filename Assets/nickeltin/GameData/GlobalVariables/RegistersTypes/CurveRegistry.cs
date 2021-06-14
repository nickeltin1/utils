using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(CurveRegistry), order = 6)]
    public class CurveRegistry : GlobalVariablesRegistry<AnimationCurve> { }
}