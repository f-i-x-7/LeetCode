// https://leetcode.com/problems/ransom-note/

//Given two strings 'ransomNote' and 'magazine', return true if 'ransomNote' can be constructed from 'magazine' and false otherwise.

//Each letter in 'magazine' can only be used once in 'ransomNote'.


//Example 1:
//Input: ransomNote = "a", magazine = "b"
//Output: false

//Example 2:
//Input: ransomNote = "aa", magazine = "ab"
//Output: false

//Example 3:
//Input: ransomNote = "aa", magazine = "aab"
//Output: true


//Constraints:

//1 <= ransomNote.length, magazine.length <= 10&5
//ransomNote and magazine consist of lowercase English letters.







Console.WriteLine("Hello, World!");

public static class Solution
{
    public static bool CanConstruct(string ransomNote, string magazine) => CanConstruct_HashMaps(ransomNote, magazine);

    public static bool CanConstruct_HashMaps(string ransomNote, string magazine)
    {
        if (string.IsNullOrEmpty(ransomNote) || string.IsNullOrEmpty(magazine) || ransomNote.Length > magazine.Length)
            return false;

        var ransomNoteLetters = ransomNote.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        var magazineLetters = magazine.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        if (ransomNoteLetters.Count > magazineLetters.Count)
            return false;

        foreach (var (letter, countInRansomeNote) in ransomNoteLetters)
        {
            if (!magazineLetters.TryGetValue(letter, out var countInMagazine) || countInRansomeNote > countInMagazine)
                return false;
        }

        return true;
    }

    public static bool CanConstruct_Arrays(string ransomNote, string magazine)
    {
        if (string.IsNullOrEmpty(ransomNote) || string.IsNullOrEmpty(magazine) || ransomNote.Length > magazine.Length)
            return false;

        // NOTE: after implementing above solution with hash maps, I've peeked Discussions section at Leetcode and noted more performant (but a bit less readable) approach:
        // use 2 arrays with length of 26 items (by number of English alphabet letters), use arr[i] as counter of i-th letter; 'i' can be easily calculated as '(int)ch - 97' (use ASCII codes).

        var ransomNoteLetters = new int[26];
        foreach (var c in ransomNote)
        {
            ransomNoteLetters[GetEnglishLetterIndex(c)]++;
        }
        var magazineLetters = new int[26];
        foreach (var c in magazine)
        {
            magazineLetters[GetEnglishLetterIndex(c)]++;
        }

        for (var i = 0; i < ransomNoteLetters.Length; i++)
        {
            if (ransomNoteLetters[i] > magazineLetters[i])
                return false;
        }

        return true;


        static int GetEnglishLetterIndex(char c) => c - 'a';
    }
}