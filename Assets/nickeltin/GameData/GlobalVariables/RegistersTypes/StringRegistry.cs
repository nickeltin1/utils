using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(StringRegistry), order = 4)]
    public class StringRegistry : GlobalVariablesRegistry<string>
    {
        
    }
}