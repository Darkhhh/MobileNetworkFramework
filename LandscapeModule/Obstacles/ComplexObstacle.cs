using MobileNetworkFramework.Common;
using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.LandscapeModule.Obstacles
{
    public class ComplexObstacle:IObstacle
    {
        #region Properties

        public int ObstaclesAmount => _obstacles.Count;

        public List<IConnectableObstacle> Obstacles => _obstacles;

        #endregion
        
        
        #region Private Values

        private List<IConnectableObstacle> _obstacles;

        #endregion
        
        
        #region Constructors

        public ComplexObstacle(IConnectableObstacle obstacle)
        {
            _obstacles = new List<IConnectableObstacle> {obstacle};
        }

        public ComplexObstacle()
        {
            _obstacles = new List<IConnectableObstacle>();
        }

        #endregion


        #region Private Methods

        private void AddObstacle(IConnectableObstacle obstacle)
        {
            _obstacles.Add(obstacle);
        }

        #endregion
        
        
        #region IObstacle

        public bool IsPointBelongTo(Point point)
        {
            foreach (var obstacle in _obstacles) if (obstacle.IsPointBelongTo(point)) return true;
            return false;
        }

        public bool DoesItIntersect(Point start, Point end)
        {
            foreach (var obstacle in _obstacles) if (obstacle.DoesItIntersect(start, end)) return true;
            return false;
        }

        #endregion


        #region Static

        public static List<ComplexObstacle> SearchForComplexObstacles(List<IConnectableObstacle> obstacles)
        {
            var complexObstacles = new List<ComplexObstacle>();
            
            #region Creating Adjacency List

            var adjacencyList = new Dictionary<IConnectableObstacle, HashSet<IConnectableObstacle>>(obstacles.Count);
            
            for (var i = 0; i < obstacles.Count; i++)
            {
                var neighbours = new HashSet<IConnectableObstacle>(obstacles.Count);
                for (var j = 0; j < obstacles.Count; j++)
                {
                    if (i == j) continue;
                    if (IConnectableObstacle.AreObstaclesIntersect(obstacles[i], obstacles[j]))
                        neighbours.Add(obstacles[j]);
                }
                adjacencyList.Add(obstacles[i], neighbours);
            }

            #endregion

            #region Finding Complex Obstacles With Depth First Search

            var used = new List<bool>(obstacles.Count);
            for (var i = 0; i < obstacles.Count; i++) used.Add(false);

            for (var i = 0; i < obstacles.Count; i++)
            {
                if(used[i]) continue;
                var setOfConnectedObstacles = Algorithms.DepthFirstSearch(adjacencyList, obstacles[i]);
                for (var j = 0; j < obstacles.Count; j++)
                    if (setOfConnectedObstacles.Contains(obstacles[j])) used[j] = true;
                var complexObstacle = new ComplexObstacle();
                foreach (var obstacle in setOfConnectedObstacles) complexObstacle.AddObstacle(obstacle);
                complexObstacles.Add(complexObstacle);
            }

            #endregion
            
            return complexObstacles;
        }

        public static List<ComplexObstacle> SearchForComplexObstacles(List<IObstacle> obstacles)
        {
            var connectableObstacles = new List<IConnectableObstacle>();
            foreach (var obstacle in obstacles)
            {
                if(obstacle is IConnectableObstacle connectableObstacle) connectableObstacles.Add(connectableObstacle);
            }

            return SearchForComplexObstacles(connectableObstacles);
        }
        
        #endregion
    }
}

