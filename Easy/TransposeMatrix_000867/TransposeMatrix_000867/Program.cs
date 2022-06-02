// https://leetcode.com/problems/transpose-matrix/

//Given a 2D integer array matrix, return the transpose of matrix.

//The transpose of a matrix is the matrix flipped over its main diagonal, switching the matrix's row and column indices.

//Example 1:
//Input: matrix = [[1, 2, 3],[4,5,6],[7,8,9]]
//Output: [[1,4,7],[2,5,8],[3,6,9]]

//Example 2:
//Input: matrix = [[1, 2, 3],[4,5,6]]
//Output: [[1,4],[2,5],[3,6]]

//Constraints:

//m == matrix.length
//n == matrix[i].length
//1 <= m, n <= 1000
//1 <= m * n <= 10^5
//-10^9 <= matrix[i][j] <= 10^9

Console.WriteLine("Hello, World!");


public class Solution
{
    public int[][] Transpose(int[][] matrix)
    {
        if (matrix == null || matrix.Length == 0)
            return Array.Empty<int[]>(); // constraints are broken

        var m = matrix.Length; // rows count in source matrix
        var n = matrix[0].Length; // columns count in source matrix

        if (n == 0)
            return Array.Empty<int[]>(); // constraints are broken

        // Time: O(m*n)
        // Space: O(1) because return result should not be considered
        var result = new int[n][];
        for (var col = 0; col < n; col++)
        {
            result[col] = new int[m];
            for (var row = 0; row < m; row++)
            {
                result[col][row] = matrix[row][col];
            }
        }

        return result;
    }
}