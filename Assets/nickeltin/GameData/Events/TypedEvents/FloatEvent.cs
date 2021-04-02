using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(FloatEvent))]
    public sealed class FloatEvent : GenericEventObject<float> { }
}