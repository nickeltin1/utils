using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(CurveObject))]
    public sealed class CurveObject : DataObject<AnimationCurve>
    {
        public CurveObject() => _value = AnimationCurve.Linear(0,0,1,1);

        public float this [float progress] => _value.Evaluate(progress);
    }
}