// https://leetcode.com/problems/longest-substring-without-repeating-characters/

//Given a string s, find the length of the longest substring without repeating characters.

//Example 1:
//Input: s = "abcabcbb"
//Output: 3
//Explanation: The answer is "abc", with the length of 3.

//Example 2:
//Input: s = "bbbbb"
//Output: 1
//Explanation: The answer is "b", with the length of 1.

//Example 3:
//Input: s = "pwwkew"
//Output: 3
//Explanation: The answer is "wke", with the length of 3.
//Notice that the answer must be a substring, "pwke" is a subsequence and not a substring.

//Constraints:
//0 <= s.length <= 5 * 10^4
//s consists of English letters, digits, symbols and spaces.


Console.WriteLine(new Solution().LengthOfLongestSubstring("abcabcbb")); // 3
Console.WriteLine(new Solution().LengthOfLongestSubstring("bbbbb")); // 1
Console.WriteLine(new Solution().LengthOfLongestSubstring("pwwkew")); // 3
Console.WriteLine(new Solution().LengthOfLongestSubstring("pwwke")); // 3
Console.WriteLine(new Solution().LengthOfLongestSubstring("dvdf")); // 3
Console.WriteLine(new Solution().LengthOfLongestSubstring("aabaab!bb")); // 3


public class Solution
{
    public int LengthOfLongestSubstring(string s) => LengthOfLongestSubstring_SlidingWindowOptimized(s);

    public int LengthOfLongestSubstring_FirstImpl(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        // Time: O(N)? or O(N^2) worst? Seems line it is similar to sliding window approach, but instead of maintaining 2 pointers,
        // we do backwards iteration. So it is likely squre time.
        // Space: O(N)

        var max = 0;
        var chars = new HashSet<char>();

        for (var i = 0; i < s.Length; i++)
        {
            if (!chars.Add(s[i]))
            {
                max = Math.Max(max, chars.Count);
                chars.Clear();

                // TODO: check whether it is worth to iterate further? May be remaining characters count is less than current max (micro-optimization)

                // May be need to backtrack like in "dvdf" string
                var previousIndexOfThisChar = s.LastIndexOf(s[i], startIndex: i - 1, count: i);
                for (var j = previousIndexOfThisChar + 1; j <= i; j++)
                {
                    chars.Add(s[j]);
                }
            }
        }

        max = Math.Max(max, chars.Count);

        return max;
    }

    public int LengthOfLongestSubstring_SlidingWindow(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        // Implemented after peeking into Leetcode solutions.
        // Time: O(N)? Actually in worst case there should be O(2N) - each char is iterated twice
        // Space: O(N)

        var max = 0;
        var chars = new HashSet<char>();

        for (var (l, r) = (0, 0); r < s.Length; r++)
        {
            var rChar = s[r];
            if (!chars.Add(rChar))
            {
                max = Math.Max(max, chars.Count);

                while (true)
                {
                    var lChar = s[l];
                    l++;
                    if (lChar == rChar)
                        break;
                    chars.Remove(lChar);
                }

                // TODO: check whether it is worth to iterate further? May be remaining characters count is less than current max (micro-optimization)
            }
        }

        max = Math.Max(max, chars.Count);

        return max;
    }

    public int LengthOfLongestSubstring_SlidingWindowOptimized(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        // Implemented after peeking into Leetcode solutions.
        // Time: O(N); each char is not iterated twice comparing to previous approach
        // Space: O(N)

        var max = 0;
        var charIndices = new Dictionary<char, int>();
        var (l, r) = (0, 0);

        for (; r < s.Length; r++)
        {
            var rChar = s[r];
            if (charIndices.TryGetValue(rChar, out var rCharPrevIndex))
            {
                if (rCharPrevIndex >= l)
                {
                    max = Math.Max(r - l, max);
                    l = rCharPrevIndex + 1;

                    // Check whether it is worth to iterate further: may be remaining characters count is less than current max.
                    // This is an optimization for case when there are long unique substrings,
                    // but it will degrade performance if string contains short unique substrings.
                    if (s.Length - l <= max)
                        return max;
                }
            }

            charIndices[rChar] = r;
        }

        max = Math.Max(r - l, max);

        return max;
    }
}