using System.Collections.Generic;
using nickeltin.UI.Groups.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace nickeltin.UI.Groups
{
    [RequireComponent(typeof(RectTransform))]
    public class Tab : GroupItemBase<Tab>, IPointerClickHandler
    {
        [SerializeField] private Menu _menu;
        private bool _opened = true;

        public void OnPointerClick(PointerEventData eventData) => Open();
        
        protected override bool Open_Internal()
        {
            if(_opened) return false;
            _opened = true;

            _onOpen.Invoke();
            if(_menu != null) _menu.Open();

            return true;
        }

        protected override bool Close_Internal()
        {
            if(!_opened) return false;
            _opened = false;
            
            _onClose.Invoke();
            if(_menu != null) _menu.Close();

            return true;
        }

        internal static void CloseTab(Tab target) => target.Close_Internal();
        public override void Open(IEnumerable<Tab> otherItems)
        {
            foreach (var tab in otherItems) tab.Close_Internal();
            Open_Internal();
        }
    }
}