// https://leetcode.com/problems/maximum-product-of-word-lengths/

//Given a string array words, return the maximum value of length(word[i]) *length(word[j]) where the two words do not share common letters. If no such two words exist, return 0.

//Example 1:
//Input: words = ["abcw", "baz", "foo", "bar", "xtfn", "abcdef"]
//Output: 16
//Explanation: The two words can be "abcw", "xtfn".

//Example 2:
//Input: words = ["a", "ab", "abc", "d", "cd", "bcd", "abcd"]
//Output: 4
//Explanation: The two words can be "ab", "cd".

//Example 3:
//Input: words = ["a", "aa", "aaa", "aaaa"]
//Output: 0
//Explanation: No such pair of words.

//Constraints:
//2 <= words.length <= 1000
//1 <= words[i].length <= 1000
//words[i] consists only of lowercase English letters.

Console.WriteLine("Hello, World!");


public class Solution
{
    public int MaxProduct(string[] words)
    {
        return MaxProduct_BruteForce(words);
    }

    private int MaxProduct_BruteForce(string[] words)
    {
        if (words == null || words.Length < 2)
            return 0;

        // Time: O(N^2)
        // Space: O(N^2)
        // Time Limit Exceeded
        var n = words.Length;
        var data = new (string Word1, string Word2, int Product)[n * (n - 1) / 2];

        var k = 0;
        for (var i = 0; i < n; i++)
        {
            for (var j = i + 1; j < n; j++)
            {
                if (i == j)
                    continue;

                data[k] = (words[i], words[j], words[i].Length * words[j].Length);
                k++;
            }
        }

        Array.Sort(data, (item1, item2) => item1.Product - item2.Product);

        for (var i = data.Length - 1; i >= 0; i--)
        {
            var letters1 = new HashSet<char>(data[i].Word1);
            var letters2 = new HashSet<char>(data[i].Word2);

            letters1.IntersectWith(letters2);
            if (letters1.Count == 0)
                return data[i].Product;
        }

        return 0;
    }

    private int MaxProduct_Optimized(string[] words)
    {
        if (words == null || words.Length < 2)
            return 0;

        // Time: O(N^2)
        // Space: O(N)
        // Implemented after peeking into Discussions section. I thought about bit manipulation by myself, but didn't think that it would be enough
        // because algorithm is still O(N^2).

        // 26-bit masks that determine which letters are present in each word
        var masks = new int[words.Length];
        for (var i = 0; i < masks.Length; i++)
        {
            foreach (var ch in words[i])
            {
                System.Diagnostics.Debug.Assert(ch >= 'a' && ch <= 'z');
                masks[i] |= 1 << (ch - 'a');
            }
        }

        var result = 0;
        for (var i = 0; i < masks.Length; i++)
        {
            for (var j = i + 1; j < masks.Length; j++)
            {
                if ((masks[i] & masks[j]) == 0)
                    result = Math.Max(result, words[i].Length * words[j].Length);
            }
        }

        return result;
    }
}
