using nickeltin.Editor.Utility;
using nickeltin.Experimental.GlobalVariables;
using nickeltin.Experimental.GlobalVariables.Types;
using UnityEngine;

namespace Testing.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsRegistryMenu + nameof(FloatEventRegistry))]
    public class FloatEventRegistry : GlobalVariablesRegistry<Event<float>> { }
}