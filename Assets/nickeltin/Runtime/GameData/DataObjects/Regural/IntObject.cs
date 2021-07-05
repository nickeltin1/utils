using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(IntObject))]
    public class IntObject : DataObject<int> { }
}