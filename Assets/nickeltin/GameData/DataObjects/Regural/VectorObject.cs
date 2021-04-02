using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.gameDataMenu + nameof(VectorObject))]
    public class VectorObject : DataObject<Vector3>
    {
        
    }
}