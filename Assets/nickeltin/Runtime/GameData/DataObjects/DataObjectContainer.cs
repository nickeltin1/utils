using nickeltin.Runtime.Utility;
using nickeltin.Runtime.NestedAssets;
using nickeltin.Runtime.NestedAssets.Attributes;
using UnityEngine;

namespace nickeltin.Runtime.GameData.DataObjects
{
    [CreateAssetMenu(menuName = MenuPathsUtility.containersMenu + nameof(DataObjectContainer))]
    [NestedAssetParentSettings(true, true)]
    public class DataObjectContainer : NestedAssetRoot<DataObjectBase> { }
}