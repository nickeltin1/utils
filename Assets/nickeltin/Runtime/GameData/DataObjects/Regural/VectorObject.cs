using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(VectorObject))]
    public class VectorObject : DataObject<Vector3> { }
}