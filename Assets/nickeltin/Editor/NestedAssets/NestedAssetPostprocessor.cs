using nickeltin.Runtime.NestedAssets;
using UnityEditor;

namespace nickeltin.Editor.NestedAssets
{
    public class NestedAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            for (int i = 0; i < movedAssets.Length; i++)
            {
                var type = AssetDatabase.GetMainAssetTypeAtPath(movedAssets[i]);
                if (type.IsSubclassOf(typeof(NestedAssetParentBase))) RenameAllChilds(movedAssets[i]);
            }
        }

        private static void RenameAllChilds(string path)
        {
            var asset = AssetDatabase.LoadAssetAtPath<NestedAssetParentBase>(path);
            
            SerializedObject serializedObject = new SerializedObject(asset);

            var settings = NestedAssetRootEditor.GetSettings(serializedObject);
            char nameSeparator = settings?.nameSeparator ?? NestedAssetParentBase.DEFAULT_NAME_SEPARATOR;
            
            SerializedProperty childs = serializedObject.FindProperty(NestedAssetParent<NestedAsset>.childs_prop_name);

            
            
            for (int i = 0; i < childs.arraySize; i++)
            {
                var child =  (NestedAsset)childs.GetArrayElementAtIndex(i).objectReferenceValue;
                var childName = NestedAssetRootEditor.GetChildNameProperty(child, 
                    out var serializedObj);
                
                child.name = NestedAssetRootEditor.GetProperChildName(asset.name, childName.stringValue, nameSeparator);
                
                serializedObj.ApplyModifiedProperties();
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}