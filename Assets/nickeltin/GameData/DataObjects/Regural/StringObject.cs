using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.gameDataMenu + nameof(StringObject))]
    public class StringObject : DataObject<string>
    {
        
    }
}