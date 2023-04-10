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

        while (stack.Count > 0) {
            var vertex = stack.Pop();
            if (visited.Contains(vertex)) continue;
            visited.Add(vertex);
            foreach(var neighbor in adjacencyList[vertex]) if (!visited.Contains(neighbor)) stack.Push(neighbor);
        }

        return visited;
    }
}