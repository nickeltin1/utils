using nickeltin.Runtime.Utility;
using nickeltin.Runtime.GameData.Events;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(FloatEventRegistry), order = 2)]
    public class FloatEventRegistry : GlobalVariablesRegistry<Event<float>> { }
}