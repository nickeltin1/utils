using nickeltin.GameData.DataObjects;
using UnityEngine;

namespace nickeltin.Other
{
    [CreateAssetMenu(menuName = "GameData/ProgressionObject")]
    public sealed class ProgressionObject : DataObject<AnimationCurve>
    {
        public override AnimationCurve Value { get => m_value; }

        public float this [float progress] => m_value.Evaluate(progress);
    }
}