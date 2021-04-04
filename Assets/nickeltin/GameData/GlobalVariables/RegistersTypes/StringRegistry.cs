using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(StringRegistry))]
    public class StringRegistry : GlobalVariablesRegistry<string>
    {
        
    }
}