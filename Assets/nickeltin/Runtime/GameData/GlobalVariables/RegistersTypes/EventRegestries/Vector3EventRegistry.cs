using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(Vector3EventRegistry), order = 3)]
    public class Vector3EventRegistry : GlobalVariablesRegistry<Event<Vector3>> { }
}