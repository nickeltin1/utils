using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public static class ItemLibrary
    {
        private static readonly Dictionary<string, Item> allItemTypes = new Dictionary<string, Item>();

        static ItemLibrary()
        {
            // GameStaticEvents.onBeforeSceneChange += () =>
            // {
                foreach (Item item in allItemTypes.Values)
                {
                    item.Reset();
                }
            // };
        }

        public static void RegisterNewItem(Item newItem)
        {
            ItemBaseData itemBaseData = newItem.baseData;
            string errorMessage = "Item " + itemBaseData.name + " with type " + newItem.GetType();
            if(itemBaseData.ID == "") Debug.LogError(errorMessage + " dose not have an id");

            if (!allItemTypes.ContainsKey(itemBaseData.ID))
            {
                allItemTypes.Add(itemBaseData.ID, newItem);
            }
            else Debug.LogError(errorMessage + 
                                " trying to register itself with allready existing id: \"" + itemBaseData.ID +"\"");
        }
    }
}