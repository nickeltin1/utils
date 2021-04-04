using nickeltin.Editor.Utility;
using UnityEngine;
using Event = nickeltin.GameData.Types.Event;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(EventRegistry))]
    public class EventRegistry : GlobalVariablesRegistry<Event> { }
}