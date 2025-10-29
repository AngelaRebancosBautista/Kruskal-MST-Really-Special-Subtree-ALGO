using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Result
{
    /*
     * Complete the 'kruskals' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts WEIGHTED_INTEGER_GRAPH g as parameter.
     */

    public static int kruskals(int gNodes, List<int> gFrom, List<int> gTo, List<int> gWeight)
    {
        // Combine all edges into a single list of tuples
        List<(int from, int to, int weight)> edges = new List<(int, int, int)>();
        for (int i = 0; i < gWeight.Count; i++)
        {
            edges.Add((gFrom[i], gTo[i], gWeight[i]));
        }

        // Sort edges by weight ascending
        edges.Sort((a, b) => a.weight.CompareTo(b.weight));

        // Initialize DSU structures
        int[] parent = new int[gNodes + 1];
        int[] rank = new int[gNodes + 1];
        for (int i = 1; i <= gNodes; i++)
        {
            parent[i] = i;
            rank[i] = 0;
        }

        int find(int x)
        {
            if (parent[x] != x)
                parent[x] = find(parent[x]); // path compression
            return parent[x];
        }

        void union(int x, int y)
        {
            int rootX = find(x);
            int rootY = find(y);

            if (rootX == rootY) return;

            // union by rank
            if (rank[rootX] < rank[rootY])
                parent[rootX] = rootY;
            else if (rank[rootX] > rank[rootY])
                parent[rootY] = rootX;
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }
        }

        int mstCost = 0;
        foreach (var edge in edges)
        {
            int u = edge.from;
            int v = edge.to;
            int w = edge.weight;

            if (find(u) != find(v))
            {
                union(u, v);
                mstCost += w;
            }
        }

        return mstCost;
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        string[] gNodesEdges = Console.ReadLine().TrimEnd().Split(' ');

        int gNodes = Convert.ToInt32(gNodesEdges[0]);
        int gEdges = Convert.ToInt32(gNodesEdges[1]);

        List<int> gFrom = new List<int>();
        List<int> gTo = new List<int>();
        List<int> gWeight = new List<int>();

        for (int i = 0; i < gEdges; i++)
        {
            string[] gFromToWeight = Console.ReadLine().TrimEnd().Split(' ');

            gFrom.Add(Convert.ToInt32(gFromToWeight[0]));
            gTo.Add(Convert.ToInt32(gFromToWeight[1]));
            gWeight.Add(Convert.ToInt32(gFromToWeight[2]));
        }

        int res = Result.kruskals(gNodes, gFrom, gTo, gWeight);

        textWriter.WriteLine(res);
        textWriter.Flush();
        textWriter.Close();
    }
}
