using System;

namespace nickeltin.Extensions.Enums
{
    public static class EnumExt
    {
        public static T[] GetAllEnumValues<T>() where T : Enum => (T[]) Enum.GetValues(typeof(T));
    }
}