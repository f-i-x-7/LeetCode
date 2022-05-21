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
        if (obstacleGrid == null || obstacleGrid.Length == 0 || obstacleGrid[0] is null || obstacleGrid[0].Length == 0)
            return 0;

        var m = obstacleGrid.Length;
        var n = obstacleGrid[0].Length;

        if (obstacleGrid[0][0] != SPACE || obstacleGrid[m - 1][n - 1] != SPACE)
            return 0;

        // BFS approach.
        // Time: O(m*n)?
        // Space: O(m*n)?
        // Out of memory :(

        // DFS - Time Limit Exceeded :(

        var pathsCount = 0;
        DFS(0, 0, ref pathsCount);
        //BFS(new List<(int Row, int Col)> { (0, 0) }, ref pathsCount);
        return pathsCount;



        void BFS(List<(int Row, int Col)> currentCells, ref int pathsCount)
        {
            var nextCells = new List<(int Row, int Col)>();
            foreach (var cell in currentCells)
            {
                if (cell == (Row: m - 1, Col: n - 1))
                {
                    pathsCount++;
                    continue;
                }

                foreach (var direction in Directions)
                {
                    var newRow = cell.Row + direction[0];
                    var newCol = cell.Col + direction[1];

                    if (newRow < 0 || newCol < 0 || newRow >= m || newCol >= n || obstacleGrid[newRow][newCol] != SPACE)
                        continue;

                    nextCells.Add((Row: newRow, Col: newCol));
                }
            }

            if (nextCells.Count == 0)
                return;

            BFS(nextCells, ref pathsCount);
        }

        void DFS(int row, int col, ref int pathsCount)
        {
            if (row == m - 1 && col == n - 1)
            {
                pathsCount++;
                return;
            }

            foreach (var direction in Directions)
            {
                var newRow = row + direction[0];
                var newCol = col + direction[1];

                if (newRow < 0 || newCol < 0 || newRow >= m || newCol >= n || obstacleGrid[newRow][newCol] != SPACE)
                    continue;

                DFS(newRow, newCol, ref pathsCount);
            }
        }
    }
}
