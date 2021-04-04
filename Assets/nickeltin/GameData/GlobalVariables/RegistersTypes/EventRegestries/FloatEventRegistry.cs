using nickeltin.Editor.Utility;
using nickeltin.GameData.Types;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(FloatEventRegistry))]
    public class FloatEventRegistry : GlobalVariablesRegistry<Event<float>> { }
}