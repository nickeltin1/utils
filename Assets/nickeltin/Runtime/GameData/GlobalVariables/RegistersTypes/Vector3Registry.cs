using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(Vector3Registry), order = 5)]
    public class Vector3Registry : GlobalVariablesRegistry<Vector3> { }
}