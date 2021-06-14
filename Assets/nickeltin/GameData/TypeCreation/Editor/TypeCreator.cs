using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using nickeltin.Extensions;
using UnityEditor;
using UnityEngine;

namespace nickeltin.GameData.Editor.TypeCreation
{
    public static class TypeCreator
    {
        public static void Create(Type genericParentType, string name, string menuPath, params Type[] parameterTypes)
        {
            if (parameterTypes == null || parameterTypes.Length == 0)
            {
                throw new ArgumentException("Parameter types is not specified");
            }

            if (genericParentType == null)
            {
                throw new ArgumentException("Parent type is not specified");
            }

            string path = EditorUtility.SaveFilePanelInProject("Save event object", 
                $"{parameterTypes[parameterTypes.Length - 1].Name}{name}", 
                "cs", "Please enter a file name to save the texture to");

            if (path.IsNullOrEmpty()) return;
            
            StringBuilder stringBuilder = new StringBuilder();
            List<string> usedNamespaces = new List<string>();

            foreach (var type in parameterTypes)
            {
                if (type.Namespace != null && !usedNamespaces.Contains(type.Namespace))
                {
                    usedNamespaces.Add(type.Namespace);
                }
            }

            foreach (var @namespace in usedNamespaces)
            {
                stringBuilder.AppendLine($"using {@namespace};");
            }
            
            stringBuilder.AppendLine($"using {nameof(UnityEngine)};");

            if (menuPath != null)
            {
                stringBuilder.AppendLine($"using {nameof(nickeltin)}.{nameof(nickeltin.Editor)}.{nameof(nickeltin.Editor.Utility)};");
            }

            string usings = stringBuilder.ToString();
            
            stringBuilder.Clear();

            stringBuilder.Append(genericParentType.Name.Split('`').First());

            for (var i = 0; i < parameterTypes.Length; i++)
            {
                stringBuilder.Append("<");
                stringBuilder.Append(parameterTypes[i].Name.Split('`').First());
            }

            for (int i = 0; i < parameterTypes.Length; i++) stringBuilder.Append(">");

            string parentType = stringBuilder.ToString();

            WriteFile(path, usings, genericParentType.Namespace + ".CustomTypes", parentType, menuPath);
        }

        private static void WriteFile(string path, string usings, string @namespace, string parent, string menuPath = null)
        {
            string name = path.Split('/').Last().Split('.').First();
            
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.WriteLine(usings);
                streamWriter.WriteLine($"namespace {@namespace}");
                streamWriter.WriteLine("{");

                if (menuPath != null)
                {
                    streamWriter.WriteLine($"    [CreateAssetMenu(menuName = {menuPath} + nameof({name}))]");
                }
                
                streamWriter.WriteLine($"    public class {name} : {parent}");
                streamWriter.WriteLine("    {");
                streamWriter.WriteLine("    }");
                streamWriter.WriteLine("}");
            }
            
            AssetDatabase.Refresh();
        }
    }
}