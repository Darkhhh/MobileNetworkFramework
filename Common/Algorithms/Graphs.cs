using MobileNetworkFramework.Common.Geometry;

namespace MobileNetworkFramework.Common.Algorithms;

public static class Graphs
{
    public static void GetAdjacencyMatrix(IConnectable[] items, bool[,] adjacencyMatrix, bool parallel = true)
    {
        if (parallel)
        {
            Parallel.For(0, items.Length - 1, i =>
            {
                for (var j = i + 1; j < items.Length; j++)
                {
                    if (items[i].Connected(items[j])) adjacencyMatrix[i, j] = true;
                    if (items[j].Connected(items[i])) adjacencyMatrix[j, i] = true;
                }
            });
        }
        else
        {
            for (var i = 0; i < items.Length - 1; i++)
            {
                for (var j = i + 1; j < items.Length; j++)
                {
                    if (items[i].Connected(items[j])) adjacencyMatrix[i, j] = true;
                    if (items[j].Connected(items[i])) adjacencyMatrix[j, i] = true;
                }
            }
        }
    }
    
    
    public static HashSet<T> DepthFirstSearch<T>(Dictionary<T, HashSet<T>> adjacencyList, T start) where T : notnull
    {
        var visited = new HashSet<T>();

        if (!adjacencyList.ContainsKey(start)) return visited;
                
        var stack = new Stack<T>();
        stack.Push(start);

        while (stack.Count > 0) 
        {
            var vertex = stack.Pop();
            if (visited.Contains(vertex)) continue;
            visited.Add(vertex);
            foreach(var neighbor in adjacencyList[vertex]) if (!visited.Contains(neighbor)) stack.Push(neighbor);
        }

        return visited;
    }
    
    #region BFS
    
    // Source: https://www.koderdojo.com/blog/breadth-first-search-and-shortest-path-in-csharp-and-net-core
    
    public static HashSet<T> BreadthFirstSearch<T>(Graph<T> graph, T start) {
        var visited = new HashSet<T>();

        if (!graph.AdjacencyList.ContainsKey(start)) return visited;
                
        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0) 
        {
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex)) continue;

            visited.Add(vertex);

            foreach(var neighbor in graph.AdjacencyList[vertex])
                if (!visited.Contains(neighbor)) queue.Enqueue(neighbor);
        }

        return visited;
    }

    /// <summary>
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="start"></param>
    /// <param name="preVisit">Allows to pass a function that gets called each time a vertex is visited</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static HashSet<T> BreadthFirstSearch<T>(Graph<T> graph, T start, Action<T> preVisit = null) {
        var visited = new HashSet<T>();

        if (!graph.AdjacencyList.ContainsKey(start))
            return visited;
        
        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0) {
            var vertex = queue.Dequeue();

            if (visited.Contains(vertex))
                continue;

            if (preVisit != null)
                preVisit(vertex);

            visited.Add(vertex);

            foreach(var neighbor in graph.AdjacencyList[vertex])
                if (!visited.Contains(neighbor))
                    queue.Enqueue(neighbor);
        }

        return visited;
    }
    
    public static Func<T, IEnumerable<T>> ShortestPathFunction<T>(Graph<T> graph, T start) where T : notnull
    {
        var previous = new Dictionary<T, T>();
        var queue = new Queue<T>();
        queue.Enqueue(start);

        while (queue.Count > 0) 
        {
            var vertex = queue.Dequeue();
            foreach(var neighbor in graph.AdjacencyList[vertex]) 
            {
                if (previous.ContainsKey(neighbor)) continue;
            
                previous[neighbor] = vertex;
                queue.Enqueue(neighbor);
            }
        }

        IEnumerable<T> ShortestPath(T v)
        {
            var path = new List<T> { };

            var current = v;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }

        return ShortestPath;
    }

    #endregion
    
}