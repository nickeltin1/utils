using System.Collections.Generic;
using nickeltin.Extensions.Attributes;
using UnityEditor;
using UnityEngine;

namespace nickeltin.Editor.Extensions
{
    public static class SerializedObjectExt
    {
        public static IEnumerable<SerializedProperty> GetVisibleChildrens(this SerializedObject serializedObject)
        {
            using (var iterator = serializedObject.GetIterator().Copy())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                        if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
                            continue;

                        yield return childProperty;
                    }
                    while (iterator.NextVisible(false));
                }
            }
        }


        public static IEnumerable<SerializedProperty> GetChildrens(this SerializedObject serializedObject)
        {
            using (var iterator = serializedObject.GetIterator().Copy())
            {
                if (iterator.Next(true))
                {
                    do
                    {
                        SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
                        if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
                            continue;

                        yield return childProperty;
                    }
                    while (iterator.Next(false));
                }
            }
        }
    }
}