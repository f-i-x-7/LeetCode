// https://leetcode.com/problems/spiral-matrix-ii/

//Given a positive integer n, generate an n x n matrix filled with elements from 1 to n2 in spiral order.

//Example 1:
//Input: n = 3
//Output:[[1,2,3],[8,9,4],[7,6,5]]

//Example 2:
//Input: n = 1
//Output:[[1]]

//Constraints:
//1 <= n <= 20

Test(1);
Test(2);
Test(3);
Test(4);
Test(5);


static void Test(int n)
{
    new Solution().GenerateMatrix(n);
}

public class Solution
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public int[][] GenerateMatrix(int n)
    {
        if (n <= 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Value should be non-negative.");

        // Brute force O(N^2) approach
        var result = new int[n][];
        for (var k = 0; k < n; k++)
        {
            result[k] = new int[n];
        }

        var (i, j) = (0, 0);
        var direction = Direction.Right;
        var x = 0;

        while (x < n * n)
        {
            x++;

            result[i][j] = x;

            switch (direction)
            {
                case Direction.Right:
                    if (j < n - 1 && result[i][j + 1] == 0)
                    {
                        j++;
                    }
                    else
                    {
                        direction = Direction.Down;
                        i++;
                    }
                    break;

                case Direction.Down:
                    if (i < n - 1 && result[i+1][j] == 0)
                    {
                        i++;
                    }
                    else
                    {
                        direction = Direction.Left;
                        j--;
                    }
                    break;

                case Direction.Left:
                    if (j > 0 && result[i][j - 1] == 0)
                    {
                        j--;
                    }
                    else
                    {
                        direction = Direction.Up;
                        i--;
                    }
                    break;

                case Direction.Up:
                    if (i > 0 && result[i - 1][j] == 0)
                    {
                        i--;
                    }
                    else
                    {
                        direction = Direction.Right;
                        j++;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }

        return result;
    }
}