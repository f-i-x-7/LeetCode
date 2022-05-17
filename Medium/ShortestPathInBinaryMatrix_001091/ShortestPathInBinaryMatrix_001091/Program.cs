// https://leetcode.com/problems/shortest-path-in-binary-matrix/

//Given an n x n binary matrix grid, return the length of the shortest clear path in the matrix. If there is no clear path, return -1.

//A clear path in a binary matrix is a path from the top-left cell (i.e., (0, 0)) to the bottom-right cell (i.e., (n - 1, n - 1)) such that:

//All the visited cells of the path are 0.
//All the adjacent cells of the path are 8-directionally connected (i.e., they are different and they share an edge or a corner).
//The length of a clear path is the number of visited cells of this path.

//Example 1:
//Input: grid = [[0, 1],[1,0]]
//Output: 2

//Example 2:
//Input: grid = [[0, 0, 0],[1,1,0],[1,1,0]]
//Output: 4

//Example 3:
//Input: grid = [[1, 0, 0],[1,1,0],[1,1,0]]
//Output: -1

//Constraints:
//n == grid.length
//n == grid[i].length
//1 <= n <= 100
//grid[i][j] is 0 or 1

Console.WriteLine("Hello, World!");


public class Solution
{
    private const int FREEWAY = 0;

    public int ShortestPathBinaryMatrix(int[][] grid)
    {
        var n = grid.Length;

        if (n < 1 || grid[0][0] != FREEWAY || grid[n - 1][n - 1] != FREEWAY)
            return -1;

        var currentLevelCells = new List<(int Row, int Column)>() { (Row: 0, Column: 0) };
        var visitedCells = new HashSet<(int Row, int Column)>() { (Row: 0, Column: 0) };

        // Time & space: O(N^2)? But LeetCode members in Discussions treat it as O(N)

        return BFS(grid, currentLevelCells, visitedCells, n, currentDepth: 1);
    }

    private int BFS(int[][] grid, List<(int Row, int Column)> previousLevelCells, HashSet<(int Row, int Column)> visitedCells, int n, int currentDepth)
    {
        if (visitedCells.Contains((Row: n - 1, Column: n - 1)))
            return currentDepth;

        var currentLevelCells = new List<(int Row, int Column)>();
        var cellsToTry = new List<(int Row, int Column)>(8);

        foreach (var (row, column) in previousLevelCells)
        {
            // We should not only increase coordinates but sometimes even decrease both of them, or increase one and decrease another, e.g.
            //[
            //[0,1,1,0,0,0],
            //[0,1,0,1,1,0],
            //[0,1,1,0,1,0],
            //[0,0,0,1,1,0],
            //[1,1,1,1,1,0],
            //[1,1,1,1,1,0]
            //]
            var canIncreaseRow = row < n - 1;
            var canIncreaseColumn = column < n - 1;
            var canDecreaseRow = row > 0;
            var canDecreaseColumn = column > 0;

            cellsToTry.Clear();

            if (canIncreaseRow && canIncreaseColumn)
                cellsToTry.Add((Row: row + 1, Column: column + 1));
            if (canDecreaseRow && canDecreaseColumn)
                cellsToTry.Add((Row: row - 1, Column: column - 1));

            if (canIncreaseRow && canDecreaseColumn)
                cellsToTry.Add((Row: row + 1, Column: column - 1));
            if (canDecreaseRow && canIncreaseColumn)
                cellsToTry.Add((Row: row - 1, Column: column + 1));

            if (canIncreaseRow)
                cellsToTry.Add((Row: row + 1, Column: column));
            if (canIncreaseColumn)
                cellsToTry.Add((Row: row, Column: column + 1));
            
            if (canDecreaseRow)
                cellsToTry.Add((Row: row - 1, Column: column));
            if (canDecreaseColumn)
                cellsToTry.Add((Row: row, Column: column - 1));

            foreach (var (rowToTry, columnToTry) in cellsToTry)
            {
                if (grid[rowToTry][columnToTry] == FREEWAY && visitedCells.Add((Row: rowToTry, Column: columnToTry)))
                    currentLevelCells.Add((Row: rowToTry, Column: columnToTry));
            }
        }

        if (currentLevelCells.Count == 0)
            return -1; // Could not advance during next iteration, so there is no target path

        return BFS(grid, currentLevelCells, visitedCells, n, currentDepth + 1);
    }
}