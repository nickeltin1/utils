using nickeltin.Runtime.Utility;
using nickeltin.Runtime.NestedAssets;
using nickeltin.Runtime.NestedAssets.Attributes;
using UnityEngine;

namespace nickeltin.Runtime.GameData.Events
{
    [CreateAssetMenu(menuName = MenuPathsUtility.containersMenu + nameof(EventObjectContainer))]
    [NestedAssetParentSettings(true, true)]
    public class EventObjectContainer : NestedAssetRoot<EventObjectBase> { }
}