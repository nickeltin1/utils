using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace nickeltin.Extensions
{
    public static class TypeExt
    {
        public static Type GetGenericInheritor(this Type parentType, Type parameterType)
        {
            var types = Assembly.GetAssembly(parentType).GetTypes(); 
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType && 
                    type.BaseType.GetGenericTypeDefinition() == parentType &&
                    parameterType == TypeExt.GetGenericParameterTypeFromFullName(type.BaseType.ToString()))
                {
                    return type;
                }
            }
            
            return null;
        }
        
        public static Type GetGenericParameterTypeFromFullName(string fullName)
        {
            int start = fullName.IndexOf('[')+1;
            fullName = fullName.Substring(start, fullName.IndexOf(']') - start);
            string assemblyName = fullName.Split('.').First();
            Type t;
            if (assemblyName.Equals(nameof(System))) t = Type.GetType(fullName);
            else t = Type.GetType($"{fullName}, {assemblyName}");
            return t;
        }
    }
}