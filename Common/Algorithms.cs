namespace MobileNetworkFramework.Common
{
    public static class Algorithms {
        public static HashSet<T> DepthFirstSearch<T>(Dictionary<T, HashSet<T>> adjacencyList, T start) where T : notnull
        {
            var visited = new HashSet<T>();

            if (!adjacencyList.ContainsKey(start)) return visited;
                
            var stack = new Stack<T>();
            stack.Push(start);

            while (stack.Count > 0) {
                var vertex = stack.Pop();
                if (visited.Contains(vertex)) continue;
                visited.Add(vertex);
                foreach(var neighbor in adjacencyList[vertex]) if (!visited.Contains(neighbor)) stack.Push(neighbor);
            }

            return visited;
        }
    }
}
