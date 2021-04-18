using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(VectorObject))]
    public class VectorObject : DataObject<Vector3>
    {
        
    }
}