using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Experimental.GlobalVariables
{
    [CreateAssetMenu(menuName = MenuPathsUtility.registryMenu + nameof(StringRegistry))]
    public class StringRegistry : GlobalVariablesRegistry<string>
    {
        
    }
}