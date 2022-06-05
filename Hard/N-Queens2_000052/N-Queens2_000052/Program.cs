// https://leetcode.com/problems/n-queens-ii/

//The n-queens puzzle is the problem of placing n queens on an n x n chessboard such that no two queens attack each other.

//Given an integer n, return the number of distinct solutions to the n-queens puzzle.

//Example 1:
//Input: n = 4
//Output: 2
//Explanation: There are two distinct solutions to the 4-queens puzzle as shown.

//Example 2:
//Input: n = 1
//Output: 1

//Constraints:
//1 <= n <= 9


public class Solution
{
    public int TotalNQueens(int n)
    {
        return TotalNQueens_HashSet(n);
    }

    public int TotalNQueens_HashSet(int n)
    {
        if (n <= 0)
            return 0;

        // Solution based on Discussions section of previous task https://leetcode.com/problems/n-queens/
        // Time: O(N!)? N choices in 1st row, (N-1) in the 2nd, etc. But for each next row we have to do greater amount of work while defining whether cell is suitable for queen...
        // Space: O(N) for call stack and queens positions

        // NOTE: according to comments, arrays with flags work up to 3 times faster than HashSet

        var queens = new HashSet<(int Row, int Col)>(n);
        var result = 0;
        DFS(0, ref result);
        return result;



        void DFS(int currentRow, ref int result)
        {
            if (currentRow >= n)
            {
                result++;
                return;
            }

            // Need to place new queen at current row. Iterate all columns and find all possible options.
            for (var col = 0; col < n; col++)
            {
                if (CanPlaceQueen(currentRow, col))
                {
                    queens.Add((Row: currentRow, Col: col));
                    DFS(currentRow + 1, ref result);
                    queens.Remove((Row: currentRow, Col: col));
                }
            }
        }

        bool CanPlaceQueen(int row, int col)
        {
            foreach (var queen in queens)
            {
                if (queen.Row == row || queen.Col == col)
                    return false; // same row or column

                if (Math.Abs(queen.Row - row) == Math.Abs(queen.Col - col))
                    return false; // same diagonal
            }

            return true;
        }
    }

    public int TotalNQueens_Arrays(int n)
    {
        if (n <= 0)
            return 0;

        // Solution based on Discussions section of previous task https://leetcode.com/problems/n-queens/
        // Time: O(N!) - N choices in 1st row, (N-1) in the 2nd, etc.
        // Space: O(N) for call stack and queens positions

        var occupiedRows = new bool[n];
        var occupiedCols = new bool[n];

        // Actually there are 2 * (N - 2) + 1 diagonals, but if we consider the corners for simplicity, then we get 2 * N - 1.

        // Diagonals parallel to one from (0,N-1) to (N-1,0); the property in such diagonals is that rowIndex+colIndex is the same.
        var occupiedDiagonals1 = new bool[2 * n - 1];

        // Diagonals parallel to one from (0,0) to (N-1,N-1); the property in such diagonals is that rowIndex-colIndex is the same;
        // in order to receive non-negative indexes, we should add (n-1) to this expression.
        var occupiedDiagonals2 = new bool[2 * n - 1];

        var result = 0;
        DFS(0, ref result);
        return result;



        void DFS(int currentRow, ref int result)
        {
            if (currentRow >= n)
            {
                result++;
                return;
            }

            // Need to place new queen at current row. Iterate all columns and find all possible options.
            for (var col = 0; col < n; col++)
            {
                if (CanPlaceQueen(currentRow, col))
                {
                    var d1 = CalcDiag1(currentRow, col);
                    var d2 = CalcDiag2(currentRow, col, n);
                    occupiedRows[currentRow] = true;
                    occupiedCols[col] = true;
                    occupiedDiagonals1[d1] = true;
                    occupiedDiagonals2[d2] = true;

                    DFS(currentRow + 1, ref result);

                    occupiedRows[currentRow] = false;
                    occupiedCols[col] = false;
                    occupiedDiagonals1[d1] = false;
                    occupiedDiagonals2[d2] = false;
                }
            }
        }

        bool CanPlaceQueen(int row, int col)
        {
            if (occupiedRows[row] || occupiedCols[col])
                return false;

            var d1 = CalcDiag1(row, col);
            if (occupiedDiagonals1[d1])
                return false;

            var d2 = CalcDiag2(row, col, n);
            if (occupiedDiagonals2[d2])
                return false;

            return true;
        }

        static int CalcDiag1(int row, int col) => row + col;
        static int CalcDiag2(int row, int col, int n) => row - col + n - 1;
    }
}