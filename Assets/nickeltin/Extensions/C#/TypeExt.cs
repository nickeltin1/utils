using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace nickeltin.Extensions
{
    //TODO: If working with assembly defenitions, rewrite all that
    public static class TypeExt
    {
        public static Type GetGenericInheritor(this Type parentType, Type[] parameterType)
        {
            HashSet<Assembly> assemblies = new HashSet<Assembly> {Assembly.GetAssembly(parentType)};
            for (int i = 0; i < parameterType.Length; i++)
            {
                if(parameterType[i] != null) assemblies.Add(Assembly.GetAssembly(parameterType[i]));
            }

            List<Type> types = new List<Type>();

            foreach (var assembly in assemblies) types.AddRange(assembly.GetTypes());

            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType && 
                    type.BaseType.GetGenericTypeDefinition() == parentType)
                {
                    var hierarchy = TypeExt.GetGenericHierarchy(type.BaseType.ToString());
                    if (hierarchy.Length != parameterType.Length) continue;

                    bool parametersMatch = true;
                    for (var i = 0; i < parameterType.Length; i++)
                    {
                        if (parameterType[i] != hierarchy[i])
                        {
                            parametersMatch = false;
                            break;
                        }
                    }
                    
                    if (parametersMatch) return type;
                }
            }
            
            return null;
        }
        
        public static Type[] GetGenericHierarchy(string fullName)
        {
            const char openChar = '[';
            const char closeChar = ']';
            
            List<Type> types = new List<Type>();
            var v = fullName.Split(new []{openChar, closeChar}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in v) types.Add(TypeExt.GetType(s));
            types.RemoveAt(0);
            return types.Count > 0 ? types.ToArray() : null;
        }
        
        public static IEnumerable<Type> GetChildTypes(this Type type)
        {
            foreach (Type childType in Assembly.GetAssembly(type).GetTypes())
            {
                if (childType.IsClass && !childType.IsAbstract && childType.IsSubclassOf(type))
                {
                    yield return childType;
                }
            }
        }

        public static Type GetType(string name)
        {
            var type = Type.GetType(name);
            
            if (type != null) return type;
            
            // if (name.Contains("."))
            // {
            //     var assemblyName = name.Substring(0, name.IndexOf('.'));
            //     
            //     var assembly = Assembly.Load(assemblyName);
            //     if (assembly == null) return null;
            //     
            //     type = assembly.GetType(name);
            //     if (type != null) return type;
            // }
            
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach (var assemblyName in referencedAssemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly != null)
                {
                    type = assembly.GetType(name);
                    if (type != null) return type;
                }
            }
            
            return null;
        }
    }
}