// https://leetcode.com/problems/network-delay-time/

//You are given a network of n nodes, labeled from 1 to n. You are also given times, a list of travel times as directed edges times[i] = (ui, vi, wi),
//where ui is the source node, vi is the target node, and wi is the time it takes for a signal to travel from source to target.

//We will send a signal from a given node k. Return the time it takes for all the n nodes to receive the signal. If it is impossible for all the n nodes to receive the signal, return -1.

//Example 1:
//Input: times = [[2, 1, 1],[2,3,1],[3,4,1]], n = 4, k = 2
//Output: 2

//Example 2:
//Input: times = [[1, 2, 1]], n = 2, k = 1
//Output: 1

//Example 3:
//Input: times = [[1, 2, 1]], n = 2, k = 2
//Output: -1

//Constraints:
//1 <= k <= n <= 100
//1 <= times.length <= 6000
//times[i].length == 3
//1 <= ui, vi <= n
//ui != vi
//0 <= wi <= 100
//All the pairs (ui, vi) are unique. (i.e., no multiple edges.)


new Solution().NetworkDelayTime(new[] { new[] { 1, 2, 1 }, new[] { 2, 1, 3 } }, n: 2, k: 2);


public class Solution
{
    public int NetworkDelayTime(int[][] times, int n, int k)
    {
        // Dijksta algorithm to find paths from k-th vertice to all other vertices
        // Time: O(N^2)
        // Space: O(N^2)
        var pathCosts = BuildPathsCosts(); // O(N^2) time, O(N^2) space


        // O(N) time, O(N) space
        var verticesCost = new int[n];
        for (var i = 0; i < n; i++)
        {
            verticesCost[i] = i == k - 1
                ? 0
                : int.MaxValue;
        }

        var verticesVisitedFlag = new bool[n];


        var currentVerticeIndex = k - 1;
        
        // O(N^2) time, O(1) space
        while (true)
        {
            var currentVerticeCost = verticesCost[currentVerticeIndex];

            for (var toVerticeIndex = 0; toVerticeIndex < n; toVerticeIndex++)
            {
                var pathCost = pathCosts[currentVerticeIndex][toVerticeIndex];
                if (pathCost < 0)
                    continue; // no edge between vertices

                var toVerticeNewCost = currentVerticeCost + pathCost;
                if (verticesCost[toVerticeIndex] > toVerticeNewCost)
                    verticesCost[toVerticeIndex] = toVerticeNewCost;
            }

            // Mark current vertice as visited
            verticesVisitedFlag[currentVerticeIndex] = true;

            // Find next vertice, it should have min cost
            var minCost = int.MaxValue;
            for (var i = 0; i < n; i++)
            {
                if (verticesVisitedFlag[i])
                    continue;

                if (verticesCost[i] < minCost)
                {
                    minCost = verticesCost[i];
                    currentVerticeIndex = i;
                }
            }

            if (minCost == int.MaxValue)
                break; // either all vertices are visited or graph is not connected
        }

        // Now it's time to return result
        var max = verticesCost.Max();
        if (max == int.MaxValue)
            return -1; // it is impossible for signal to visit all edges

        return max;



        int[][] BuildPathsCosts()
        {
            // O(N^2) time
            var result = new int[n][];
            for (var i = 0; i < n; i++)
            {
                result[i] = new int[n];
                for (var j = 0; j < n; j++)
                {
                    result[i][j] = -1;
                }
            }

            // O(N^2) time? each vertice can be connected with each vertice technically
            foreach (var arr in times)
            {
                var fromVerticeIndex = arr[0] - 1;
                var toVerticeIndex = arr[1] - 1;
                var pathLength = arr[2];

                result[fromVerticeIndex][toVerticeIndex] = pathLength;
            }

            return result;
        }
    }
}