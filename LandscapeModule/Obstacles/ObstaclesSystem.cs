using MobileNetworkFramework.Common;
using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class ObstaclesSystem : IObstacle
    {
        public enum ObstacleType
        {
            Cylinder, Sharp, Transparent
        }

        public int ObstaclesNumber => _obstacles.Count + _customObstacles.Count;
        public int ComplexObstaclesNumber => _complexObstacles.Count;

        public List<IIntersectableObstacle> IntersectableObstacles
        {
            get
            {
                var result = new List<IIntersectableObstacle>(_obstacles.Count);
                foreach (var obstacle in _obstacles)
                {
                    if (obstacle is IIntersectableObstacle intersectableObstacle) result.Add(intersectableObstacle);
                }
                return result;
            }
        }
        
        private List<IObstacle> _obstacles = new List<IObstacle>();
        private List<ComplexObstacle> _complexObstacles = new List<ComplexObstacle>();
        private List<CustomObstacle> _customObstacles = new List<CustomObstacle>();
        
        public ObstaclesSystem(){}

        #region Creating Obstacles Methods

        /// <summary>
        /// Searching for complex obstacles in obstacles system
        /// </summary>
        /// <returns>Number of complex obstacles</returns>
        public int SearchForComplexObstacles()
        {
            _complexObstacles = ComplexObstacle.SearchForComplexObstacles(_obstacles);
            return _complexObstacles.Count;
        }

        /// <summary>
        /// Creating obstacles of certain type
        /// </summary>
        /// <param name="obstacleType">Only Cylinder, Sharp, Transparent is accepted</param>
        /// <param name="numberOfObstacles"> Number of obstacles</param>
        /// <param name="range">Range of obstacles</param>
        /// <param name="terrainQuad">Quad for generating random inside point</param>
        /// <exception cref="Exception">Incorrect obstacle type</exception>
        public void CreateObstacles(ObstacleType obstacleType, int numberOfObstacles, float range, Quad terrainQuad)
        {
            if (obstacleType is not (ObstacleType.Cylinder or ObstacleType.Sharp or ObstacleType.Transparent))
                throw new Exception($"ObstaclesSystem: Incorrect obstacle type ({obstacleType.ToString()})");
            for (var i = 0; i < numberOfObstacles; i++)
            {
                var center = Randomizer.RandomPointInQuad(terrainQuad);
                if (obstacleType == ObstacleType.Cylinder)
                {
                    _obstacles.Add(new CylinderObstacle(center, range));
                }

                if (obstacleType is ObstacleType.Sharp or ObstacleType.Transparent)
                {
                    var numberOfVertices = Randomizer.RandomInt(SharpObstacle.GetMinVerticesNumber(),
                        SharpObstacle.GetMaxVerticesNumber());
                    _obstacles.Add(obstacleType == ObstacleType.Sharp
                        ? new SharpObstacle(center, range, numberOfVertices)
                        : new TransparentObstacle(center, range, numberOfVertices));
                }
            }
        }

        public void AddCustomObstacle(CustomObstacle customObstacle) => _customObstacles.Add(customObstacle);

        #endregion

        #region IObstacle

        public bool IsPointBelongTo(Point point)
        {
            foreach (var obstacle in _obstacles) if (obstacle.IsPointBelongTo(point)) return true;
            foreach (var customObstacle in _customObstacles) if (customObstacle.IsPointBelongTo(point)) return true;
            return false;
        }

        public bool DoesItIntersect(Point start, Point end)
        {
            foreach (var obstacle in _obstacles)
            {
                if (obstacle.DoesItIntersect(start, end)) return true;
            }
            
            foreach (var customObstacle in _customObstacles)
            {
                if (customObstacle.DoesItIntersect(start, end)) return true;
            }

            return false;
        }

        #endregion
    }
}

