using System;
using UnityEditor;
using UnityEngine;
using SceneAttribute = nickeltin.Extensions.Attributes.SceneAttribute;

namespace nickeltin.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField (position, label.text, "Use [Scene] with strings.");
                return;
            }
            
            
            var sceneObject = GetSceneObject(property.stringValue);
            var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), false);
            if (scene == null) 
            {
                property.stringValue = string.Empty;
            } 
            else if (scene.name != property.stringValue) 
            {
                var sceneObj = GetSceneObject(scene.name);
                if (sceneObj == null) 
                {
                    Debug.LogError("The scene " + scene.name + " cannot be used. To use this scene add it to the build settings for the project");
                }
                else 
                {
                    property.stringValue = scene.name;
                }
            }
           
        }
        private SceneAsset GetSceneObject(string sceneObjectName) 
        {
            if (string.IsNullOrEmpty(sceneObjectName)) 
            {
                return null;
            }
 
            foreach (var editorScene in EditorBuildSettings.scenes) 
            {
                if (editorScene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath<SceneAsset>(editorScene.path);
                }
            }
            
            //Debug.LogError("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
            return null;
        }
    }
}