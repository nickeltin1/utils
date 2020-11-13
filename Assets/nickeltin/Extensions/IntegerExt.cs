namespace nickeltin.Extensions.Int
{
    public static class IntegerExt
    {
        public static bool Negative(this int i) => i < 0;
        public static bool Positive(this int i) => i > 0;
        public static bool Zero(this int i) => i == 0;
        public static bool NegativeOrZero(this int i) => Zero(i) || Negative(i);
        public static bool PositiveOrZero(this int i) => Zero(i) || Positive(i);
    }
}