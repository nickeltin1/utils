using System;
using System.Collections.Generic;
using System.Linq;
using nickeltin.Extensions;
using UnityEngine;

namespace nickeltin.UI.Groups.Base
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class GroupBase<T> : MonoBehaviour where T : GroupItemBase<T>
    {
        [SerializeField] protected T[] _items;
        
        private void OnValidate()
        {
            if (_items == null || _items.Length == 0) RefreshItems();
        }

        private void Awake() => _items.ForEach(i => i.Init(this));

        public void CloseItem(T item) => item.Close();
        
        public void OpenItem(T item) => item.Open(_items);
        
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