using System;

namespace nickeltin.Runtime.Enums
{
    public class EnumUtils
    {
        public static T[] GetAllEnumValues<T>() where T : Enum => (T[]) Enum.GetValues(typeof(T));
    }
}