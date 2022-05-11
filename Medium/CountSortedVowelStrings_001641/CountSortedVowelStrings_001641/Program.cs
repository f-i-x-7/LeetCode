// https://leetcode.com/problems/count-sorted-vowel-strings/

//Given an integer n, return the number of strings of length n that consist only of vowels (a, e, i, o, u) and are lexicographically sorted.

//A string s is lexicographically sorted if for all valid i, s[i] is the same as or comes before s[i+1] in the alphabet.

//Example 1:
//Input: n = 1
//Output: 5
//Explanation: The 5 sorted strings that consist of vowels only are ["a","e","i","o","u"].

//Example 2:
//Input: n = 2
//Output: 15
//Explanation: The 15 sorted strings that consist of vowels only are
//["aa","ae","ai","ao","au","ee","ei","eo","eu","ii","io","iu","oo","ou","uu"].
//Note that "ea" is not a valid string since 'e' comes after 'a' in the alphabet.

//Example 3:
//Input: n = 33
//Output: 66045

//Constraints:
//1 <= n <= 50


Console.WriteLine("Hello, World!");



public class Solution
{
    // They are lexicographically sorted, it is taken into account in algorithm implementation.
    private static readonly char[] Vowels = new[] { 'a', 'e', 'i', 'o', 'u' };

    private static int GetCharIndex(char c) => c switch
    {
        'a' => 0,
        'e' => 1,
        'i' => 2,
        'o' => 3,
        'u' => 4,
        default(char) => 0, // in the beginning of calculations, when string is not started to be built
        _ => -1
    };

    public int CountVowelStrings(int n)
    {
        if (n <= 0 || n > 50)
            return -1;

        // Pure bute force:
        // Create all possible strings, then iterate over them, find whether they are sorted in lexicographical order,
        // and increment counter if that is true.
        // If the set S has n elements, the number of k-tuples over S is n^k.
        // So in our case, we have 5^n total strings. For n=50, that is very much (8.8817841970013 x 10^34).

        // So it is better to reduce amount of possible strings, considering lexicographical order on the fly.
        // Time complexity is still O(5^N).
        // Space complexity is O(5^N) too (for call stack).


        var counter = 0;
        Process(ref counter, 0, default(char));
        return counter;


        void Process(ref int counter, int charsInString, char previousChar)
        {
            if (charsInString == n)
            {
                counter++;
                return;
            }

            var previousCharIndex = GetCharIndex(previousChar);

            for (var i = previousCharIndex; i < Vowels.Length; i++)
            {
                var nextChar = Vowels[i];
                Process(ref counter, charsInString + 1, nextChar);
            }
        }
    }
}