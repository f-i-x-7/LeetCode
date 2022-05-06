// https://leetcode.com/problems/remove-all-adjacent-duplicates-in-string-ii/

//You are given a string s and an integer k, a k duplicate removal consists of choosing k adjacent and equal letters from s and removing them, causing the left and the right side of the deleted substring to concatenate together.
//We repeatedly make k duplicate removals on s until we no longer can.
//Return the final string after all such duplicate removals have been made. It is guaranteed that the answer is unique.

//Example 1:
//Input: s = "abcd", k = 2
//Output: "abcd"
//Explanation: There's nothing to delete.

//Example 2:
//Input: s = "deeedbbcccbdaa", k = 3
//Output: "aa"
//Explanation:
//First delete "eee" and "ccc", get "ddbbbdaa"
//Then delete "bbb", get "dddaa"
//Finally delete "ddd", get "aa"

//Example 3:
//Input: s = "pbbcggttciiippooaais", k = 2
//Output: "ps"
//Explanation:
//First delete "bb", "gg", "tt", "ii", "pp", "oo" and "aa", get "pcciis"
//Then delete "cc" and "ii", get "ps"

//Constraints:
//1 <= s.length <= 10^5
//2 <= k <= 10^4
//s only contains lower case English letters.


using System.Text;

Test("abcd", 2, "abcd");
Test("deeedbbcccbdaa", 3, "aa");
Test("pbbcggttciiippooaais", 2, "ps");

static void Test(string s, int k, string expectedResult)
{
    var actualResult = new Solution().RemoveDuplicates(s, k);
    if (expectedResult != actualResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {s}, k={k}, ExpectedResult={expectedResult}, ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {s}, k={k}, Result={expectedResult}");
    }
}

public class Solution
{
    public string RemoveDuplicates(string s, int k) => RemoveDuplicates_Optimal(s, k);

    public string RemoveDuplicates_Approach1(string s, int k)
    {
        // O(N^2) time (loop over string of length N), O(N) space (same string builder is reused)
        // Unfortunately time limit was exceeded for one test case :(
        var sb = new StringBuilder();
        while (true)
        {
            var duplicatesCount = 1;
            var indexBeforeDuplicates = 0;
            var anySymbolsRemoved = false;

            for (var i = 1; i < s.Length; i++)
            {
                if (s[i] == s[i - 1])
                {
                    duplicatesCount++;
                    if (duplicatesCount == k)
                    {
                        anySymbolsRemoved = true;
                        duplicatesCount = 1;
                        // i-th symbol is removed, so do not compare next symbol with it (in other words, skip one iteration)
                        i++;
                        indexBeforeDuplicates = i;
                    }
                }
                else
                {
                    duplicatesCount = 1;
                    for (var j = indexBeforeDuplicates; j < i; j++)
                    {
                        sb.Append(s[j]);
                    }
                    indexBeforeDuplicates = i;
                }
            }

            // last items could be duplicates, but with length less than k. Need to add them to string builder
            for (var j = indexBeforeDuplicates; j < s.Length; j++)
            {
                sb.Append(s[j]);
            }

            if (!anySymbolsRemoved)
                break;

            s = sb.ToString();
            sb.Clear();
        }

        return s;
    }

    public string RemoveDuplicates_Approach2(string s, int k)
    {
        // O(N^2) time (loop over string of length N), O(N) space (same string builder and list are reused)
        // Unfortunately time limit was exceeded for one test case :(
        var indicesToSkipStartingFrom = new List<int>();
        var sb = new StringBuilder();

        while (true)
        {
            var duplicatesCount = 1;
            for (var i = 1; i < s.Length; i++)
            {
                if (s[i - 1] == s[i])
                {
                    duplicatesCount++;
                    if (duplicatesCount == k)
                    {
                        indicesToSkipStartingFrom.Add(i + 1 - k);
                    }
                }
                else
                {
                    duplicatesCount = 1;
                }
            }

            if (indicesToSkipStartingFrom.Count == 0)
                break;

            var indexOfIndexFromSkipList = 0;
            for (var i = 0; i < s.Length; i++)
            {
                if (indexOfIndexFromSkipList < indicesToSkipStartingFrom.Count && i == indicesToSkipStartingFrom[indexOfIndexFromSkipList])
                {
                    i += k - 1; // i-1 because of 'i++' in 'for' loop
                    indexOfIndexFromSkipList++;
                }
                else
                {
                    sb.Append(s[i]);
                }
            }

            s = sb.ToString();
            sb.Clear();
            indicesToSkipStartingFrom.Clear();
        }

        return s;
    }

    public string RemoveDuplicates_Optimal(string s, int k)
    {
        // O(N * k) time? O(N) space - for stack and string builder
        // Used Leetcode hint that stack should be used
        var duplicatesCount = 1;
        var previous = default(char);

        var chars = new Stack<char>();

        for (var i = s.Length - 1; i >= 0; i--)
        {
            var current = s[i];
            if (current == previous)
            {
                duplicatesCount++;
                if (duplicatesCount == k)
                {
                    // Remove k-1 items from stack and do not push current char
                    var counter = 0;
                    while (counter < k - 1)
                    {
                        chars.Pop();
                        counter++;
                    }

                    if (chars.Count == 0)
                    {
                        previous = default;
                        duplicatesCount = 0;
                    }
                    else
                    {
                        previous = chars.Pop();
                        duplicatesCount = 1;

                        if (chars.Count > 0)
                        {
                            var stack = new Stack<char>();
                            
                            while (true)
                            {
                                var ch = chars.Pop();
                                stack.Push(ch);

                                if (ch == previous)
                                {
                                    duplicatesCount++;
                                }

                                if (ch != previous || chars.Count == 0)
                                {
                                    System.Diagnostics.Debug.Assert(duplicatesCount < k);
                                    while (stack.Count > 0)
                                    {
                                        chars.Push(stack.Pop());
                                    }
                                    break;
                                }
                            }
                        }

                        chars.Push(previous);
                    }
                }
                else
                {
                    previous = current;
                    chars.Push(current);
                }
            }
            else
            {
                duplicatesCount = 1;
                previous = current;
                chars.Push(current);
            }
        }
        
        // Build string without duplciates
        var sb = new StringBuilder(chars.Count);
        while (chars.Count > 0)
        {
            sb.Append(chars.Pop());
        }
        return sb.ToString();
    }
}