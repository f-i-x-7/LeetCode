// https://leetcode.com/problems/letter-combinations-of-a-phone-number/

//Given a string containing digits from 2-9 inclusive, return all possible letter combinations that the number could represent. Return the answer in any order.

//A mapping of digit to letters (just like on the telephone buttons) is given below. Note that 1 does not map to any letters.

//Example 1:
//Input: digits = "23"
//Output: ["ad","ae","af","bd","be","bf","cd","ce","cf"]

//Example 2:
//Input: digits = ""
//Output: []

//Example 3:
//Input: digits = "2"
//Output: ["a","b","c"]

//Constraints:
//0 <= digits.length <= 4
//digits[i] is a digit in the range['2', '9'].

Console.WriteLine("Hello, World!");


public class Solution
{
    private static readonly Dictionary<char, char[]> map = new()
    {
        ['2'] = new char[] { 'a', 'b', 'c' },
        ['3'] = new char[] { 'd', 'e', 'f' },
        ['4'] = new char[] { 'g', 'h', 'i' },
        ['5'] = new char[] { 'j', 'k', 'l' },
        ['6'] = new char[] { 'm', 'n', 'o' },
        ['7'] = new char[] { 'p', 'q', 'r', 's' },
        ['8'] = new char[] { 't', 'u', 'v' },
        ['9'] = new char[] { 'w', 'x', 'y', 'z' }
    };

    public IList<string> LetterCombinations(string digits)
    {
        if (string.IsNullOrEmpty(digits))
            return Array.Empty<string>();

        // O(N * 3^N) - O(N * 4^N) time, O(3^N) - O(4^N) space

        // Try to presize list with required capacity.
        // Assume that each digit is mapped to 3 letters, then for N digits there are 3^N possible combinations.
        // This is lower bound (e.g. if there are '7' or '9' digits then actual count of combinations is bigger)
        // Result list takes from O(3^N) to O(4^N) space
        var result = new List<char[]>((int)Math.Pow(3, digits.Length));

        // 1st step: O(N) time, O(N) space
        var chars = digits.Select(ch => map[ch]).ToArray();

        // 2nd step: O(1) time, O(N) space (number of iterations is either 3 or 4, and array allocation of size N inside each iteration takes O(1) time because no LOH involved)
        foreach (var firstChar in chars[0])
        {
            var arr = new char[digits.Length];
            arr[0] = firstChar;
            result.Add(arr);
        }

        // 3rd step: O(N * X ^ N) time: N-1 iterations, up to O(4^N) time each
        for (var i = 1; i < chars.Length; i++)
        {
            var nextChars = chars[i];
            var resultSnapshot = result.ToArray(); // up to O(4^N) time

            // This inner loop takes up to O(4^N) time: number of iterations is constant (2 or 3), but AddRange() inside is a hidden loop
            // with O(K) time complexity where K is result.Count at this moment, and K growth exponentially
            for (var j = 0; j < nextChars.Length - 1; j++)
            {
                result.AddRange(resultSnapshot.Select(ch => (char[])ch.Clone()));
            }

            // again, up to O(4^N) time
            var k = 0;
            var counter = 0;
            for (var j = 0; j < result.Count; j++)
            {
                result[j][i] = nextChars[k];
                counter++;
                if (counter == resultSnapshot.Length)
                {
                    counter = 0;
                    k++;
                }
            }
        }

        // 4th step: up to O(4^N) time
        return result.Select(ch => new string(ch)).ToArray();
    }
}