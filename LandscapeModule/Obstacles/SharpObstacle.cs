using MobileNetworkFramework.Common;
using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class SharpObstacle: IObstacle, IConnectableObstacle, IIntersectableObstacle
    {
        #region Static

        private static int _minVerticesNumber = -1;
        private static int _maxVerticesNumber = -1;
        private static bool _staticValuesWereInitialized = false;

        public static bool ValuesWereInitialized => _staticValuesWereInitialized;
        
        public static void SetMinMaxVertices(int minVerticesNumber, int maxVerticesNumber)
        {
            if (minVerticesNumber < 0 || maxVerticesNumber < 0)
                throw new Exception("Less then zero initial values");
            if (maxVerticesNumber < minVerticesNumber)
                throw new Exception("Max value less then zero value");
            
            if (!_staticValuesWereInitialized)
            {
                _staticValuesWereInitialized = true;
                _minVerticesNumber = minVerticesNumber;
                _maxVerticesNumber = maxVerticesNumber;
            }
            else throw new Exception("Trying to reinitialize static values");
        }

        public static int GetMinVerticesNumber()
        {
            if (_minVerticesNumber < 0) throw new Exception("MinVerticesNumber is not initialized (or it's value < 0)");
            return _minVerticesNumber;
        }
        
        public static int GetMaxVerticesNumber()
        {
            if (_maxVerticesNumber < 0) throw new Exception("MaxVerticesNumber is not initialized (or it's value < 0)");
            return _maxVerticesNumber;
        }

        public static void Reset()
        {
            _minVerticesNumber = -1;
            _maxVerticesNumber = -1;
            _staticValuesWereInitialized = false;
        }

        #endregion
        
        
        #region Private Values

        private protected int _numberOfVertices = 0;

        private protected List<Point> _vertices = new List<Point>();

        private protected double[] _lengths = Array.Empty<double>();

        #endregion
        
        
        #region Properties

        public Point Center { get; set; }

        public float Range { get; set; }

        public List<Point> Vertices => _vertices;

        #endregion


        #region Constructors

        /// <summary>
        /// Creates an instance of SharpObstacle
        /// </summary>
        /// <param name="center">Center point of an obstacle</param>
        /// <param name="range">Circumscribed range of a circle of an obstacle </param>
        /// <param name="numberOfVertices">
        /// Must be generated in [SharpObstacle.GetMinVerticesNumber(); SharpObstacle.GetMaxVerticesNumber()] range,
        /// which should be set before by using SetMinMaxVertices(int min, int max). </param>
        /// <exception cref="Exception">Throws exception if range less then 0 or numberOfVertices less then 3</exception>
        
        public SharpObstacle(Point center, float range, int numberOfVertices)
        {
            if (range <= 0 || numberOfVertices < 3)
                throw new Exception("Incorrect initial values");
            Center = center;
            Range = range;
            _numberOfVertices = numberOfVertices;
            GenerateObstacle();
        }

        /// <summary>
        /// Creates an instance of SharpObstacle with Center = (0;0) and Range = 1.0f
        /// </summary>
        public SharpObstacle()
        {
            _numberOfVertices = Randomizer.RandomInt(
                SharpObstacle.GetMinVerticesNumber(),
                SharpObstacle.GetMaxVerticesNumber());
            Center = new Point();
            Range = 1.0f;
            GenerateObstacle();
        }

        #endregion
        
        
        #region Private Methods

        private List<double> GenerateLengths()
        {
            var lengths = new List<double> { 0, (Math.PI * Range * 2) / 3, (Math.PI * Range * 4) / 3 };
            for (var i = 0; i < _numberOfVertices - 3; i++) lengths.Add(Randomizer.RandomDouble(0.1, 2 * Math.PI * Range));
            lengths.Sort();
            return lengths;
        }

        private void GenerateObstacle()
        {
            _vertices.Clear();
            var lengths = GenerateLengths();
            _lengths = lengths.ToArray();
            foreach (var length in lengths)
            {
                _vertices.Add(new Point(
                    Center.X + (Range * Math.Sin(length / Range)),
                    Center.Y + (Range * Math.Cos(length / Range)))
                );
            }
            _vertices.Add(_vertices[0]);
        }

        #endregion


        #region Interfaces

        public virtual bool DoesItIntersect(Point start, Point end)
        {
            for (var j = 0; j < _vertices.Count - 1; j++) if (Segment.LineSegmentsIntersect(_vertices[j], _vertices[j + 1], start, end)) return true;
            return false;
        }

        public virtual bool IsPointBelongTo(Point point)
        {
            return Point.GetDistanceBetweenPoints(point, Center) < Range && !DoesItIntersect(point, Center);
        }
        
        public virtual List<Point> GetIntersectionPoints(Point start, Point end, out int numOfPoints)
        {
            var intersectionPoints = new List<Point>(2);
            for (var j = 0; j < Vertices.Count - 1; j++)
            {
                if (Segment.LineSegmentsIntersect(Vertices[j], Vertices[j + 1], start, end, out var point))
                    intersectionPoints.Add(point);
            }

            numOfPoints = intersectionPoints.Count;
            return intersectionPoints;
        }

        #endregion
    }
}

