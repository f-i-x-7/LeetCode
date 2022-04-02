// https://leetcode.com/problems/valid-palindrome-ii/

// Given a string s, return true if the s can be palindrome after deleting at most one character from it.


Console.WriteLine("All same characters, odd & even length");
PrintResult("aaa", true);
PrintResult("aaaa", true);
PrintResult("aaaaa", true);

Console.WriteLine();
Console.WriteLine("Palindromes, odd & even length");
PrintResult("aba", true);
PrintResult("abba", true);
PrintResult("abcba", true);

Console.WriteLine();
Console.WriteLine("Definitely non-palindromes even when 1 character can be removed");
PrintResult("abc", false);
PrintResult("abcd", false);
PrintResult("abcda", false);

Console.WriteLine();
Console.WriteLine("Palindrome if 1 character is removed near the middle of string");
// Odd length
PrintResult("aac", true);
PrintResult("caa", true);
// Even length
PrintResult("aaca", true);
PrintResult("acaa", true);
PrintResult("abca", true);


Console.WriteLine();
Console.WriteLine("Palindrome if 1 character is removed NOT near the middle of string");

// Odd length, after one character removal it will be "abccba"
PrintResult("dabccba", true);
PrintResult("adbccba", true);
PrintResult("abdccba", true);
PrintResult("abccdba", true);
PrintResult("abccbda", true);
PrintResult("abccbad", true);

// Even length, after one character removal it will be "abcba"
PrintResult("dabcba", true);
PrintResult("adbcba", true);
PrintResult("abdcba", true);
PrintResult("abcdba", true);
PrintResult("abcbda", true);
PrintResult("abcbad", true);


// From leetcode check
PrintResult("aguokepatgbnvfqmgmlcupuufxoohdfpgjdmysgvhmvffcnqxjjxqncffvmhvgsymdjgpfdhooxfuupuculmgmqfvnbgtapekouga", true);



static void PrintResult(string s, bool expectedResult)
{
    var actualResult = ValidPalindrome(s);
    Console.WriteLine((actualResult == expectedResult ? "" : "ERROR! ") + s + " = " + actualResult);
}


static bool ValidPalindrome(string s)
{
    if (string.IsNullOrWhiteSpace(s))
        throw new ArgumentNullException(nameof(s));
    if (s.Length > 100_000)
        throw new ArgumentException("Too long string.", nameof(s));

    var midIndex = s.Length / 2;
    var oneCharRemoved = false;
    var frontIndexOffset = 0;
    var backIndexOffset = 0;

    for (var i = 0; i < midIndex; i++)
    {
        var j = s.Length - 1 - i - backIndexOffset;
        if (s[i + frontIndexOffset] == s[j])
            continue;

        // Mismatch found.

        // If one character was already removed - answer is false.
        if (oneCharRemoved)
            return false;

        oneCharRemoved = true;

        // Skip one character either from beginning of string or from end of string and recheck.
        // FIXME: need to fix case when both checks at current stage will succeed.
        if (s[i + 1] == s[j])
        {
            frontIndexOffset++;
            continue;
        }

        if (s[i] == s[j - 1])
        {
            backIndexOffset++;
            continue;
        }

        // Mismatch found after skipping one character - asnwer is false.
        return false;
    }

    return true;
}