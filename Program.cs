using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp6
{
    internal class Program
    {

        public static string SearchCycle(int n, List<int> adjacencyList)
        {
            var root = adjacencyList.FindIndex(v => v != 0) + 1;
            var visitedVertexs = new List<int>();
            var pendingVisits = new Queue<int>();
            pendingVisits.Enqueue(root);
            var from = Enumerable.Range(0, n + 1).ToList();
            while (pendingVisits.Count != 0)
            {
                var vertex = pendingVisits.Dequeue();
                var nextPosition = adjacencyList.Count;
                visitedVertexs.Add(vertex);
                if (vertex != n) nextPosition = adjacencyList[vertex];

                for (var i = adjacencyList[vertex - 1]; i < nextPosition; i++)
                {
                    if (visitedVertexs.Contains(adjacencyList[i]))
                    {
                        var visitedVertex = visitedVertexs.Find(v => v == adjacencyList[i]);
                        if (visitedVertex == @from[vertex]) continue;
                        {
                            return "N :" + GetCycle(from, vertex, visitedVertex);
                        }
                    }

                    @from[adjacencyList[i]] = vertex;
                    pendingVisits.Enqueue(adjacencyList[i]);
                }
            }

            if (visitedVertexs.Count == n) return "A";
            var remainingVertex = GetRemainingVertex(n, adjacencyList, visitedVertexs);
            return SearchCycle(n, remainingVertex);
        }

        private static string GetCycle(List<int> allPath, int fromVertex, int toVertex)
        {
            var path = new List<int>() {fromVertex, toVertex};
            while (fromVertex != toVertex)
            {
                fromVertex = allPath[fromVertex];
                path.Add(fromVertex);
                if (fromVertex == toVertex)
                    break;
                toVertex = allPath[toVertex];
                path.Add(toVertex);
            }

            path.Sort();
            var pathLikeLine = "";
            path.Distinct().ToList().ForEach(v => pathLikeLine += v + " ");
            return pathLikeLine;
        }

        private static List<int> GetRemainingVertex(int n, List<int> adjacencyList, List<int> visited)
        {
            var newAdjacencyList = Enumerable.Range(1, n).Select(i => visited.Contains(i) ? 0 : i).ToList();
            for (var i = 0; i < newAdjacencyList.Count; i++)
                if (newAdjacencyList[i] != 0)
                    newAdjacencyList[i] = adjacencyList[i];
            newAdjacencyList.AddRange(adjacencyList.GetRange(n, adjacencyList.Count - n));
            return newAdjacencyList;
        }

        private static void Main(string[] args)
        {
            using (var file = new StreamReader("in.txt"))
            {
                var n = int.Parse(file.ReadLine());
                var vertexs = Enumerable.Range(0, n).ToList();
                for (var i = 0; i < n; i++)
                {
                    var adjacent_vertices = file.ReadLine().Split(' ')
                        .Where(vertex => !string.IsNullOrWhiteSpace(vertex)).Select(int.Parse).Where(v => v != 0)
                        .ToList();
                    vertexs[i] = vertexs.Count;
                    vertexs.AddRange(adjacent_vertices);
                }

                using (var streamWriter = new StreamWriter("out.txt"))
                {
                    streamWriter.Write(SearchCycle(n, vertexs));
                }
            }
        }
    }
}
