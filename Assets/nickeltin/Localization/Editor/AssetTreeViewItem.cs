using UnityEditor.IMGUI.Controls;

namespace nickeltin.Localization.Editor
{
    public class AssetTreeViewItem : TreeViewItem
    {
        private bool m_IsDirty;

        /// <summary>
        /// Gets or sets item as dirty. Added "*" postfix to the display name if is dirty.
        /// </summary>
        public bool IsDirty
        {
            get { return m_IsDirty; }
            set
            {
                m_IsDirty = value;
                if (value)
                {
                    displayName = LocalizedAsset.name + "*";
                }
                else
                {
                    displayName = LocalizedAsset.name;
                }
            }
        }

        public LocalizedAssetBase LocalizedAsset { get; private set; }

        public AssetTreeViewItem(int depth, LocalizedAssetBase data) : base(data.GetInstanceID(), depth, data.name)
        {
            LocalizedAsset = data;
        }
    }
}
