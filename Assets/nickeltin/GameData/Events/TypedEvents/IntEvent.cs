using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(IntEvent))]
    public sealed class IntEvent : EventObject<int> { }
}