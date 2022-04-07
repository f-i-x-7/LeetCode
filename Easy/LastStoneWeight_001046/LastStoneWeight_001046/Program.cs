// https://leetcode.com/problems/last-stone-weight/

//You are given an array of integers stones where stones[i] is the weight of the ith stone.

//We are playing a game with the stones. On each turn, we choose the heaviest two stones and smash them together. Suppose the heaviest two stones have weights x and y with x <= y. The result of this smash is:

//If x == y, both stones are destroyed, and
//If x != y, the stone of weight x is destroyed, and the stone of weight y has new weight y-x.
//At the end of the game, there is at most one stone left.
//Return the smallest possible weight of the left stone. If there are no stones left, return 0.

//Example 1:
//Input: stones = [2, 7, 4, 1, 8, 1]
//Output: 1
//Explanation:
//We combine 7 and 8 to get 1 so the array converts to [2,4,1,1,1] then,
//we combine 2 and 4 to get 2 so the array converts to [2,1,1,1] then,
//we combine 2 and 1 to get 1 so the array converts to [1,1,1] then,
//we combine 1 and 1 to get 0 so the array converts to [1] then that's the value of the last stone.

//Example 2:
//Input: stones = [1]
//Output: 1

//Constraints:
//1 <= stones.length <= 30
//1 <= stones[i] <= 1000

Test(new[] { 1 }, 1);
Test(new[] { 5 }, 5);
Test(new[] { 1, 1 }, 0);
Test(new[] { 5, 2 }, 3);
Test(new[] { 2, 5 }, 3);
Test(new[] { 2, 7, 4, 1, 8, 1 }, 1);




static void Test(int[] stones, int expectedResult)
{
    var copy = new int[stones.Length];
    Array.Copy(stones, copy, stones.Length);

    var actualResult = Solution.LastStoneWeight(stones);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {ToStr(copy)}; ExpectedResult={expectedResult}; ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCEEDED: {ToStr(copy)}; Result={expectedResult}");
    }

    string ToStr(int[] arr) => "[" + string.Join(',', arr) + "]";
}

public static class Solution
{
    public static int LastStoneWeight(int[] stones) => LastStoneWeight_FindMaxInPlace(stones);

    private static int LastStoneWeight_WithArraySort(int[] stones)
    {
        // This solution is easier to write and read because built-in sorting function is used,
        // but actually it is not as performant as other solutions could be because actually we need only 2 max elements in array (this can be done in O(N) time), not the entire sorted array (which requires O(N * logN) time).
        // So this implementation requires O(N^2 * logN) time (number of while loop iterations is proportional to N because we smash no more than 2 stones per iteration).

        if (stones is null || stones.Length == 0)
            return -1;

        if (stones.Length == 1)
            return stones[0];

        while (true)
        {
            Array.Sort(stones);

            var x = stones[stones.Length - 1];
            if (x == 0)
            {
                // No stones left
                return 0;
            }
            var y = stones[stones.Length - 2];
            System.Diagnostics.Debug.Assert(x >= y);
            if (y == 0)
            {
                // One stone left
                return x;
            }

            // Smash stones
            stones[stones.Length - 2] = 0;
            stones[stones.Length - 1] = x - y;
        }
    }

    private static int LastStoneWeight_FindMaxInPlace(int[] stones)
    {
        // This implementation requires O(N^2) time (number of while loop iterations is proportional to N because we smash no more than 2 stones per iteration).

        if (stones is null || stones.Length == 0)
            return -1;

        if (stones.Length == 1)
            return stones[0];

        while (true)
        {
            var max1 = 0; // max value in array
            var max1Index = -1;
            var max2 = 0; // submax value in array; can be equal to max1 if there are >= 2 same maximums
            var max2Index = -1;

            for (var i = 0; i < stones.Length; i++)
            {
                var stone = stones[i];
                if (stone > max1)
                {
                    max2 = max1;
                    max2Index = max1Index;

                    max1 = stone;
                    max1Index = i;
                }
                else if (stone > max2)
                {
                    max2 = stone;
                    max2Index = i;
                }
            }

            System.Diagnostics.Debug.Assert(max1 >= max2);

            if (max1 == 0)
            {
                // No stones left
                return 0;
            }

            if (max2 == 0)
            {
                // One stone left
                return max1;
            }

            // Smash stones
            stones[max2Index] = 0;
            stones[max1Index] = max1 - max2;
        }
    }
}