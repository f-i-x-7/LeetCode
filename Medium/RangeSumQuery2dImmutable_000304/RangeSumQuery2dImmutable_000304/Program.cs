// https://leetcode.com/problems/range-sum-query-2d-immutable/

//Given a 2D matrix matrix, handle multiple queries of the following type:

//Calculate the sum of the elements of matrix inside the rectangle defined by its upper left corner (row1, col1) and lower right corner (row2, col2).
//Implement the NumMatrix class:

//NumMatrix(int[][] matrix) Initializes the object with the integer matrix matrix.
//int sumRegion(int row1, int col1, int row2, int col2) Returns the sum of the elements of matrix inside the rectangle defined by its upper left corner (row1, col1) and lower right corner (row2, col2).

//Example 1:
//Input
//["NumMatrix", "sumRegion", "sumRegion", "sumRegion"]
//[[[[3, 0, 1, 4, 2], [5, 6, 3, 2, 1], [1, 2, 0, 1, 5], [4, 1, 0, 1, 7], [1, 0, 3, 0, 5]]], [2, 1, 4, 3], [1, 1, 2, 2], [1, 2, 2, 4]]
//Output
//[null, 8, 11, 12]
//Explanation
//NumMatrix numMatrix = new NumMatrix([[3, 0, 1, 4, 2], [5, 6, 3, 2, 1], [1, 2, 0, 1, 5], [4, 1, 0, 1, 7], [1, 0, 3, 0, 5]]);
//numMatrix.sumRegion(2, 1, 4, 3); // return 8 (i.e sum of the red rectangle)
//numMatrix.sumRegion(1, 1, 2, 2); // return 11 (i.e sum of the green rectangle)
//numMatrix.sumRegion(1, 2, 2, 4); // return 12 (i.e sum of the blue rectangle)

//Constraints:
//m == matrix.length
//n == matrix[i].length
//1 <= m, n <= 200
//-10^5 <= matrix[i][j] <= 10^5
//0 <= row1 <= row2 < m
//0 <= col1 <= col2 < n
//At most 10^4 calls will be made to sumRegion.

Console.WriteLine("Hello, World!");


public class NumMatrix_Naive
{
    private readonly int[][] matrix;
    private readonly int rowsCount;
    private readonly int colsCount;

    public NumMatrix_Naive(int[][] matrix)
    {
        this.matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));

        rowsCount = matrix.Length;
        if (rowsCount == 0)
            throw new ArgumentException("Zero rows count is not allowed.", nameof(matrix));

        colsCount = matrix[0].Length;
        if (colsCount == 0)
            throw new ArgumentException("Zero columns count is not allowed.", nameof(matrix));
    }

    public int SumRegion(int row1, int col1, int row2, int col2)
    {
        if (row1 > row2 || col1 > col2 || row1 < 0 || col1 < 0 || row2 > rowsCount - 1 || col2 > colsCount - 1)
            return 0;

        // Time: O(m * n)
        // Space: O(1)
        // Time Limit exceeded

        var sum = 0;

        for (var r = row1; r <= row2; r++)
        {
            for (var c = col1; c <= col2; c++)
            {
                sum += matrix[r][c];
            }
        }

        return sum;
    }
}

public class NumMatrix_CacheEveryQuery
{
    private struct Point : IEquatable<Point>
    {
        public int Row;
        public int Col;

        public override int GetHashCode() => (Row, Col).GetHashCode();
        public override bool Equals(object obj) => obj is Point other && Equals(other);
        public bool Equals(Point other) => Row == other.Row && Col == other.Col;
    }

    private readonly Dictionary<(Point From, Point To), int> cacheEveryQuery = new();
    private readonly int[][] matrix;
    private readonly int rowsCount;
    private readonly int colsCount;

    public NumMatrix_CacheEveryQuery(int[][] matrix)
    {
        this.matrix = matrix ?? throw new ArgumentNullException(nameof(matrix));

        rowsCount = matrix.Length;
        if (rowsCount == 0)
            throw new ArgumentException("Zero rows count is not allowed.", nameof(matrix));

        colsCount = matrix[0].Length;
        if (colsCount == 0)
            throw new ArgumentException("Zero columns count is not allowed.", nameof(matrix));
    }

    public int SumRegion(int row1, int col1, int row2, int col2)
    {
        if (row1 > row2 || col1 > col2 || row1 < 0 || col1 < 0 || row2 > rowsCount - 1 || col2 > colsCount - 1)
            return 0;

        // Time: O(m * n) for single call if not cached, O(1) otherwise
        // Space: O(m^2 * n^2)
        // Memory Limit not exceeded! But, according to official Solution, it was meant to precalculate all possible sums, that's while memory limit should be exceeded

        var from = new Point { Row = row1, Col = col1 };
        var to = new Point { Row = row2, Col = col2 };

        if (cacheEveryQuery.TryGetValue((from, to), out var sum))
            return sum;

        sum = 0;

        for (var r = row1; r <= row2; r++)
        {
            for (var c = col1; c <= col2; c++)
            {
                sum += matrix[r][c];
            }
        }

        cacheEveryQuery.Add((from, to), sum);

        return sum;
    }
}

