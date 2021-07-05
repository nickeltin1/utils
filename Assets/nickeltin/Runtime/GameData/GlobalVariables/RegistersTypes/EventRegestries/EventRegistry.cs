using nickeltin.Runtime.Utility;
using UnityEngine;
using Event = nickeltin.Runtime.GameData.Events.Event;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(EventRegistry), order = 0)]
    public class EventRegistry : GlobalVariablesRegistry<Events.Event>
    {
        public void InvokeEvent(int id) => _entries[id].Invoke();
    }
}