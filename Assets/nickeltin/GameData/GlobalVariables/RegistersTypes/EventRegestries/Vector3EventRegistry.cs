using nickeltin.Editor.Utility;
using nickeltin.Experimental.GlobalVariables;
using nickeltin.Experimental.GlobalVariables.Types;
using UnityEngine;

namespace Testing.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(Vector3EventRegistry))]
    public class Vector3EventRegistry : GlobalVariablesRegistry<Event<Vector3>> { }
}