using System;
using System.Collections.Generic;
using System.Reflection;
using nickeltin.Editor.Extensions;
using nickeltin.Extensions;
using nickeltin.Runtime.NestedAssets;
using nickeltin.Runtime.NestedAssets.Attributes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.Editor.NestedAssets
{
    [CustomEditor(typeof(NestedAssetRoot<>), true)]
    public class NestedAssetRootEditor : UnityEditor.Editor
    {
        protected ReorderableList _reorderableList;
        protected SerializedProperty _childs;
        
        protected Type _childsType;
        
        protected bool _allowExport = false;
        protected bool _allowImport = false;
        protected char _separatorChar = NestedAssetParentBase.DEFAULT_NAME_SEPARATOR;
        protected List<Type> _excludeChildTypes = new List<Type>();

        #region Unity Methods

        protected virtual void OnEnable()
        {
            CacheChildsProperty();
            
            _childsType = _childs.GetObjectType();
            
            _reorderableList = new ReorderableList(_childs.serializedObject, _childs,
                false, true, true, true)
            {
                drawElementCallback = DrawElementCallback,
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Childs"),
                onRemoveCallback = list => DeleteChild(list.index),
                onAddDropdownCallback = OnAddDropdownCallback,
                onMouseUpCallback = OnMouseUpCallback,
            };
            
            var settingsAttribute = GetSettings(serializedObject);
            if (settingsAttribute != null)
            {
                _allowExport = settingsAttribute.allowExport;
                _allowImport = settingsAttribute.allowImport;
                _separatorChar = settingsAttribute.nameSeparator;
                if (settingsAttribute.excludeChilds)
                {
                    _excludeChildTypes = new List<Type>(settingsAttribute.excludeChildTypes);
                }
            }
        }
        
        public override void OnInspectorGUI()
        {
            EditorExtension.DrawScriptField(this);
            
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            
            GUI.enabled = !Application.isPlaying;
            _reorderableList.DoLayoutList();
            GUI.enabled = true;
            
            DrawPropertiesExcluding(serializedObject, "m_Script");
            
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            
            if(_allowImport) HandleDropdownImport();
        }

        #endregion
        
        #region ReorderableList callbacks
        
        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty element = _childs.GetArrayElementAtIndex(index);
            rect.height -= (EditorGUIUtility.standardVerticalSpacing * 2);
            rect.y += EditorGUIUtility.standardVerticalSpacing;
            NestedAsset identifier = (NestedAsset)element.objectReferenceValue;
            SerializedProperty _name = GetChildNameProperty(identifier, out var child);
            child.Update();
            EditorGUI.PropertyField(rect, _name);
            if (child.ApplyModifiedProperties())
            {
                if (identifier.name != GetProperChildName(_name.stringValue))
                {
                    identifier.name = GetProperChildName(_name.stringValue);
                    AssetDatabase.SaveAssets();
                }
            }
        }
        
        private void OnMouseUpCallback(ReorderableList list)
        {
            if (Event.current.button == 1)
            {
                GenericMenu menu = new GenericMenu();
                OnContextMenuCallback(list, menu);
                menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            }
        }

        protected virtual void OnContextMenuCallback(ReorderableList list, GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Properties"), false, () =>
            {
                Selection.activeObject = list.GetSelectedObject(_childs);
                EditorApplication.ExecuteMenuItem("Assets/Properties...");
                Selection.activeObject = target;
            });
            
            menu.AddItem(new GUIContent("Select asset"), false, () =>
            {
                var obj = list.GetSelectedObject(_childs);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            });

            if (_allowExport)
            {
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
        }

        protected virtual void OnAddDropdownCallback(Rect buttonrect, ReorderableList list)
        {
            CreateChildFromBaseType(_childsType, buttonrect, _excludeChildTypes);
        }
        
        #endregion

        private void CacheChildsProperty() => _childs = serializedObject.FindProperty("_childs");

        private void HandleDropdownImport()
        {
            if (DragAndDrop.objectReferences.Length == 0 || DragAndDrop.objectReferences[0] == null) return;

            if (DragAndDrop.objectReferences[0].GetType().IsSubclassOf(_childsType))
            {
                EditorGUILayout.HelpBox("Drop object here to create its copy inside.", 
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
        
        private void AddChild(Object child)
        {
            AssetDatabase.AddObjectToAsset(child, target);
            CacheChildsProperty();
            _childs.arraySize++;
            _childs.GetArrayElementAtIndex(_childs.arraySize-1).objectReferenceValue = child;
            serializedObject.ApplyModifiedProperties();
        }

        private Object RemoveChild(int index)
        {
            var child = _childs.GetArrayElementAtIndex(index).objectReferenceValue;
            _childs.GetArrayElementAtIndex(index).objectReferenceValue = null;
            _childs.DeleteArrayElementAtIndex(index);
            AssetDatabase.RemoveObjectFromAsset(child);
            return child;
        }
        
        protected void CreateNewChild(Type childType, HideFlags hideFlags = HideFlags.None)
        {
            var identifier = ScriptableObject.CreateInstance(childType);
            identifier.hideFlags = hideFlags;
            SetChildName((NestedAsset) identifier, childType.Name);
            AddChild(identifier);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }

        private void DeleteChild(int index)
        {
            int actionIndex = EditorUtility.DisplayDialogComplex("Warning",
                "Are you sure you want to delete this object?",
                "Delete", "Delete all childs","Cancel");

            if (actionIndex == 0)
            {
                Object.DestroyImmediate(RemoveChild(index), true);
            }
            else if (actionIndex == 1)
            {
                for (int i = _childs.arraySize - 1; i >= 0; i--)
                {
                    Object.DestroyImmediate(RemoveChild(i));
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string GetProperChildName(string name) => 
            GetProperChildName(target.name, name, _separatorChar);

        private void SetChildName(NestedAsset child, string name)
        {
            child.name = GetProperChildName(name);
            SerializedProperty _name = GetChildNameProperty(child, out var serializedObj);
            _name.stringValue = name;
            serializedObj.ApplyModifiedProperties();
        }

        private void ImportExistentObjects(Object[] childs, bool deleteOriginals = false)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                var child = childs[i];

                if (_excludeChildTypes.Contains(child.GetType()))
                {
                    Debug.LogError($"{child.name} of type {child.GetType()} is excluded inside of {nameof(NestedAssetParentSettings)} attribute");
                    continue;
                }
                
                if (AssetDatabase.IsSubAsset(child))
                {
                    Debug.LogError(child.name + " is already nested");
                    continue;
                }
                
                var copy = Object.Instantiate(child);
                SetChildName((NestedAsset)copy, child.name);
                AddChild(copy);

                if (deleteOriginals)
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(child));
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private void ExportExistentChild(int index, bool deleteOriginals = false)
        {
            string rootPath = AssetDatabase.GetAssetPath(target);
            string[] path = rootPath.Split('/');
            rootPath = "";
            for (int i = 0; i < path.Length - 1; i++) rootPath += path[i] + '/';

            Object child = deleteOriginals ? RemoveChild(index) : _childs.GetArrayElementAtIndex(index).objectReferenceValue;
            serializedObject.ApplyModifiedProperties();
            var childName = GetChildNameProperty((NestedAsset) child, out var _);
            rootPath = AssetDatabase.GenerateUniqueAssetPath(rootPath + childName.stringValue + ".asset");
            var copy = Object.Instantiate(child);

            if (deleteOriginals)
            {
                Object.DestroyImmediate(child, true);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.CreateAsset(copy, rootPath);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Shows dropdown with all non-generic/abstract childs, and creates new child of selected type
        /// </summary>
        /// <param name="baseType">Base type of child</param>
        /// <param name="dropdownRect">Where dropdown will be shown</param>
        /// <param name="excluding">Types that should not be added</param>
        protected void CreateChildFromBaseType(Type baseType, Rect dropdownRect, List<Type> excluding = null)
        {
            bool excluded = excluding != null;

            GenericMenu typesMenu = new GenericMenu();
            
            if (!baseType.IsAbstract && !baseType.IsGenericType)
            {
                typesMenu.AddItem(new GUIContent("Create " + baseType.Name), false, 
                    () => CreateNewChild(baseType));
            }
            
            foreach (var childType in baseType.GetChildTypes())
            {
                if (excluded && excluding.Contains(childType)) continue;
                
                typesMenu.AddItem(new GUIContent("Create " + childType.Name), false, 
                    () => CreateNewChild(childType));
            }

            if (typesMenu.GetItemCount() > 0) typesMenu.DropDown(dropdownRect);
            else throw new Exception($"Non abstract and non generic child types for {baseType} not found");
        }

        #region Static Methods

        public static SerializedProperty GetChildNameProperty(NestedAsset child, out SerializedObject serializedObject)
        {
            var childObject = new SerializedObject(child);
            serializedObject = childObject; 
            return childObject.FindProperty("_name");
        }
        
        public static string GetProperChildName(string parentName, string childName, char nameSeparator) => 
            parentName + nameSeparator + childName;

        public static NestedAssetParentSettings GetSettings(SerializedObject target) => 
            GetAttribute<NestedAssetParentSettings>(target);

        protected static T GetAttribute<T>(SerializedObject target) where T : Attribute => 
            target.targetObject.GetType().GetCustomAttribute<T>();

        #endregion
    }
}