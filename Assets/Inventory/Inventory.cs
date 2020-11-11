using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Characters;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Character Components/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private List<ItemStack> items = new List<ItemStack>();

        public event Action onInventoryChanged;
        public event Action<ItemStack> onItemRemove;
        public event Action<ItemStack> onItemAdded;
        public event Action<Item, int> onItemDropped;

        [HideInInspector] public CharacterBase owner;

        public void ItemDropInvoke(Item item, int quantity) => onItemDropped?.Invoke(item, quantity);

        public void AddItem(ItemStack stack) => AddItem(stack.Item, stack.CurrentSize);

        public void AddItem(Item item, int quantity)
        {
            void AddNewStack()
            {
                var newStack = new ItemStack(item);
                items.Add(newStack);
                AddItem(item, quantity - 1);
                onItemAdded?.Invoke(newStack);
            }
            
            if (items.Count > 0)
            {
                for(int i = items.Count - 1; i >= 0; i--)
                {
                    if (items[i].TryToAdd(item, quantity, out var remainder))
                    {
                        onItemAdded?.Invoke(items[i]);
                        quantity -= quantity - remainder;
                        if (quantity == 0) break;
                    }
                }
                
                if (quantity > 0) AddNewStack();
            }
            else AddNewStack();
            
            onInventoryChanged?.Invoke();
        }
        
        public void RemoveItem(ItemStack item, [DefaultParameterValue(0)][Optional] int quantity, 
            [DefaultParameterValue(false)][Optional]bool dropped)
        {
            if (items == null || items.Count == 0) return;
            if (quantity == 0) quantity = item.CurrentSize;
            item.Get(quantity, out var rem);
            if (item.CurrentSize == 0) items.Remove(item);
            onItemRemove?.Invoke(item);
            if (dropped) onItemDropped?.Invoke(item.Item, quantity - rem);
            onInventoryChanged?.Invoke();
        }
        
        public List<ItemStack> GetItems() => items;
    }
}