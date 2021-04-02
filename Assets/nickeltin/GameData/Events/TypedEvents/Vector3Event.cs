using nickeltin.Editor.Utility;
using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.eventsMenu + nameof(Vector3Event))]
    public sealed class Vector3Event : GenericEventObject<Vector3> { }
}