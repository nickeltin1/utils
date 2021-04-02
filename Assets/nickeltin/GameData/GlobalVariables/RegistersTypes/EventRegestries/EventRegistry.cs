using nickeltin.Experimental.GlobalVariables;
using nickeltin.Editor.Utility;
using UnityEngine;
using Event = nickeltin.Experimental.GlobalVariables.Types.Event;

namespace Testing.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(EventRegistry))]
    public class EventRegistry : GlobalVariablesRegistry<Event> { }
}