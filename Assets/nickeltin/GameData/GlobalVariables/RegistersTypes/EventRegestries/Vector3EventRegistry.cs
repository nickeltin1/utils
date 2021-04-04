using nickeltin.Editor.Utility;
using nickeltin.GameData.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(Vector3EventRegistry))]
    public class Vector3EventRegistry : GlobalVariablesRegistry<Event<Vector3>> { }
}