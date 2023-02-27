namespace MobileNetworkFramework.Common.Geometry
{
    public static class Segment
    {
        #region Intersection

        /// <summary>
        /// Проверяет пересекаются ли два отрезка, и если да, возвращает true и точку пересечения
        /// </summary>
        /// <param name="p">Начало первого отрезка</param>
        /// <param name="p2">Конец первого отрезка</param>
        /// <param name="q">Начало второго отрезка</param>
        /// <param name="q2">Конец второго отрезка</param>
        /// <param name="considerCollinearOverlapAsIntersect">Рассматривать коллинеарность как пересечение</param>
        /// <returns></returns>
        public static bool LineSegmentsIntersect(Point p, Point p2, Point q, Point q2, bool considerCollinearOverlapAsIntersect = false)
        {
            return LineSegmentsIntersect(p, p2, q, q2, out var point, considerCollinearOverlapAsIntersect);
        }
        
        
        /// <summary>
        /// Checking intersection of two segments
        /// </summary>
        /// <param name="p">Start of first segment</param>
        /// <param name="p2">End of first segment</param>
        /// <param name="q">Start of second segment</param>
        /// <param name="q2">End of second segment</param>
        /// <param name="intersection">Intersection point of two segments</param>
        /// <param name="considerCollinearOverlapAsIntersect"></param>
        /// <returns></returns>
        public static bool LineSegmentsIntersect(Point p, Point p2, Point q, Point q2, out Point intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Point();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            if (rxs.IsZero() && qpxr.IsZero())
            {
                if (!considerCollinearOverlapAsIntersect) return false;
                return (0 <= (q - p) * r && (q - p) * r <= r * r) || (0 <= (p - q) * s && (p - q) * s <= s * s);
            }

            if (rxs.IsZero() && !qpxr.IsZero()) return false;

            var t = (q - p).Cross(s) / rxs;
            var u = (q - p).Cross(r) / rxs;

            if (rxs.IsZero() || (!(0 <= t) || !(t <= 1)) || (!(0 <= u) || !(u <= 1))) return false;
            intersection = p + t * r;
            return true;
        }

        
        /// <summary>
        /// Checks two segments on intersection, with assumption that same start or end point does not count as intersection
        /// </summary>
        /// <param name="p">Start of first segment</param>
        /// <param name="p2">End of first segment</param>
        /// <param name="q">Start of second segment</param>
        /// <param name="q2">End of second segment</param>
        /// <param name="considerSameBeginOrEndAsIntersection"></param>
        /// <returns></returns>
        public static bool LineSegmentsIntersectWithAssumption(Point p, Point p2, Point q, Point q2, bool considerSameBeginOrEndAsIntersection = true)
        {
            if (!considerSameBeginOrEndAsIntersection) return LineSegmentsIntersect(p, p2, q, q2);
            if (p == q || p == q2 || p2 == q || p2 == q2) return false;
            return LineSegmentsIntersect(p, p2, q, q2);
        }

        #endregion
        

        #region Middle Point Of Segment

        public static Point GetMiddlePoint(Point start, Point end)
        {
            var result = new Point
            {
                X = (start.X + end.X) / 2,
                Y = (start.Y + end.Y) / 2
            };
            return result;
        }
        
        public static void GetMiddlePoint(Point start, Point end, ref Point result)
        {
            result.X = (start.X + end.X) / 2;
            result.Y = (start.Y + end.Y) / 2;
        }

        #endregion
    }
}
