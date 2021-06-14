using nickeltin.Editor.Utility;
using nickeltin.GameData.Events.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(IntEventRegistry), order = 1)]
    public class IntEventRegistry : GlobalVariablesRegistry<Event<int>> { }
}