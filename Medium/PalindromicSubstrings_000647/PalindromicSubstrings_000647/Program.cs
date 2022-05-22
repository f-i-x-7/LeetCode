// https://leetcode.com/problems/palindromic-substrings/

//Given a string s, return the number of palindromic substrings in it.

//A string is a palindrome when it reads the same backward as forward.

//A substring is a contiguous sequence of characters within the string.

//Example 1:
//Input: s = "abc"
//Output: 3
//Explanation: Three palindromic strings: "a", "b", "c".

//Example 2:
//Input: s = "aaa"
//Output: 6
//Explanation: Six palindromic strings: "a", "a", "a", "aa", "aa", "aaa".

//Constraints:
//1 <= s.length <= 1000
//s consists of lowercase English letters.

Console.WriteLine("Hello, World!");

public class Solution
{
    public int CountSubstrings(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        var result = 0;

        // Time: O(N^2)?
        // Space: O(1)

        // Try odd length substrings first. Start from length 1, then increase to 3, if such substrings are palindromes - increase to 5, and so on.
        // Stop when substring is not palindrome, because adding one char from head and one char from tail definitely won't produce a palindrome.
        for (var i = 0; i < s.Length; i++)
        {
            var startIndex = i;
            var endIndex = i;
            while (IsPalindrome(s, startIndex, endIndex))
            {
                result++;
                startIndex--;
                endIndex++;
            }
        }

        // Now try even length substrings, start from length 2, then increase to 4, then for palindromic - to 6, and so on.
        // Logic is the same.
        for (var i = 0; i < s.Length - 1; i++)
        {
            var startIndex = i;
            var endIndex = i + 1;
            while (IsPalindrome(s, startIndex, endIndex))
            {
                result++;
                startIndex--;
                endIndex++;
            }
        }

        return result;
    }

    private static bool IsPalindrome(string s, int startIndex, int endIndex)
    {
        System.Diagnostics.Debug.Assert(startIndex <= endIndex);

        if (startIndex < 0 || endIndex >= s.Length)
            return false;

        var head = startIndex;
        var tail = endIndex;

        while (head < tail)
        {
            if (s[head] != s[tail])
                return false;

            head++;
            tail--;
        }

        return true;
    }
}