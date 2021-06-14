using System;
using System.Collections.Generic;
using nickeltin.UI.Groups.Base;
using UnityEngine;

namespace nickeltin.UI.Groups
{
    public class Menu : GroupItemBase<Menu>
    {
        [Serializable]
        public enum MenuType { Overlay, Main }
        
        [SerializeField, Tooltip("Overlay - Do not disables other menus on open. Main - Disables other menus on open")] 
        private MenuType _type;
        
        private bool _opened => gameObject.activeInHierarchy;
        
        
        protected override bool Open_Internal()
        {
            if(_opened) return false;
            
            gameObject.SetActive(true);
            _onOpen.Invoke();

            return true;
        }

        protected override bool Close_Internal()
        {
            if(!_opened) return false;
            
            _onClose.Invoke();
            gameObject.SetActive(false);

            return true;
        }
        
        
        public override void Open(IEnumerable<Menu> otherItems)
        {
            if (_type == MenuType.Main)
            {
                foreach (var menu in otherItems)
                {
                    if(!menu.Equals(this) && menu._type != MenuType.Overlay) menu.Close_Internal();
                }
            }
            Open_Internal();
            transform.SetSiblingIndex(transform.parent.childCount-1);
        }
    }
}