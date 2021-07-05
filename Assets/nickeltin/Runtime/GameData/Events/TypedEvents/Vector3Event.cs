using nickeltin.Runtime.Utility;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(Vector3Event))]
    public sealed class Vector3Event : EventObject<Vector3> { }
}