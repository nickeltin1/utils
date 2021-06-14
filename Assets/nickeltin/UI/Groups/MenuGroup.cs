using nickeltin.Editor.Attributes;
using nickeltin.UI.Groups.Base;
using UnityEngine;

namespace nickeltin.UI.Groups
{
    public class MenuGroup : GroupBase<Menu>
    {
        [SerializeField] private bool _closeAllAtStart = true;
        [SerializeField] private bool _openMenuAtStart;
        [SerializeField, ShowIf("_openMenuAtStart")] private Menu _startMenu;

        private void Start()
        {
            if(_closeAllAtStart) CloseAllItems();
            if(_openMenuAtStart) _startMenu.Open();
        }

        [ContextMenu("Refresh menus list")]
        protected override void RefreshItems() => base.RefreshItems();
    }
}