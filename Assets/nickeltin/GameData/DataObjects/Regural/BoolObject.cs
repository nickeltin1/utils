using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(BoolObject))]
    public class BoolObject : DataObject<bool>
    {
        
    }
}