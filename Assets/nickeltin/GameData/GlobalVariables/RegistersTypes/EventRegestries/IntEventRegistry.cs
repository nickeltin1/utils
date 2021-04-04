using nickeltin.GameData.Types;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(IntEventRegistry))]
    public class IntEventRegistry : GlobalVariablesRegistry<Event<int>> { }
}