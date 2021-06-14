using nickeltin.Editor.Utility;
using nickeltin.GameData.Events.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(FloatEventRegistry), order = 2)]
    public class FloatEventRegistry : GlobalVariablesRegistry<Event<float>> { }
}