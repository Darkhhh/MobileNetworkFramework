using MobileNetworkFramework.Common;
using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class CustomObstacle: IObstacle, IIntersectableObstacle
    {
        #region enum

        public enum Type
        {
            Transparent, NonTransparent
        }

        #endregion
        
        
        #region Private Values

        private readonly Point _center;
        
        private readonly List<Point> _vertices;

        private Type _type = Type.NonTransparent;

        #endregion


        #region Constructors

        /// <summary>
        /// Custom obstacle of a convex shape
        /// </summary>
        /// <param name="center">Point inside of an obstacle</param>
        /// <param name="vertices">Vertices of an obstacle (last vert must be duplicate of the first)</param>
        /// <param name="obstacleType">Type of an custom obstacle (options in CustomObstacle.Type)</param>
        /// <exception cref="Exception">If (center not inside obstacle) or (shape is concave) exception will be thrown</exception>
        public CustomObstacle(Point center, List<Point> vertices, Type obstacleType)
        {
            _center = center;
            _vertices = vertices;
            _type = obstacleType;
            if (!CheckInitVertices(center, vertices, out var message)) throw new Exception(message);
        }

        #endregion
        
        
        #region Private Methods

        private bool CheckInitVertices(Point center, List<Point> vertices, out string exceptionMessage)
        {
            var length = vertices.Count;
            var prd = new double[length];
            Point first = new Point(), second = new Point();
            for (var i = 0; i < length; i++)
            {
                first.X = vertices[i].X - vertices[Utils.PreviousIndex(i, length)].X;
                first.Y = vertices[i].Y - vertices[Utils.PreviousIndex(i, length)].Y;
                second.X = vertices[Utils.NextIndex(i, length)].X - vertices[i].X;
                second.Y = vertices[Utils.NextIndex(i, length)].Y - vertices[i].Y;
                prd[i] = first.X * second.Y - first.Y * second.X;
            }

            var sign = prd[0].Sign();
            foreach (var d in prd)
                if (d.Sign() != sign)
                {
                    exceptionMessage = "Non concave shape";
                    return false;
                }

            var changedType = false;
            if (_type == Type.Transparent)
            {
                changedType = true;
                _type = Type.NonTransparent;
            }
            foreach (var vertex in vertices)
            {
                if (DoesItIntersect(vertex, center))
                {
                    exceptionMessage = "Center is out of the obstacle";
                    return false;
                }
            }

            if (changedType) _type = Type.Transparent;
            exceptionMessage = string.Empty;
            return true;
        }

        #endregion
        
        
        #region IObstacle

        public bool IsPointBelongTo(Point point)
        {
            for (var j = 0; j < _vertices.Count; j++)
                if (Segment.LineSegmentsIntersect(
                        _vertices[j],
                        _vertices[Utils.NextIndex(j, _vertices.Count)],
                        point,
                        _center))
                {
                    return false;
                }
                    
            return true;
        }

        public bool DoesItIntersect(Point start, Point end)
        {
            switch (_type)
            {
                case Type.NonTransparent:
                {
                    for (var j = 0; j < _vertices.Count; j++) 
                        if (Segment.LineSegmentsIntersectWithAssumption(_vertices[j], _vertices[Utils.NextIndex(j, _vertices.Count)], start, end)) 
                            return true;
                    return false;
                }
                case Type.Transparent:
                    return false;
                default:
                    throw new Exception(
                        $"ComplexObstacle: The behaviour for this ComplexObstacle.Type is not implemented({_type.ToString()})");
            }
        }

        #endregion

        public List<Point> GetIntersectionPoints(Point start, Point end, out int numOfPoints)
        {
            var intersectionPoints = new List<Point>(2);
            for (var j = 0; j < _vertices.Count - 1; j++)
            {
                if (Segment.LineSegmentsIntersect(_vertices[j], _vertices[j + 1], start, end, out var point))
                    intersectionPoints.Add(point);
            }

            numOfPoints = intersectionPoints.Count;
            return intersectionPoints;
        }
    }
}

