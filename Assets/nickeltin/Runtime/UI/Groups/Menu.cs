using System;
using System.Collections.Generic;
using nickeltin.Runtime.UI.Groups.Base;
using UnityEngine;

namespace nickeltin.Runtime.UI.Groups
{
    public class Menu : GroupItemBase<Menu>
    {
        [Serializable]
        public enum MenuType { Overlay, Main }
        
        [SerializeField, Tooltip("Overlay - Do not disables other menus on open. Main - Disables other menus on open")] 
        private MenuType _type;

        [SerializeField] private List<RectTransform> _sharedContent;
        
        private bool _opened => gameObject.activeInHierarchy;
        
        protected override bool Open_Internal()
        {
            if(_opened) return false;
            
            gameObject.SetActive(true);
            SharedContentSetActive(true);
            _onOpen.Invoke();

            return true;
        }

        protected override bool Close_Internal()
        {
            if(!_opened) return false;
            
            _onClose.Invoke();
            gameObject.SetActive(false);
            SharedContentSetActive(false);

            return true;
        }


        public void SharedContentSetActive(bool state)
        {
            for (var i = 0; i < _sharedContent.Count; i++)
            {
                _sharedContent[i].gameObject.SetActive(state);
            }
        }
        
        public override bool Close(IEnumerable<Menu> otherItems) => Close_Internal();

        public override bool Open(IEnumerable<Menu> otherItems)
        {
            if (_type == MenuType.Main)
            {
                foreach (var menu in otherItems)
                {
                    if(!menu.Equals(this) && menu._type != MenuType.Overlay) menu.Close_Internal();
                }
            }
            
            if (Open_Internal())
            {
                transform.SetSiblingIndex(transform.parent.childCount-1);
                return true;
            }

            return false;
        }
    }
}