// Implemented after peeking into Leetcode solution
public class NumMatrix_PrecomputeRowSums
{
    /// <summary>
    /// Each row contains sums of items in this row: j-th item contains sum of row items from 0-th till (j-1)-th.
    /// </summary>
    private readonly int[][] dp;
    private readonly int rowsCount;
    private readonly int colsCount;

    // Precomputation time: O(m * n)
    // Query time: O(m)
    // Additional space: O(m * n)
    public NumMatrix_PrecomputeRowSums(int[][] matrix)
    {
        _ = matrix ?? throw new ArgumentNullException(nameof(matrix));

        rowsCount = matrix.Length;
        if (rowsCount == 0)
            throw new ArgumentException("Zero rows count is not allowed.", nameof(matrix));

        colsCount = matrix[0].Length;
        if (colsCount == 0)
            throw new ArgumentException("Zero columns count is not allowed.", nameof(matrix));

        dp = new int[rowsCount][];
        for (var r = 0; r < rowsCount; r++)
        {
            dp[r] = new int[colsCount + 1];
            for (var c = 0; c < colsCount; c++)
            {
                dp[r][c + 1] = dp[r][c] + matrix[r][c];
            }
        }
    }

    public int SumRegion(int row1, int col1, int row2, int col2)
    {
        if (row1 > row2 || col1 > col2 || row1 < 0 || col1 < 0 || row2 > rowsCount - 1 || col2 > colsCount - 1)
            return 0;

        var sum = 0;

        for (var r = row1; r <= row2; r++)
        {
            sum += dp[r][col2 + 1] - dp[r][col1];
        }

        return sum;
    }
}

// Implemented after peeking into Leetcode solution
public class NumMatrix_PrecomputeSmart
{
    /// <summary>
    /// Each item with index (r, c) contains sums of items in square range from (0,0) till (r-1,c-1) inclusively in initial matrix.
    /// This means that zero row and column are not filled at all.
    /// </summary>
    private readonly int[][] dp;
    private readonly int rowsCount;
    private readonly int colsCount;

    // Precomputation time: O(m * n)
    // Query time: O(m)
    // Additional space: O(m * n)
    public NumMatrix_PrecomputeSmart(int[][] matrix)
    {
        _ = matrix ?? throw new ArgumentNullException(nameof(matrix));

        rowsCount = matrix.Length;
        if (rowsCount == 0)
            throw new ArgumentException("Zero rows count is not allowed.", nameof(matrix));

        colsCount = matrix[0].Length;
        if (colsCount == 0)
            throw new ArgumentException("Zero columns count is not allowed.", nameof(matrix));

        // Sample calculation is below.
        // Matrix:
        // 1   2   3
        // 4   5   6
        // 7   8   9

        // Dp:
        // 0   0   0   0
        // 0   1   3   6
        // 0   5   12  21
        // 0   12  27  45

        // dp[3][3] = SUM(matrix[0][0] : matrix[2][2]) = SUM(0_0 : 2_2) =
        //      SUM(0_0 : 1_2) + SUM(0_0 : 2_1) - SUM(0_0 : 1_1) + matrix[2][2] = // subtract SUM(0_0 : 1_1) because it is considered 2 times in adding sums
        //      dp[2][3] + dp[3][2] - dp[2][2] + matrix[2][2] =
        //      21 + 27 - 12 + 9 =
        //      45

        dp = new int[rowsCount + 1][];
        dp[0] = new int[colsCount + 1];
        for (var r = 0; r < rowsCount; r++)
        {
            dp[r + 1] = new int[colsCount + 1];

            for (var c = 0; c < colsCount; c++)
            {
                dp[r + 1][c + 1] = dp[r + 1][c] + dp[r][c + 1] - dp[r][c] + matrix[r][c];
            }
        }
    }

    public int SumRegion(int row1, int col1, int row2, int col2)
    {
        if (row1 > row2 || col1 > col2 || row1 < 0 || col1 < 0 || row2 > rowsCount - 1 || col2 > colsCount - 1)
            return 0;

        // We need to calculate SUM(R1_C1 : R2_C2).
        // Based on precomputed data, we can calculate it in constant time as:

        // SUM(R1_C1 : R2_C2) =
        // SUM(0_0 : R2_C2) - SUM(0_0 : R2_C1-1) - SUM(0_0 : R1-1_C2) + SUM(0_0 : R1-1_C1-1) = // add SUM(0_0 : R1-1_C1-1) because it is subtracted 2 times when previous sums are subtracted
        // dp[r2 + 1][c2 + 1] - dp[r2 + 1][c1] - dp[r1][c2 + 1] + dp[r1][c1]

        return dp[row2 + 1][col2 + 1] - dp[row2 + 1][col1] - dp[row1][col2 + 1] + dp[row1][col1];
    }
}


/**
 * Your NumMatrix object will be instantiated and called as such:
 * NumMatrix obj = new NumMatrix(matrix);
 * int param_1 = obj.SumRegion(row1,col1,row2,col2);
 */