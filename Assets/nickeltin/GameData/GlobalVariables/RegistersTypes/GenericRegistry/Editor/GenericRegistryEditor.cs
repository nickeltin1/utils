using System;
using nickeltin.Extensions;
using nickeltin.Extensions.Editor;
using nickeltin.NestedAssets.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace nickeltin.GameData.GlobalVariables.RegistersTypes.Editor
{
    [CustomEditor(typeof(GenericRegistry))]
    public class GenericRegistryEditor : NestedAssetRootEditor
    {
        private Type _childsType;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _childsType = _childs.GetObjectType();
        }
        
        protected override void OnContextMenuCallback(ReorderableList list, GenericMenu menu)
        {
            base.OnContextMenuCallback(list, menu);
            
            menu.AddItem(new GUIContent("Export"), false, () =>
            {
                int actionIndex = EditorUtility.DisplayDialogComplex("Warning",
                    "Copy of this object will be created in parents directory. " +
                    "Original object will not be deleted.",
                    "Export", "Export and delete original","Cancel");

                if (actionIndex == 0)
                    ExportExistentChild(list.index, false);
                else if (actionIndex == 1)
                    ExportExistentChild(list.index, true);
            });
        }

        protected override void OnAddDropdownCallback(Rect buttonrect, ReorderableList list)
        {
            GenericMenu typesMenu = new GenericMenu();
            foreach (var childType in typeof(GlobalVariablesRegistryBase).GetChildTypes())
            {
                typesMenu.AddItem(new GUIContent("Create " + childType.Name), false, () =>
                {
                    CreateNewChild(childType);
                });
            }
            typesMenu.DropDown(buttonrect);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (DragAndDrop.objectReferences.Length == 0 || DragAndDrop.objectReferences[0] == null) return;

            if (DragAndDrop.objectReferences[0].GetType().IsSubclassOf(_childsType))
            {
                EditorGUILayout.HelpBox("Drop object here to create its copy inside of registry.", 
                    MessageType.Warning);
                
                var e = Event.current;
                
                if (e.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    e.Use();
                }   
                else if (e.type == EventType.DragPerform)
                {
                    e.Use();
                    int actionIndex = EditorUtility.DisplayDialogComplex("Warning",
                        "Copy of this object will be created inside parent object. " +
                        "Original object will not be deleted. " +
                        "Importing with deletion will delete original object and all its references will be lost.",
                        "Import", "Import and delete original","Cancel");

                    if (actionIndex == 0)
                        ImportExistentObjects(DragAndDrop.objectReferences);
                    else if (actionIndex == 1)
                        ImportExistentObjects(DragAndDrop.objectReferences, true);

                }
            }
        }
    }
}