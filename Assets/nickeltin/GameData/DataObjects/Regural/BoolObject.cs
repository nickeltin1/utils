using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.gameDataMenu + nameof(BoolObject))]
    public class BoolObject : DataObject<bool>
    {
        
    }
}