using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(IntEvent))]
    public sealed class IntEvent : GenericEventObject<int> { }
}