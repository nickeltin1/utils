using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class InventorySorter
    {
        private Inventory target;
        private readonly Dictionary<ItemType, List<ItemStack>> allGroups;
        public event Action<ItemType> onGroupChanged;
        
        public InventorySorter(Inventory target)
        {
            this.target = target;
            allGroups = new Dictionary<ItemType, List<ItemStack>>();
            foreach (var itemType in Enum.GetValues(typeof(ItemType))) 
                allGroups.Add((ItemType) itemType, new List<ItemStack>());
            foreach (var item in target.GetItems()) allGroups[item.Item.baseData.Type].Add(item);

            target.onItemAdded += TargetOnItemAdded;
            target.onItemRemove += TargetOnItemRemove;
        }

        ~InventorySorter()
        {
            target.onItemAdded -= TargetOnItemAdded;
            target.onItemRemove -= TargetOnItemRemove;
        }

        private void TargetOnItemAdded(ItemStack itemStack)
        {
            var group = allGroups[itemStack.Item.baseData.Type];
            if (!group.Contains(itemStack)) group.Add(itemStack);
            onGroupChanged?.Invoke(itemStack.Item.baseData.Type);
        }

        private void TargetOnItemRemove(ItemStack itemStack)
        {
            if(itemStack.CurrentSize == 0) allGroups[itemStack.Item.baseData.Type].Remove(itemStack);
            onGroupChanged?.Invoke(itemStack.Item.baseData.Type);
        }


        public List<ItemStack> GetItemGroup(ItemType type) => allGroups[type];
    }
}