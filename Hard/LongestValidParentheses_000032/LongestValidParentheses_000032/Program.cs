// https://leetcode.com/problems/longest-valid-parentheses/

//Given a string containing just the characters '(' and ')', find the length of the longest valid (well-formed) parentheses substring.

//Example 1:
//Input: s = "(()"
//Output: 2
//Explanation: The longest valid parentheses substring is "()".

//Example 2:
//Input: s = ")()())"
//Output: 4
//Explanation: The longest valid parentheses substring is "()()".

//Example 3:
//Input: s = ""
//Output: 0

//Constraints:
//0 <= s.length <= 3 * 104
//s[i] is '(', or ')'.

Test("", 0);
Test("(((", 0);
Test(")))", 0);
Test("(())", 4);

Test("(()", 2);
Test(")()())", 4);
Test("()(()", 2);
Test("(()(((()", 2);

void Test(string s, int expectedResult)
{
    var actualResult = new Solution().LongestValidParentheses(s);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {s}, ExpectedResult={expectedResult}, ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {s}, Result={actualResult}");
    }
}

public class Solution
{
    public int LongestValidParentheses(string s)
    {
        return LongestValidParentheses_LinearAttempt(s);
    }

    private int LongestValidParentheses_LinearAttempt(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;

        // Time: O(N)
        // Space: O(N)

        var result = 0;
        var chars = new Stack<char>();
        var counter = 0;
        var counterAfterLastValidParentheses = 0;

        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == '(')
            {
                chars.Push(s[i]);
                counter++;
                counterAfterLastValidParentheses++;
            }
            else
            {
                if (chars.Count > 0)
                {
                    chars.Pop();
                    counter++;
                    if (chars.Count == 0)
                    {
                        result = Math.Max(result, counter);
                        counterAfterLastValidParentheses = 0;
                    }
                    else
                    {
                        counterAfterLastValidParentheses++;
                    }
                }
                else
                {
                    counter = 0;
                }
            }
        }

        // Imagine a string e.g. "(()"
        // Even if we finished string traversal and stack is not empty, we can compute result
        result = Math.Max(result, counterAfterLastValidParentheses - chars.Count);

        return result;
    }
}