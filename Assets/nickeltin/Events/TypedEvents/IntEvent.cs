using UnityEngine;

namespace nickeltin.Events
{
    [CreateAssetMenu(menuName = "Events/IntEvent")]
    public sealed class IntEvent : GenericEventObject<int> { }
}