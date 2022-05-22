// https://leetcode.com/problems/longest-increasing-path-in-a-matrix/

//Given an m x n integers matrix, return the length of the longest increasing path in matrix.

//From each cell, you can either move in four directions: left, right, up, or down.
//You may not move diagonally or move outside the boundary (i.e., wrap-around is not allowed).

//Example 1:
//Input: matrix = [[9, 9, 4],[6,6,8],[2,1,1]]
//Output: 4
//Explanation: The longest increasing path is [1, 2, 6, 9].

//Example 2:
//Input: matrix = [[3, 4, 5],[3,2,6],[2,2,1]]
//Output: 4
//Explanation: The longest increasing path is [3, 4, 5, 6]. Moving diagonally is not allowed.

//Example 3:
//Input: matrix = [[1]]
//Output: 1

//Constraints:
//m == matrix.length
//n == matrix[i].length
//1 <= m, n <= 200
//0 <= matrix[i][j] <= 2^31 - 1

Console.WriteLine("Hello, World!");



new Solution().LongestIncreasingPath(new[] { new[] { 1 } });

// [[7,7,5],[2,4,6],[8,2,0]]
new Solution().LongestIncreasingPath(new[]
{
    new[] { 7, 7, 5 },
    new[] { 2, 4, 6 },
    new[] { 8, 2, 0 }
});


public class Solution
{
    private struct Cell : IEquatable<Cell>
    {
        public int Row;
        public int Column;

        public override int GetHashCode() => (Row, Column).GetHashCode();
        public override bool Equals(object obj) => obj is Cell other && Equals(other);
        public bool Equals(Cell other) => Row == other.Row && Column == other.Column;
        public override string ToString() => $"({Row},{Column})";
    }

    private static readonly (int RowDelta, int ColDelta)[] Directions = new[]
    {
        (0, 1),
        (1, 0),
        (0, -1),
        (-1, 0)
    };

    public int LongestIncreasingPath(int[][] matrix)
    {
        if (matrix is null || matrix.Length == 0 || matrix[0] is null || matrix[0].Length == 0)
            return -1;

        var m = matrix.Length;
        var n = matrix[0].Length;

        // Unfortunately, at Leetcode time limit is exceeded for current implementation :(
        // Time: O(m * n)
        // Space: O(m * n). It is for additional data structures, also some call stack space for recursive DFS is used.

        // Represent matrix as directed graph where increasing path is set of edges.
        var (adjacencyMap, cellsWithIncomingEdges) = GetAdjacencyMapAndCellsWIthIncomingEdges(matrix);
        var startCells = GetStartCells(adjacencyMap, cellsWithIncomingEdges);

        // DP array was used after peeking into Leetcode solutions/discussions.
        // It contains a target path length (i.e. increasing path length) from cell with coordinates (i, j).
        // NOTE: actually, after peeking and introducing DP array, I realized that solution can be simplified, transformation to graph can be eliminated, etc.
        // But I saved it as is.
        var dp = GetDpArray(m, n);

        // Now we can do either DFS/BFS or implement Dijkstra algorithm to find longest path starting from start cells.
        // Note that in current graph there are no cycles, so it is easier to implement DFS/BFS.
        var result = 1;
        foreach (var cell in startCells)
        {
            result = Math.Max(result, DFS(cell) + 1);
        }

        return result;


        int DFS(Cell current)
        {
            if (!adjacencyMap.TryGetValue(current, out var adjacentCells) || adjacentCells.Count == 0)
                return 0;

            var result = -1;
            foreach (var adjCell in adjacentCells)
            {
                var adjCellPathLength = dp[adjCell.Row][adjCell.Column] > 0
                    ? dp[adjCell.Row][adjCell.Column]
                    : DFS(adjCell);
                result = Math.Max(result, adjCellPathLength + 1);
            }
            dp[current.Row][current.Column] = result;
            return result;
        }
    }

    // Time: O(m * n)
    private (Dictionary<Cell, List<Cell>> AdjacencyMap, HashSet<Cell> CellsWithIncomingEdges) GetAdjacencyMapAndCellsWIthIncomingEdges(int[][] matrix)
    {
        var map = new Dictionary<Cell, List<Cell>>();
        var cellsWithIncomingEdges = new HashSet<Cell>();

        for (var row = 0; row < matrix.Length; row++)
        {
            for (var col = 0; col < matrix[row].Length; col++)
            {
                var cell = new Cell { Row = row, Column = col };

                foreach (var (rowDelta, colDelta) in Directions)
                {
                    var adjRow = row + rowDelta;
                    var adjCol = col + colDelta;

                    if (adjRow >= matrix.Length || adjRow < 0 || adjCol >= matrix[row].Length || adjCol < 0 || matrix[row][col] >= matrix[adjRow][adjCol])
                        continue;

                    var adjCell = new Cell { Row = adjRow, Column = adjCol };
                    if (!map.TryGetValue(cell, out var list))
                    {
                        list = new List<Cell>();
                        map[cell] = list;
                    }
                    list.Add(adjCell);
                    cellsWithIncomingEdges.Add(adjCell);
                }
            }
        }

        return (map, cellsWithIncomingEdges);
    }

    // Time: O(m * n)
    private HashSet<Cell> GetStartCells(Dictionary<Cell, List<Cell>> adjacencyMap, HashSet<Cell> cellsWithIncomingEdges)
    {
        var result = new HashSet<Cell>();

        // Start cells are those which present in adjacency map as keys but not present in cells with incoming edges.
        foreach (var (cell, adjacentCells) in adjacencyMap)
        {
            if (adjacentCells.Count > 0 && !cellsWithIncomingEdges.Contains(cell))
                result.Add(cell);
        }
        
        return result;
    }

    private int[][] GetDpArray(int m, int n)
    {
        var result = new int[m][];
        for (var i = 0; i < m; i++)
        {
            result[i] = new int[n];
            Array.Fill(result[i], -1);
        }
        return result;
    }
}