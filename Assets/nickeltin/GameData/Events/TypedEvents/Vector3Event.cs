using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(Vector3Event))]
    public sealed class Vector3Event : EventObject<Vector3> { }
}