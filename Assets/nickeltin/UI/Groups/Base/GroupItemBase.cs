using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace nickeltin.UI.Groups.Base
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class GroupItemBase<T> : MonoBehaviour where T : GroupItemBase<T>
    {
        [SerializeField] protected UnityEvent _onOpen;
        [SerializeField] protected UnityEvent _onClose;
        
        protected GroupBase<T> _group;
        
        public virtual GroupItemBase<T> Init(GroupBase<T> group)
        {
            _group = group;
            return this;
        }

        public void Open()
        {
            if (_group != null) _group.OpenItem(this as T);
            else Open_Internal();
        }

        public void Close() => Close_Internal();


        public abstract void Open(IEnumerable<T> otherItems);

        protected abstract bool Open_Internal();
        
        protected abstract bool Close_Internal();
    }
}