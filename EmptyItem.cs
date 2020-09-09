using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "EmptyItem", menuName = "Items/EmptyItem", order = 51)]
    public class EmptyItem : Item
    {
        public override Sprite GetSprite()
        {
            if(base.GetSprite() == null) Debug.LogWarning("Empty item doesn't have sprite!");
            return base.GetSprite();
        }
    }
}