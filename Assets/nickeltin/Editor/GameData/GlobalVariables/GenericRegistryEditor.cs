using nickeltin.Editor.NestedAssets;
using nickeltin.Runtime.GameData.GlobalVariables;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace nickeltin.Editor.GameData.GlobalVariables
{
    [CustomEditor(typeof(GenericRegistry))]
    public class GenericRegistryEditor : NestedAssetRootEditor
    {
        protected override void OnAddDropdownCallback(Rect buttonrect, ReorderableList list) => 
            CreateChildFromBaseType(typeof(GlobalVariablesRegistryBase), buttonrect);
    }
}