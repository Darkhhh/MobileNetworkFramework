using MobileNetworkFramework.LandscapeModule.Obstacles;

namespace MobileNetworkFramework.Common.Geometry
{
    public class Graph<T> where T : notnull, IConnectableObstacle
    {
        #region Properties

        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        #endregion


        #region Constructors

        public Graph() {}
        
        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T,T>> edges) {
            foreach(var vertex in vertices)
                AddVertex(vertex);

            foreach(var edge in edges)
                AddEdge(edge);
        }

        #endregion


        #region Public Methods

        public void AddVertex(T vertex) {
            AdjacencyList[vertex] = new HashSet<T>();
        }

        public void AddEdge(Tuple<T,T> edge) {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2)) {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }

        public void CreateAdjacencyList(List<T> vertices)
        {
            
        }

        #endregion
    }
}

