// https://leetcode.com/problems/shift-2d-grid/

//Given a 2D grid of size m x n and an integer k. You need to shift the grid k times.

//In one shift operation:

//Element at grid[i][j] moves to grid[i] [j + 1].
//Element at grid[i][n - 1] moves to grid[i + 1] [0].
//Element at grid[m - 1][n - 1] moves to grid[0] [0].
//Return the 2D grid after applying shift operation k times.

//Example 1:
//Input: grid = [[1,2,3],[4,5,6],[7,8,9]], k = 1
//Output:[[9,1,2],[3,4,5],[6,7,8]]

//Example 2:
//Input: grid = [[3,8,1,9],[19,7,2,5],[4,6,11,10],[12,0,21,13]], k = 4
//Output:[[12,0,21,13],[3,8,1,9],[19,7,2,5],[4,6,11,10]]

//Example 3:
//Input: grid = [[1, 2, 3],[4,5,6],[7,8,9]], k = 9
//Output:[[1,2,3],[4,5,6],[7,8,9]]

//Constraints:
//m == grid.length
//n == grid[i].length
//1 <= m <= 50
//1 <= n <= 50
//- 1000 <= grid[i][j] <= 1000
//0 <= k <= 100



Test(new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } },
    k: 1,
    expectedResult: new[] { new[] { 9, 1, 2 }, new[] { 3, 4, 5 }, new[] { 6, 7, 8 } });

Test(new[] { new[] { 3,8,1,9 }, new[] { 19,7,2,5 }, new[] { 4,6,11,10 }, new[] { 12,0,21,13 } },
    k: 4,
    expectedResult: new[] { new[] { 12,0,21,13 }, new[] { 3,8,1,9 }, new[] { 19,7,2,5 }, new[] { 4,6,11,10 } });

Test(new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } },
    k: 9,
    expectedResult: new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } });





void Test(int[][] grid, int k, int[][] expectedResult)
{
    var gridStr = ToStr(grid);
    var actualResult = new Solution().ShiftGrid(grid, k);

    var listExpected = expectedResult.SelectMany(x => x).ToList();
    var listActual = actualResult.SelectMany(x => x).ToList();

    if (!Enumerable.SequenceEqual(listExpected, listActual))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {gridStr}; k={k}; ExpectedResult={ToStr(expectedResult)}; ActualResult={ToStr(actualResult)}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {gridStr}; k={k}; Result={ToStr(expectedResult)}");
    }



    string ToStr(IList<IList<int>> arr)
    {
        return '[' + string.Join(',', arr.Select(subArr => '[' + string.Join(',', subArr) + ']')) + ']';
    }
}


public class Solution
{
    public IList<IList<int>> ShiftGrid(int[][] grid, int k)
    {
        // Invalid cases
        if (grid is null || grid.Length == 0 || k < 0)
            return grid;

        // NOTE: Leetcode does not require to return new grid. As it is allowed to do replacements in place, some optimizations for corner cases are performed.
        // During actual interview it is better to negotiate this aspect of behavior in advance.
        // And these optimizations initially were not implemented correctly by me, so I've received only a lot of errors.

        // O(1) optimization for corner case with no shift
        if (k == 0)
            return grid;

        var m = grid.Length; // number of rows
        var n = grid[0].Length; // number of columns

        // Invalid case
        if (n == 0)
            return grid;

        var totalCount = m * n;

        // O(1) optimization for another corner case with no shift
        if (k % totalCount == 0)
            return grid;

        // Corner case - entire rows need to be shifted
        // O(m) time
        // As shift occurs, it is far easier to create new array (this will produce more readable code).
        // But again - as it is not strictly required to return new object, rows of initial jagged array are reused (thus opening the door for optimization).
        if (k % n == 0)
        {
            var shift = (k / n) % m;
            System.Diagnostics.Debug.Assert(shift > 0 && shift < m);

            var result = new int[m][];

            for (var i = 0; i < grid.Length; i++)
            {
                var newIndex = (i + shift) % m;
                result[newIndex] = grid[i];
            }

            return result;
        }

        // O(n*m) = O(N) implementation for regular case
        {
            var shift = k % totalCount;
            var result = new int[m][];
            for (var i = 0; i < grid.Length; i++)
            {
                result[i] = new int[n];
                for (var j = 0; j < grid[i].Length; j++)
                {
                    var targetIndexInOneDimensionalArray = i * n + j;
                    var sourceIndexInOneDimensionalArray = targetIndexInOneDimensionalArray - shift;
                    if (sourceIndexInOneDimensionalArray < 0)
                        sourceIndexInOneDimensionalArray += totalCount;

                    var sourceRowIndex = sourceIndexInOneDimensionalArray / n;
                    var sourceColumnIndex = sourceIndexInOneDimensionalArray % n;

                    result[i][j] = grid[sourceRowIndex][sourceColumnIndex];
                }
            }

            return result;
        }
    }
}