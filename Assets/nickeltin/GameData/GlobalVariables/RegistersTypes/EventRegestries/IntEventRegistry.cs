using nickeltin.Experimental.GlobalVariables;
using nickeltin.Experimental.GlobalVariables.Types;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace Testing.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(IntEventRegistry))]
    public class IntEventRegistry : GlobalVariablesRegistry<Event<int>> { }
}