namespace MobileNetworkFramework.Common
{
    public static class Extensions
    {
        public const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }

        public static bool Sign(this double d)
        {
            return d >= 0;
        }
    }
}
