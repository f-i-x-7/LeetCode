// https://leetcode.com/problems/game-of-life/

//According to Wikipedia's article: "The Game of Life, also known simply as Life, is a cellular automaton devised by the British mathematician John Horton Conway in 1970."

//The board is made up of an m x n grid of cells, where each cell has an initial state: live(represented by a 1) or dead(represented by a 0).
//Each cell interacts with its eight neighbors (horizontal, vertical, diagonal) using the following four rules(taken from the above Wikipedia article):

//Any live cell with fewer than two live neighbors dies as if caused by under-population.
//Any live cell with two or three live neighbors lives on to the next generation.
//Any live cell with more than three live neighbors dies, as if by over-population.
//Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

//The next state is created by applying the above rules simultaneously to every cell in the current state, where births and deaths occur simultaneously. Given the current state of the m x n grid board, return the next state.

 

//Example 1:
//Input: board = [[0, 1, 0],[0,0,1],[1,1,1],[0,0,0]]
//Output:[[0,0,0],[1,0,1],[0,1,1],[0,1,0]]

//Example 2:
//Input: board = [[1, 1],[1,0]]
//Output:[[1,1],[1,1]]
 

//Constraints:
//m == board.length
//n == board[i].length
//1 <= m, n <= 25
//board[i][j] is 0 or 1.


//Follow up:
//Could you solve it in-place? Remember that the board needs to be updated simultaneously: You cannot update some cells first and then use their updated values to update other cells.

//In this question, we represent the board using a 2D array.In principle, the board is infinite, which would cause problems when the active area encroaches upon the border of the array (i.e., live cells reach the border).
//How would you address these problems?


Console.WriteLine("Hello, World!");

public class Solution
{
    public void GameOfLife(int[][] board)
    {
        if (board == null || board.Length == 0)
            return;

        const int Dead = 0;
        const int Alive = 1;
        const int Resurrecting = 2;
        const int Dying = 3;

        // Initially brute force solution was implemented, O(N) time, O(N) memory (where N = m x n).
        // After peeking into Leetcode discussions, reimplemented with in-place updating - O(1) memory.
        // Trick is with using more values (not only 0 and 1).

        // Technically some minor perf improvements can be made with unsafe code - iterate to remove arrays bounds check
        // (I am pretty sure that JIT is not smart enough to eliminate this bounds check when loop variables are compared with m and n).
        var m = board.Length;
        var n = board[0].Length;

        for (var i = 0; i < m; i++)
        {
            var upperNeighborsRowIndex = Math.Max(0, i - 1);
            var lowerNeighborsRowIndex = Math.Min(m - 1, i + 1);

            for (var j = 0; j < n; j++)
            {
                var liveNeighbors = 0;

                var leftNeighborsColumnIndex = Math.Max(0, j - 1);
                var rightNeighborsColumnIndex = Math.Min(n - 1, j + 1);

                for (var k = upperNeighborsRowIndex; k <= lowerNeighborsRowIndex; k++)
                {
                    for (var l = leftNeighborsColumnIndex; l <= rightNeighborsColumnIndex; l++)
                    {
                        if (k == i && l == j)
                            continue;

                        var cellValue = board[k][l];
                        if (cellValue == Alive || cellValue == Dying) // Alive before transformation
                            liveNeighbors++;
                    }
                }

                System.Diagnostics.Debug.Assert(liveNeighbors >= 0 && liveNeighbors <= 8);

                if (board[i][j] == Dead)
                {
                    // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
                    if (liveNeighbors == 3)
                        board[i][j] = Resurrecting;

                    // Cell remains dead. Do nothing.
                }
                else
                {
                    // Live cell
                    switch (liveNeighbors)
                    {
                        case 2:
                        case 3:
                            // Any live cell with two or three live neighbors lives on to the next generation.
                            // Do nothing.
                            break;

                        default:
                            // Any live cell with fewer than two live neighbors dies as if caused by under-population.
                            // OR
                            // Any live cell with more than three live neighbors dies, as if by over-population.
                            board[i][j] = Dying;
                            break;
                    }
                }
            }
        }

        // Transform board.
        for (var i = 0; i < m; i++)
        {
            for (var j = 0; j < n; j++)
            {
                switch (board[i][j])
                {
                    case Dying:
                        board[i][j] = Dead;
                        break;
                    case Resurrecting:
                        board[i][j] = Alive;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}