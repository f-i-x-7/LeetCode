// https://leetcode.com/problems/n-queens/

//The n-queens puzzle is the problem of placing n queens on an n x n chessboard such that no two queens attack each other.

//Given an integer n, return all distinct solutions to the n-queens puzzle. You may return the answer in any order.

//Each solution contains a distinct board configuration of the n-queens' placement, where 'Q' and '.' both indicate a queen and an empty space, respectively.

//Example 1:
//Input: n = 4
//Output: [[".Q..","...Q","Q...","..Q."],["..Q.","Q...","...Q",".Q.."]]
//Explanation: There exist two distinct solutions to the 4-queens puzzle as shown above

//Example 2:
//Input: n = 1
//Output: [["Q"]]

//Constraints:
//1 <= n <= 9

new Solution().SolveNQueens(4);

public class Solution
{
    public IList<IList<string>> SolveNQueens(int n) => SolveNQueens_Array(n);

    public IList<IList<string>> SolveNQueens_HashSet(int n)
    {
        if (n <= 0)
            return Array.Empty<string[]>();

        // Time: O(N!)? N choices at 1st row, (N-1) at 2nd, etc. But each new choice requires greater amount of checks...
        // Space: O(N) for call stack & to store currentQueens

        var result = new List<IList<string>>();
        DFS(currentQueens: new HashSet<(int Row, int Col)>(n), n, result);
        return result;



        static void DFS(HashSet<(int Row, int Col)> currentQueens, int n, IList<IList<string>> result)
        {
            if (currentQueens.Count >= n)
            {
                result.Add(CreateSolution(currentQueens, n));
                return;
            }

            var currentRow = currentQueens.Count;
            for (var col = 0; col < n; col++)
            {
                if (CanPlaceNewQueen(currentRow, col, currentQueens))
                {
                    currentQueens.Add((Row: currentRow, Col: col));
                    DFS(currentQueens, n, result);
                    currentQueens.Remove((Row: currentRow, Col: col));
                }
            }
        }

        static string[] CreateSolution(HashSet<(int Row, int Col)> currentQueens, int n)
        {
            var solution = new char[n][];
            for (var i = 0; i < solution.Length; i++)
            {
                solution[i] = new char[n];
                Array.Fill(solution[i], '.');
            }

            foreach (var (queenRow, queenCol) in currentQueens)
            {
                solution[queenRow][queenCol] = 'Q';
            }

            return solution.Select(chars => new string(chars)).ToArray();
        }

        static bool CanPlaceNewQueen(int row, int col, HashSet<(int Row, int Col)> currentQueens)
        {
            foreach (var (queenRow, queenCol) in currentQueens)
            {
                if (queenRow == row || queenCol == col)
                    return false; // row or col is already occuired

                if (Math.Abs(queenRow - row) == Math.Abs(queenCol - col))
                    return false; // diagonal is already occupied
            }

            return true;
        }
    }

    private static readonly (int Row, int Col)[] DiagonalDirections = new[]
    {
        (1, 1), (1, -1), (-1, 1), (-1, -1)
    };

    public IList<IList<string>> SolveNQueens_Array(int n)
    {
        if (n <= 0)
            return Array.Empty<string[]>();

        // Faster than HashSet solution (144 ms vs 220 ms)

        // Time: O(N!) - N choices for 1st row, N-1 for 2nd, etc. For each row we can say that constant amount of work is done (entire board need to be scanned), this differs from hash set implementation
        // Space: O(N^2) for board, O(N) for call stack

        var board = new char[n][];
        for (var row = 0; row < board.Length; row++)
        {
            board[row] = new char[n];
            Array.Fill(board[row], '.');
        }

        var result = new List<IList<string>>();
        DFS(placedQueens: 0, n, result, board);
        return result;




        static void DFS(int placedQueens, int n, IList<IList<string>> result, char[][] board)
        {
            if (placedQueens >= n)
            {
                result.Add(board.Select(chars => new string(chars)).ToArray());
                return;
            }

            var currentRow = placedQueens;

            for (var col = 0; col < n; col++)
            {
                if (CanPlaceNewQueen(currentRow, col, board))
                {
                    board[currentRow][col] = 'Q';
                    DFS(placedQueens + 1, n, result, board);
                    board[currentRow][col] = '.';
                }
            }
        }

        static bool CanPlaceNewQueen(int row, int col, char[][] board)
        {
            var n = board.Length;

            // Check row
            for (var c = 0; c < n; c++)
            {
                if (board[row][c] == 'Q')
                    return false;
            }

            // Check column
            for (var r = 0; r < n; r++)
            {
                if (board[r][col] == 'Q')
                    return false;
            }

            // Check diagonals
            foreach (var (rowDelta, colDelta) in DiagonalDirections)
            {
                var (r, c) = (row + rowDelta, col + colDelta);
                while (r >= 0 && r < n && c >= 0 && c < n)
                {
                    if (board[r][c] == 'Q')
                        return false;

                    r += rowDelta;
                    c += colDelta;
                }
            }

            return true;
        }
    }
}