using nickeltin.Editor.Utility;
using nickeltin.GameData.Events.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(Vector3EventRegistry), order = 3)]
    public class Vector3EventRegistry : GlobalVariablesRegistry<Event<Vector3>> { }
}