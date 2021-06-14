using System;
using nickeltin.Editor.Utility;
using nickeltin.Extensions;
using nickeltin.GameData.DataObjects;
using nickeltin.GameData.Events;
using nickeltin.GameData.Events.Types;
using nickeltin.GameData.GlobalVariables;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor.TypeCreation
{
    public class TypeCreatorEditorWindow : EditorWindow
    {
        private const string m_dataObjectName = "Data Object";
        private const string m_eventName = "Event Object";
        private const string m_registryName = "Registry";
        private const string m_eventRegistryName= "Event Reigstry";
        private const string m_eventListenerName = "Event Listener"; 
        
        
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
            this.m_toolbarValue = GUILayout.SelectionGrid(this.m_toolbarValue, new[]
            {
                m_dataObjectName,
                m_eventName,
                m_registryName, 
                m_eventRegistryName,
                m_eventListenerName
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
                if(DrawButton(m_dataObjectName))
                {
                    TypeCreator.Create(typeof(DataObject<>), "Object", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.dataObjectsMenu)}",
                        GetTargetedType());
                    
                }
            }
            //Event
            else if (m_toolbarValue == 1)
            {
                if(DrawButton(m_eventName))
                {
                    TypeCreator.Create(typeof(EventObject<>), "Event", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.eventsMenu)}",
                        GetTargetedType());
                }
            }
            //Registry
            else if (m_toolbarValue == 2)
            {
                if(DrawButton(m_registryName))
                {
                    TypeCreator.Create(typeof(GlobalVariablesRegistry<>), "Registry", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.registryMenu)}", 
                        GetTargetedType());
                }
            }
            //EventRegistry
            else if (m_toolbarValue == 3)
            {
                if(DrawButton(m_eventRegistryName))
                {
                    TypeCreator.Create(typeof(GlobalVariablesRegistry<>), "EventRegistry", 
                        $"{nameof(MenuPathsUtility)}.{nameof(MenuPathsUtility.eventsRegistryMenu)}",
                        typeof(Event<>), GetTargetedType());
                };
            }
            //EventListener
            else if (m_toolbarValue == 4)
            {
                if(DrawButton(m_eventListenerName))
                {
                    TypeCreator.Create(typeof(EventListener<>), "EventListener", 
                        null, GetTargetedType());
                };
            }

            GUI.enabled = true;
        }

        private Type GetTargetedType()
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

            if (type != null && !type.IsSerializable)
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