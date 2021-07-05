using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(IntEvent))]
    public sealed class IntEvent : EventObject<int> { }
}