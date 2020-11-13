using System;
using System.Text;
using UnityEngine;

namespace Items
{
    [Serializable]
    public abstract class Item : ScriptableObject, IEquatable<Item>
    {
        public ItemBaseData baseData;
        [Space]
        public WorldItem worldPrefab;
        public Pickupable worldDropPrefab;
        
        protected bool initialized = false;
        
        public virtual Sprite GetSprite() => baseData.sprite;
        
        public Pickupable SpawnDrop(int quantity)
        {
            if (worldDropPrefab != null)
            {
                var drop = Instantiate(worldDropPrefab);
                drop.item = this;
                drop.quantity = quantity;
                drop.gameObject.name = baseData.ID + "_" + quantity;
                return drop;
            }
            return null;
        }
        
        protected virtual void OnEnable()
        {
            initialized = false;
            ItemLibrary.RegisterNewItem(this);
        }

        public bool Equals(Item other)
        {
            if (other != null && other.GetType() == this.GetType())
                if (baseData.ID == other.baseData.ID) return true;
            return false;
        }

        public string GetDescription()
        {
            StringBuilder description = new StringBuilder();
            description.AppendLine(baseData.name);
            description.AppendLine();
            description.AppendLine(baseData.Type.ToString());
            string additionalDesc = AdditionalDescriptionData();
            if (additionalDesc != null)
            {
                description.AppendLine();
                description.Append(additionalDesc);
            }
            description.AppendLine();
            description.AppendLine(baseData.description);
            return description.ToString();
        }

        protected virtual string AdditionalDescriptionData() => null;

        public void Reset() => initialized = false;
    }
    
    [Serializable]
    public enum ItemType
    {
        Weapon, Consumables, Ability, Material, KeyItem
    }

    [Serializable]
    public class ItemBaseData
    {
        public string name = "New Item";
        [Range(1, 99)] public int stackSize = 1;
        [SerializeField] private ItemType idPrefix;
        [SerializeField] private string idPostfix;
        private bool dropable;
        public bool Dropable
        {
            get => dropable || (idPrefix != ItemType.Ability && idPrefix != ItemType.KeyItem);
            set => dropable = value;
        }
        public string ID { get => idPrefix + "_" + idPostfix;}
        public ItemType Type { get => idPrefix; }
        public Sprite sprite;
        [TextArea] public string description;
    }
}