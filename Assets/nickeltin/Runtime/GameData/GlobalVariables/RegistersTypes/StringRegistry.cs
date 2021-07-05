using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(StringRegistry), order = 4)]
    public class StringRegistry : GlobalVariablesRegistry<string>
    {
        
    }
}