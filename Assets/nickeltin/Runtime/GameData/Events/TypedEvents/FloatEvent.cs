using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(FloatEvent))]
    public sealed class FloatEvent : EventObject<float> { }
}