using nickeltin.GameData.DataObjects;
using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Other
{
    [CreateAssetMenu(menuName = MenuPathsUtility.gameDataMenu + nameof(ProgressionObject))]
    public sealed class ProgressionObject : DataObject<AnimationCurve>
    {
        public override AnimationCurve Value { get => m_value; }

        public float this [float progress] => m_value.Evaluate(progress);
    }
}