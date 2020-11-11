using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class ItemStack
    {
        public Item Item;
        [Range(1,99)] public int MaximumSize;
        [Range(1,99)] public int CurrentSize;
        
        public ItemStack(Item item, int quantity = 1)
        {
            if( item == null) return;
            Item = item;
            MaximumSize = item.baseData.stackSize;
            CurrentSize = Mathf.Clamp(quantity, 1, item.baseData.stackSize);
        }

        public bool TryToAdd(Item item, int quantity, out int remainder)
        {
            if (Item != null && Item.Equals(item))
            {
                remainder = Add(quantity);
                return true;
            }

            remainder = quantity;
            return false;
        }

        ///<returns>Remainder item count, which doesn't fit the stack</returns>
        public int Add(int quantity)
        {
            int quantityAbs = Math.Abs(quantity);
            int newCurrentSize = CurrentSize + quantityAbs;
            int remaining = newCurrentSize > MaximumSize ? newCurrentSize - MaximumSize : 0;
            CurrentSize = Mathf.Clamp(newCurrentSize, 1, MaximumSize);
            return remaining;
        }

        public Item Get(int quantity, out int remainder)
        {
            int quantityAbs = Math.Abs(quantity);
            int newCurrentSize = CurrentSize - quantityAbs;
            remainder = newCurrentSize < 0 ? Mathf.Abs(newCurrentSize) : 0;
            CurrentSize = Mathf.Clamp(newCurrentSize, 0, MaximumSize);
            return Item;
        }
    }
}