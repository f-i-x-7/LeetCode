// https://leetcode.com/problems/unique-paths-ii/

//You are given an m x n integer array grid. There is a robot initially located at the top-left corner (i.e., grid[0][0]).
//The robot tries to move to the bottom-right corner (i.e., grid[m - 1][n-1]). The robot can only move either down or right at any point in time.

//An obstacle and space are marked as 1 or 0 respectively in grid. A path that the robot takes cannot include any square that is an obstacle.

//Return the number of possible unique paths that the robot can take to reach the bottom-right corner.

//The testcases are generated so that the answer will be less than or equal to 2 * 109.

//Example 1:
//Input: obstacleGrid = [[0, 0, 0],[0,1,0],[0,0,0]]
//Output: 2
//Explanation: There is one obstacle in the middle of the 3x3 grid above.
//There are two ways to reach the bottom-right corner:
//1.Right->Right->Down->Down
//2.Down->Down->Right->Right

//Example 2:
//Input: obstacleGrid = [[0, 1],[0,0]]
//Output: 1

//Constraints:
//m == obstacleGrid.length
//n == obstacleGrid[i].length
//1 <= m, n <= 100
//obstacleGrid[i][j] is 0 or 1.

Console.WriteLine("Hello, World!");



public class Solution
{
    private const int SPACE = 0;
    private static readonly int[][] Directions = new[] { new[] { 0, 1 }, new[] { 1, 0 } };

    public int UniquePathsWithObstacles(int[][] obstacleGrid)
    {
        return UniquePathsWithObstacles_DFS(obstacleGrid);
    }

    private int UniquePathsWithObstacles_DFS(int[][] obstacleGrid)
    {
        if (obstacleGrid == null || obstacleGrid.Length == 0 || obstacleGrid[0] is null || obstacleGrid[0].Length == 0)
            return 0;

        var m = obstacleGrid.Length;
        var n = obstacleGrid[0].Length;

        if (obstacleGrid[0][0] != SPACE || obstacleGrid[m - 1][n - 1] != SPACE)
            return 0;

        // DFS approach with memoization.
        // Time: O(m*n) - each cell will be visited only once
        // Space: O(m*n)
        // Implemented after peeking into Discussions section.

        var numberOfPathsFromCellToFinish = new Dictionary<(int Row, int Col), int>();

        return DFS(0, 0);

        // Returns number of ways from cell with coordinates (row, col) to finish cell.
        int DFS(int row, int col)
        {
            if (row < 0 || col < 0 || row >= m || col >= n)
                return 0; // out of grid

            if (row == m - 1 && col == n - 1)
                return 1; // finish cell

            if (obstacleGrid[row][col] != SPACE)
                return 0; // obstacle

            if (!numberOfPathsFromCellToFinish.TryGetValue((Row: row, Col: col), out var numPaths))
            {
                foreach (var direction in Directions)
                {
                    var newRow = row + direction[0];
                    var newCol = col + direction[1];

                    numPaths += DFS(newRow, newCol);
                }

                numberOfPathsFromCellToFinish.Add((Row: row, Col: col), numPaths);
            }

            return numPaths;
        }
    }

    private int UniquePathsWithObstacles_DP(int[][] obstacleGrid)
    {
        if (obstacleGrid == null || obstacleGrid.Length == 0 || obstacleGrid[0] is null || obstacleGrid[0].Length == 0)
            return 0;

        var m = obstacleGrid.Length;
        var n = obstacleGrid[0].Length;

        if (obstacleGrid[0][0] != SPACE || obstacleGrid[m - 1][n - 1] != SPACE)
            return 0;

        // DP approach, taken from solution section from Leetcode.
        // Time: O(m*n) - each cell will be visited only once
        // Space: O(m*n) - can be reduced to O(1) if current grid can be modified.

        var dp = new int[m][]; // contains number of ways to arrive from starting cell to (i,j) cell
        for (var i = 0; i < m; i++)
        {
            dp[i] = new int[n];
        }

        // Iterate over first row and first column, fill dp grid.
        // This is because we can arrive into cells of first row only if robot always moves left (for first column - always moves down).
        for (var j = 0; j < n; j++)
        {
            if (obstacleGrid[0][j] == SPACE)
                dp[0][j] = 1;
            else
                break;
        }

        for (var i = 0; i < m; i++)
        {
            if (obstacleGrid[i][0] == SPACE)
                dp[i][0] = 1;
            else
                break;
        }

        // Then iterate over non-filled cells by rows (e.g. starting from (1,1) cell), fill dp grid as sum of dp cells from above and left.
        for (var i = 1; i < m; i++)
        {
            for (var j = 1; j < n; j++)
            {
                if (obstacleGrid[i][j] == SPACE)
                    dp[i][j] = dp[i - 1][j] + dp[i][j - 1];
            }
        }

        return dp[m - 1][n - 1];
    }
}
