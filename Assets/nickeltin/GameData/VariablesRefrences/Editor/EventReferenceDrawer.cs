﻿using nickeltin.GameData.DataObjects;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor
{
    [CustomPropertyDrawer(typeof(EventReferenceBase), true)]
    public sealed class EventReferenceDrawer : ReferenceDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePopupStyle();
            
            BeginProperty(ref position, property, ref label);
            
            CacheReferenceType(property, "_referenceType");
            
            SerializedProperty eventObject = property.FindPropertyRelative("_eventObject");
            SerializedProperty gloablEvent = property.FindPropertyRelative("_globalEvent");

            DrawProperty(position, property,eventObject, gloablEvent);

            EndProperty(property);
        }
    }
}