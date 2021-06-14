using System;
using nickeltin.EditorExtensions.Editor;
using nickeltin.Extensions.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace nickeltin.NestedAssets.Editor
{
    [CustomEditor(typeof(NestedAssetRoot<>), true)]
    public class NestedAssetRootEditor : UnityEditor.Editor
    {
        protected ReorderableList _reorderableList;
        protected SerializedProperty _childs;

        private void CacheChildsProperty()
        {
            _childs = serializedObject.FindProperty("_childs");
        }
        
        protected virtual void OnEnable()
        {
            CacheChildsProperty();
            
            _reorderableList = new ReorderableList(_childs.serializedObject, _childs,
                false, true, true, true)
            {
                drawElementCallback = DrawElementCallback,
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Childs"),
                onRemoveCallback = list => DeleteChild(list.index),
                onAddDropdownCallback = OnAddDropdownCallback,
                onMouseUpCallback = OnMouseUpCallback,
            };
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
            menu.AddItem(new GUIContent("Select asset"), false, () =>
            {
                var obj = _childs.GetArrayElementAtIndex(list.index).objectReferenceValue;
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            });
        }

        protected virtual void OnAddDropdownCallback(Rect buttonrect, ReorderableList list)
        {
            CreateNewChild(_childs.GetObjectType());
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
        }

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
        
        protected void CreateNewChild(Type childType)
        {
            var identifier = ScriptableObject.CreateInstance(childType);
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

        protected string GetProperChildName(string name) => GetProperChildName(target.name, name);
        
        public static SerializedProperty GetChildNameProperty(NestedAsset child, out SerializedObject serializedObject)
        {
            var childObject = new SerializedObject(child);
            serializedObject = childObject; 
            return childObject.FindProperty("_name");
        }

        public static string GetProperChildName(string parentName, string childName) => parentName + "." + childName;

        private void SetChildName(NestedAsset child, string name)
        {
            child.name = GetProperChildName(name);
            SerializedProperty _name = GetChildNameProperty(child, out var serializedObj);
            _name.stringValue = name;
            serializedObj.ApplyModifiedProperties();
        }
        
        protected void ImportExistentObjects(Object[] childs, bool deleteOriginals = false)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                var child = childs[i];

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
        
        protected void ExportExistentChild(int index, bool deleteOriginals = false)
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
    }
}