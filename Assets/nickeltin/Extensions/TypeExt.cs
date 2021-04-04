using System;
using System.Collections.Generic;
using System.Reflection;

namespace nickeltin.Extensions
{
    public static class TypeExt
    {
        public static Type GetGenericInheritor(this Type parentType, Type[] parameterType)
        {
            var types = Assembly.GetAssembly(parentType).GetTypes(); 
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
            foreach (var s in v) types.Add(Type.GetType(s));
            types.RemoveAt(0);
            return types.Count > 0 ? types.ToArray() : null;
        }
    }
}