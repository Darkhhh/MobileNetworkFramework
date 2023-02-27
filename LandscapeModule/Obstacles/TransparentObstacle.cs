using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class TransparentObstacle: SharpObstacle
    {
        #region Override SharpObstacle

        public override bool IsPointBelongTo(Point point)
        {
            for (var j = 0; j < _vertices.Count - 1; j++) if (Segment.LineSegmentsIntersect(_vertices[j], _vertices[j + 1], point, Center)) return false;
            return true;
        }

        public override bool DoesItIntersect(Point start, Point end)
        {
            return false;
        }

        public override List<Point> GetIntersectionPoints(Point start, Point end, out int numOfPoints)
        {
            numOfPoints = 0;
            return new List<Point>();
        }

        #endregion

        public TransparentObstacle(Point center, float range, int numberOfVertices) : base(center, range, numberOfVertices)
        {
            
        }
    }
}
