using System.Collections.Generic;
using System.Linq;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.Runtime.UI.Groups.Base
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class GroupBase<T> : MonoBehaviour where T : GroupItemBase<T>
    {
        [SerializeField] protected T[] _items;

        protected readonly List<T> _openedItems = new List<T>();
        
        private void OnValidate()
        {
            if (_items == null || _items.Length == 0) RefreshItems();
        }

        private void Awake() => _items.ForEach(i => i.Init(this));

        public void CloseItem(T item)
        {
            if (item.Close(_items))
            {
                if (_openedItems.Contains(item)) _openedItems.Remove(item);
            }
        }

        public void OpenItem(T item)
        {
            if (item.Open(_items))
            {
                if(!_openedItems.Contains(item)) _openedItems.Add(item);   
            }
        }

        public void CloseAllItems() => _items.ForEach(CloseItem);


        protected virtual void RefreshItems() => _items = CacheItems().ToArray();
        
        private IEnumerable<T>CacheItems()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out T tab))
                    yield return tab;
            }
        }
    }
}