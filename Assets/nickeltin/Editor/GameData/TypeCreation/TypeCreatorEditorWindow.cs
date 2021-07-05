using System;
using nickeltin.Runtime.Utility;
using nickeltin.Extensions;
using nickeltin.Runtime.GameData.DataObjects;
using nickeltin.Runtime.GameData.Events;
using nickeltin.Runtime.NestedAssets;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.GameData.TypeCreation
{
    public class TypeCreatorEditorWindow : EditorWindow
    {
        private const string _dataObjectName = "Data Object";
        private const string _eventName = "Event Object";
        private const string _containerName = "Container";
        private const string _eventListenerName = "Event Listener"; 
        
        
        private int m_toolbarValue;

        private readonly GUILayoutOption m_buttonLayout = GUILayout.Height(40f);
        private MonoScript m_type;
        
        [MenuItem(MenuPathsUtility.baseMenu + "TypeCreator")]
        private static void ShowWindow()
        {
            var window = GetWindow<TypeCreatorEditorWindow>();
            window.titleContent = new GUIContent("Type Creator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10f);
            this.m_toolbarValue = GUILayout.SelectionGrid(this.m_toolbarValue, new[]
            {
                _dataObjectName,
                _eventName,
                _containerName,
                _eventListenerName
            }, 3);
            GUILayout.Space(20f);

            m_type = EditorGUILayout.ObjectField(new GUIContent("Custom Type"), m_type, typeof(MonoScript), false) as MonoScript;
            
            
            if (m_type == null)
            {
                EditorGUILayout.HelpBox("This field can not be empty", MessageType.Warning);
                GUI.enabled = false; 
            }
            
            //DataObject
            if (m_toolbarValue == 0)
            {
                if(DrawButton(_dataObjectName))
                {
                    TypeCreator.Create(typeof(DataObject<>), "Object", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.dataObjectsMenu)}",
                        GetTargetedType());
                    
                }
            }
            //Event
            else if (m_toolbarValue == 1)
            {
                if(DrawButton(_eventName))
                {
                    TypeCreator.Create(typeof(EventObject<>), "Event", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.eventsMenu)}",
                        GetTargetedType());
                }
            }
            //Container
            else if (m_toolbarValue == 2)
            {
                if(DrawButton(_containerName))
                {
                    var targetedType = GetTargetedType(false);
                    if (TypeCreator.VerifyConstraints(targetedType, typeof(NestedAsset)))
                    {
                        TypeCreator.Create(typeof(NestedAssetRoot<>), "Container", 
                            $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.containersMenu)}", 
                            targetedType);
                    }
                };
            }
            //EventListener
            else if (m_toolbarValue == 3)
            {
                if(DrawButton(_eventListenerName))
                {
                    TypeCreator.Create(typeof(EventListener<>), "EventListener", 
                        null, GetTargetedType());
                };
            }

            GUI.enabled = true;
        }

        private Type GetTargetedType(bool checkForSerializable = true)
        {
            Type type = m_type.GetClass();
            
            if (type == null)
            {
                string typeDefenition = m_type.ToString();
                
                var parts = typeDefenition.Split(new [] {"namespace"}, 
                    StringSplitOptions.RemoveEmptyEntries)[1].Split(' ')[1].Split('\n');
                
                typeDefenition = parts[0].Trim() + "." + m_type.name;
                
                type = TypeExt.GetType(typeDefenition);
            }

            if (checkForSerializable && type != null && !type.IsSerializable)
            {
                throw new Exception($"{type} is not serializable");
            }
            
            return type;
        }

        private bool DrawButton(string postfix)
        {
            return (GUILayout.Button("Create new " + postfix + " type", m_buttonLayout));
        }
    }
}