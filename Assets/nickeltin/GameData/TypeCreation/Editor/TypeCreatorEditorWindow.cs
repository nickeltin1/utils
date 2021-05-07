using System;
using nickeltin.Editor.Utility;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.Events;
using nickeltin.GameData.GlobalVariables;
using nickeltin.GameData.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace nickeltin.GameData.Editor.TypeCreation
{
    public class TypeCreatorEditorWindow : EditorWindow
    {
        private const string m_dataObjectName = "DataObject";
        private const string m_eventName = "EventObject";
        private const string m_registryName = "Registry";
        private const string m_eventRegistryName= "EventReigstry";
        
        
        private int m_toolbarValue;
        private readonly GUILayoutOption m_buttonLayout = GUILayout.Height(40f);
        private MonoScript m_type;
        
        [MenuItem(MenuPathsUtility.baseMenu + "TypeCreator")]
        private static void ShowWindow()
        {
            var window = GetWindow<TypeCreatorEditorWindow>();
            window.titleContent = new GUIContent("TypeCreator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10f);
            this.m_toolbarValue = GUILayout.Toolbar(this.m_toolbarValue, new[]
            {
                m_dataObjectName,
                m_eventName,
                m_registryName, 
                m_eventRegistryName
            });
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
                if(DrawButton(m_dataObjectName))
                {
                    TypeCreator.Create(typeof(DataObject<>), "Object", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.dataObjectsMenu)}",
                        m_type.GetClass());
                }
            }
            //Event
            else if (m_toolbarValue == 1)
            {
                if(DrawButton(m_eventName))
                {
                    TypeCreator.Create(typeof(EventObject<>), "Event", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.eventsMenu)}",
                        m_type.GetClass());
                }
            }
            //Registry
            else if (m_toolbarValue == 2)
            {
                if(DrawButton(m_registryName))
                {
                    TypeCreator.Create(typeof(GlobalVariablesRegistry<>), "Registry", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.registryMenu)}", 
                        m_type.GetClass());
                }
            }
            //EventRegistry
            else if (m_toolbarValue == 3)
            {
                if(DrawButton(m_eventRegistryName))
                {
                    TypeCreator.Create(typeof(GlobalVariablesRegistry<>), "EventRegistry", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.eventsRegistryMenu)}",
                        typeof(Event<>), m_type.GetClass());
                };
            }

            GUI.enabled = true;
        }

        private bool DrawButton(string postfix)
        {
            return (GUILayout.Button("Create new " + postfix + " type", m_buttonLayout));
        }
    }
}