using nickeltin.GameData.DataObjects;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Other
{
    [CreateAssetMenu(menuName = MenuPathsUtility.dataObjectsMenu + nameof(CurveObject))]
    public sealed class CurveObject : DataObject<AnimationCurve>
    {
        public CurveObject() => m_value = AnimationCurve.Linear(0,0,1,1);

        public float this [float progress] => m_value.Evaluate(progress);
    }
}