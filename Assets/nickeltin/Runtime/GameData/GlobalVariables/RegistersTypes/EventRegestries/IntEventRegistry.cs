using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(IntEventRegistry), order = 1)]
    public class IntEventRegistry : GlobalVariablesRegistry<Event<int>> { }
}