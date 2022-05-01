// https://leetcode.com/problems/backspace-string-compare/

//Given two strings s and t, return true if they are equal when both are typed into empty text editors. '#' means a backspace character.

//Note that after backspacing an empty text, the text will continue empty.

//Example 1:
//Input: s = "ab#c", t = "ad#c"
//Output: true
//Explanation: Both s and t become "ac".

//Example 2:
//Input: s = "ab##", t = "c#d#"
//Output: true
//Explanation: Both s and t become "".

//Example 3:
//Input: s = "a#c", t = "b"
//Output: false
//Explanation: s becomes "c" while t becomes "b".

//Constraints:
//1 <= s.length, t.length <= 200
//s and t only contain lowercase letters and '#' characters.

//Follow up: Can you solve it in O(n) time and O(1) space?

using System.Text;

Test("ab#c", "ad#c", true); // result string is "ac"
Test("ab##", "c#d#", true); // result string is ""
Test("a#c", "b", false); // result strings are "c" and "b"

Test("y#fo##f", "y#f#o##f", true); // result string is "f"

Test("#", "##", true); // result string is ""
Test("##a#d#", "##b##c#", true); // result string is ""






static void Test(string s, string t, bool expectedResult)
{
    var actualResult = new Solution().BackspaceCompare(s, t);
    if (actualResult == expectedResult)
    {
        Console.WriteLine($"SUCCESS: s={s}, t={t}, Result={expectedResult}");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: s={s}, t={t}, ExpectedResult={expectedResult}, ActualResult={actualResult}");
        Console.ResetColor();
    }
}


public class Solution
{
    public bool BackspaceCompare(string s, string t)
    {
        return BackspaceCompare_Optimal(s, t);
    }

    public bool BackspaceCompare_Naive(string s, string t)
    {
        // O(N) time, O(N) space
        var sb1 = new StringBuilder(s.Length);
        var sb2 = new StringBuilder(t.Length);

        var i = 0;
        while (true)
        {
            if (i >= s.Length && i >= t.Length)
            {
                break;
            }

            if (i < s.Length)
            {
                ProcessChar(i, s, sb1);
            }

            if (i < t.Length)
            {
                ProcessChar(i, t, sb2);
            }

            i++;
        }

        return sb1.ToString() == sb2.ToString();



        static void ProcessChar(int i, string str, StringBuilder sb)
        {
            if (str[i] == '#')
            {
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
            }
            else
                sb.Append(str[i]);
        }
    }

    public bool BackspaceCompare_Optimal(string s, string t)
    {
        // O(N) time, O(1) space

        // Compare chars while iterating from tail, break loop and return false when 1st discrepance is found.
        // Process 1 char per iteration.
        var index1 = s.Length - 1;
        var index2 = t.Length - 1;

        while (true)
        {
            var ch1Exists = TryGetNextChar(s, ref index1, out var ch1);
            var ch2Exists = TryGetNextChar(t, ref index2, out var ch2);

            if (!ch1Exists && !ch2Exists)
                return true;

            if (!ch1Exists || !ch2Exists)
                return false; // strings of different length

            if (ch1 != ch2)
                return false;

            index1--;
            index2--;
        }



        static bool TryGetNextChar(string str, ref int i, out char result)
        {
            var skip = 0;

            while (i >= 0)
            {
                var ch = str[i];
                if (ch == '#')
                {
                    skip++;
                }
                else
                {
                    if (skip > 0)
                    {
                        skip--;
                    }
                    else
                    {
                        result = ch;
                        return true;
                    }
                }

                i--;
            }

            result = default;
            return false;
        }
    }
}