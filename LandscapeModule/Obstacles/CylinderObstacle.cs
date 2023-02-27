using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class CylinderObstacle: IObstacle, IConnectableObstacle, IIntersectableObstacle
    {
        #region Properties

        public Point Center { get; private set; }

        public float Range { get; private set; }

        #endregion

        public CylinderObstacle(Point center, float range)
        {
            Center = center;
            Range = range;
        }
        
        public bool DoesItIntersect(Point start, Point end)
        {
            if (start is null || end is null) return false;
            start -= Center;
            end -= Center;
            
            var dx = end.X - start.X;
            var dy = end.Y - start.Y;
            var r = Range;

            var a = dx * dx + dy * dy;
            var b = 2.0 * (start.X * dx + start.Y * dy);
            var c = start.X * start.X + start.Y * start.Y - r * r;


            if (-b < 0) return (c < 0);
            if (-b < (2.0 * a)) return ((4.0 * a * c - b * b) < 0);

            return (a + b + c < 0);
        }

        public bool IsPointBelongTo(Point point)
        {
            return Point.GetDistanceBetweenPoints(point, Center) < Range;
        }

        public List<Point> GetIntersectionPoints(Point start, Point end, out int numOfPoints)
        {
            numOfPoints = 0;
            start -= Center;
            end -= Center;
            
            double r = Range, a = start.Y - end.Y, 
                b = end.X - start.X, c = start.X * end.Y - start.Y * end.X;

            double x0 = -a * c / (a * a + b * b),  y0 = -b * c / (a * a + b * b);
            if (c * c > r * r * (a * a + b * b) + double.Epsilon) return new List<Point>();
            
            if (Math.Abs(c * c - r * r * (a * a + b * b)) < double.Epsilon)
            {
                numOfPoints = 1;
                return new List<Point>(1){ new Point(x0, y0) };
            }
            
            var d = r * r - c * c / (a * a + b * b);
            var mult = Math.Sqrt(d / (a * a + b * b));
            double ax = x0 + b * mult, ay = y0 - a * mult, bx = x0 - b * mult, by = y0 + a * mult;
            numOfPoints = 2;
            return new List<Point>(2) {new Point(ax,ay), new Point(bx,by)};
        }
    }
}

