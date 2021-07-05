using UnityEngine;

namespace nickeltin.Extensions.Attributes
{
    public class LinkedListsSettingsAttribute : PropertyAttribute
    {
        public readonly bool draggable;
        public readonly bool displayHeader;
        public readonly bool displayAddButton;
        public readonly bool displayRemoveButton;
        public readonly bool runtimeImmutable;

        public LinkedListsSettingsAttribute(bool draggable = true, bool displayHeader = true, 
            bool displayAddButton = true, bool displayRemoveButton = true, bool runtimeImmutable = false)
        {
            this.draggable = draggable;
            this.displayHeader = displayHeader;
            this.displayAddButton = displayAddButton;
            this.displayRemoveButton = displayRemoveButton;
            this.runtimeImmutable = runtimeImmutable;
        }
    }
}