using nickeltin.Editor.Utility;
using UnityEngine;
using Event = nickeltin.GameData.Events.Types.Event;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(EventRegistry), order = 0)]
    public class EventRegistry : GlobalVariablesRegistry<Event>
    {
        public void InvokeEvent(int id) => _entries[id].Invoke();
    }
}