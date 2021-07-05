using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace nickeltin.Editor.Extensions
{
    public static class ReorderableListExt
    {
        public static Object GetSelectedObject(this ReorderableList list, SerializedProperty source)
        {
            return source.GetArrayElementAtIndex(list.index).objectReferenceValue;
        }
    }
}