namespace nickeltin.Extensions
{
    public static class ObjectExt
    {
        public static bool IsSerializable(this object obj)
        {
            return obj != null && obj.GetType().IsSerializable;
        }
    }
}