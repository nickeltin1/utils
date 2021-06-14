using nickeltin.UI.Groups.Base;
using UnityEngine;

namespace nickeltin.UI.Groups
{
    [RequireComponent(typeof(RectTransform))]
    public class TabGroup : GroupBase<Tab>
    {
        private void Start() => CloseAllItems();
        

        [ContextMenu("Refresh tabs list")]
        protected override void RefreshItems() => base.RefreshItems();
    }
}