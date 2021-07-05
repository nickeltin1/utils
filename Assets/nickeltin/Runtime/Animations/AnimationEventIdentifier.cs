using nickeltin.Runtime.NestedAssets;
using UnityEngine;

namespace nickeltin.Runtime.Animations
{
    public sealed class AnimationEventIdentifier : NestedAsset
    {
        [SerializeField, Multiline] private string _description;
    }
}