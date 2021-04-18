using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(StringObject))]
    public class StringObject : DataObject<string>
    {
        
    }
}