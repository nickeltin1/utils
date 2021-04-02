using UnityEngine;

namespace nickeltin.Editor.Attributes
{
    public class LinkedListsSettingsAttribute : PropertyAttribute
    {
        public readonly bool draggable;
        public readonly bool displayHeader;
        public readonly bool displayAddButton;
        public readonly bool displayRemoveButton;

        public LinkedListsSettingsAttribute(bool draggable = true, bool displayHeader = true, 
            bool displayAddButton = true, bool displayRemoveButton = true)
        {
            this.draggable = draggable;
            this.displayHeader = displayHeader;
            this.displayAddButton = displayAddButton;
            this.displayRemoveButton = displayRemoveButton;
        }
    }
}