using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(FloatEvent))]
    public sealed class FloatEvent : EventObject<float> { }
}