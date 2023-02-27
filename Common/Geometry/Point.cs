namespace MobileNetworkFramework.Common.Geometry
{
    public class Point
    {
        #region Properties

        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;

        #endregion


        #region Constructors

        private void Init(double xValue, double yValue)
        {
            X = xValue;
            Y = yValue;
        }
        public Point(double x, double y)
        {
            Init(x, y);
        }
        public Point()
        {
            Init(0, 0);
        }

        #endregion


        #region Operators

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            if (!(a is null) && !(b is null))
                return Math.Abs(a.X - b.X) < Extensions.Epsilon && Math.Abs(a.Y - b.Y) < Extensions.Epsilon;
            else
                return false;
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        public static double operator *(Point v, Point w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static Point operator *(Point v, double mult)
        {
            return new Point(v.X * mult, v.Y * mult);
        }

        public static Point operator *(double mult, Point v)
        {
            return new Point(v.X * mult, v.Y * mult);
        }

        #endregion


        #region Override Methods

        public override bool Equals(object? other)
        {
            return other is Point point && Equals(point);
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() << 2;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }

        #endregion


        #region Public Methods

        public double Cross(Point v)
        {
            return X * v.Y - Y * v.X;
        }

        #endregion


        #region Static Methods

        public static float GetDistanceBetweenPoints(Point first, Point second)
        {
            return (float)Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
        }

        #endregion
    }
}
