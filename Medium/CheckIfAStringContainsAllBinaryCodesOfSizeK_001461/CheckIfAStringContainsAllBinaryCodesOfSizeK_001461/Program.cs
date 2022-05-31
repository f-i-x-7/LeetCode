// https://leetcode.com/problems/check-if-a-string-contains-all-binary-codes-of-size-k/

//Given a binary string s and an integer k, return true if every binary code of length k is a substring of s. Otherwise, return false.

//Example 1:
//Input: s = "00110110", k = 2
//Output: true
//Explanation: The binary codes of length 2 are "00", "01", "10" and "11". They can be all found as substrings at indices 0, 1, 3 and 2 respectively.

//Example 2:
//Input: s = "0110", k = 1
//Output: true
//Explanation: The binary codes of length 1 are "0" and "1", it is clear that both exist as a substring. 

//Example 3:
//Input: s = "0110", k = 2
//Output: false
//Explanation: The binary code "00" is of length 2 and does not exist in the array.

//Constraints:
//1 <= s.length <= 5 * 10^5
//s[i] is either '0' or '1'.
//1 <= k <= 20

Console.WriteLine("Hello, World!");


public class Solution
{
    public bool HasAllCodes(string s, int k) => HasAllCodes_Naive(s, k);

    private bool HasAllCodes_Naive(string s, int k)
    {
        if (s is null || s.Length < k || k <= 0)
            return false;

        var targetCount = (int)Math.Pow(2, k);

        // Time: O(N - k) or O((N - k) * k)? Assuming that hash code calculation is O(1), so it depends on whether substring is O(k) or O(1)
        // Space: O(2^k) max?
        // Also validation of string contents can be easily introduced here without increasing time or space complexity (well, O(N - k) actually will become O(N) but this is negligible)
        var codes = new HashSet<string>();
        for (var i = 0; i <= s.Length - k; i++)
        {
            var code = s.Substring(i, k);
            if (codes.Add(code) && codes.Count == targetCount)
                return true;
        }
        return false;
    }

    private bool HasAllCodes_RollingHash(string s, int k)
    {
        if (s is null || s.Length < k || k <= 0)
            return false;

        var targetCount = (int)Math.Pow(2, k);
        var hasCombinations = new bool[targetCount];

        // Solution is peeked at Leetcode.
        // Actually, if diving deeper, then regular hash code calculation for string of length k seem to be O(k) operation.
        // This approach has O(1) time complexity operation for processing of each of k-length substirngs.
        // Time: O(N + 2^k)
        // Space: O(2^k)
        // Validation of string contents here can be easily introduced too without increasing time or space complexity.

        var allOnesBitMask = targetCount - 1; // all bits are 1, length is k binary digits
        var hash = 0;

        // Time: O(N)
        for (var i = 0; i < s.Length; i++)
        {
            var currentDigit = s[i] - '0'; // either 0 or 1
            hash = (hash << 1) & allOnesBitMask | currentDigit;
            if (i >= k - 1)
                hasCombinations[hash] = true;
        }

        // Time: O(2^K)
        return hasCombinations.All(x => x); // all true
    }
}
