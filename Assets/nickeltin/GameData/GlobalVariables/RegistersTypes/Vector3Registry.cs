using nickeltin.Editor.Utility;
using nickeltin.Experimental.GlobalVariables;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(Vector3Registry))]
    public class Vector3Registry : GlobalVariablesRegistry<Vector3> { }
}