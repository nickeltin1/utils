using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(BoolObject))]
    public class BoolObject : DataObject<bool> { }
}