// https://leetcode.com/problems/russian-doll-envelopes/

//You are given a 2D array of integers envelopes where envelopes[i] = [wi, hi] represents the width and the height of an envelope.

//One envelope can fit into another if and only if both the width and height of one envelope are greater than the other envelope's width and height.

//Return the maximum number of envelopes you can Russian doll (i.e., put one inside the other).

//Note: You cannot rotate an envelope.

//Example 1:
//Input: envelopes = [[5, 4],[6,4],[6,7],[2,3]]
//Output: 3
//Explanation: The maximum number of envelopes you can Russian doll is 3 ([2,3] => [5,4] => [6,7]).

//Example 2:
//Input: envelopes = [[1, 1],[1,1],[1,1]]
//Output: 1

//Constraints:
//1 <= envelopes.length <= 10^5
//envelopes[i].length == 2
//1 <= wi, hi <= 10^5


Console.WriteLine("Hello, World!");


public class Solution
{

    public int MaxEnvelopes(int[][] envelopes)
    {
        return MaxEnvelopes_SquareTime(envelopes);
    }

    private int MaxEnvelopes_SquareTime(int[][] envelopes)
    {
        if (envelopes is null || envelopes.Length == 0)
            return 0;

        // Time: O(N^2)
        // Space: O(N)
        // Time Limit Exceeded :(

        // Sort array by ascending width, for same width - by ascending height.
        // O(N * logN time)
        Array.Sort(envelopes, (item1, item2) =>
        {
            if (item1[0] < item2[0])
                return -1;
            if (item1[0] > item2[0])
                return 1;

            return item1[1] - item2[1];
        });

        // Now we can easily find result, given the fact that the array is sorted.
        // Loop with O(N^2) time.
        var dp = new int[envelopes.Length];
        dp[0] = 1; // envelop with the least sizes.
        for (var i = 1; i < dp.Length; i++)
        {
            var currentEnvelop = envelopes[i];
            var countOfLesserEnvelopes = 0;

            for (var j = 0; j < i; j++)
            {
                var previousEnvelop = envelopes[j];
                if (previousEnvelop[0] < currentEnvelop[0] && previousEnvelop[1] < currentEnvelop[1])
                {
                    countOfLesserEnvelopes = Math.Max(countOfLesserEnvelopes, dp[j]);
                }
            }

            dp[i] = 1 + countOfLesserEnvelopes;
        }

        // O(N) time
        return dp.Max();
    }
}