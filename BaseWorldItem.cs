using Interfaces;
using UnityEngine;

namespace Items.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BaseWorldItem : WorldItem
    {
        public override IInitializable Initialize(IInitializer item)
        {
            Debug.LogWarning("Base world item init");
            return this;
        }
    }
}