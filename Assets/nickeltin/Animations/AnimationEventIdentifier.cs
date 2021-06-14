using nickeltin.NestedAssets;
using UnityEngine;

namespace nickeltin.Animations
{
    public sealed class AnimationEventIdentifier : NestedAsset
    {
        [SerializeField, Multiline] private string _description;
    }
}