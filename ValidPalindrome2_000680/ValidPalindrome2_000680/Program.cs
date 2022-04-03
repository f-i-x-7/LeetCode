// https://leetcode.com/problems/valid-palindrome-ii/

// Given a string s, return true if the s can be palindrome after deleting at most one character from it.


using System.Diagnostics;

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

// After leetcode check
// Some failed string from leetcode. When it is up to removing one character, it seems at first that both char from front and from back can be removed.
PrintResult("aguokepatgbnvfqmgmlcupuufxoohdfpgjdmysgvhmvffcnqxjjxqncffvmhvgsymdjgpfdhooxfuupuculmgmqfvnbgtapekouga", true);
// Simplified version of previous string. Actually 'u' from back actually should be removed (the one before last char),
// but technically at this position s[1] ('c') can be removed too (but this will lead to fail later).
PrintResult("mcupuuxjjxuupucum", true);




static void PrintResult(string s, bool expectedResult)
{
    var actualResult = ValidPalindrome(s);

    if (actualResult != expectedResult)
        Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine((actualResult == expectedResult ? "" : "ERROR! ") + s + " = " + actualResult);

    if (actualResult != expectedResult)
        Console.ResetColor();
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
    int? forkIndexWhereFrontCharWasRemoved = null;

    for (var i = 0; i < midIndex; i++)
    {
        var j = s.Length - 1 - i;
        if (s[i + frontIndexOffset] == s[j - backIndexOffset])
            continue;

        // Mismatch found.

        if (oneCharRemoved)
        {
            if (forkIndexWhereFrontCharWasRemoved != null)
            {
                // Previously there was a fork, and we removed char from front. But this strategy didn't succeed.
                // Return back to fork position, try to remove back char and proceed.
                Debug.Assert(frontIndexOffset == 1 && backIndexOffset == 0);

                frontIndexOffset--;
                backIndexOffset++;
                forkIndexWhereFrontCharWasRemoved = null;
                continue;
            }

            // If one character was already removed and there were no fork previously (or both its ways were observed) - answer is false.
            return false;
        }

        oneCharRemoved = true;

        Debug.Assert(frontIndexOffset == 0 && backIndexOffset == 0);

        // Skip one character either from beginning of string or from end of string and recheck.
        // FIXME: need to fix case when both checks at current stage will succeed.
        var canRemoveFrontChar = s[i + 1] == s[j];
        var canRemoveBackChar = s[i] == s[j - 1];

        if (canRemoveFrontChar)
        {
            if (canRemoveBackChar)
            {
                // Fork case: at this point, both chars can be removed.
                // Try to remove front char, but if later one more mismatch occurs - return to here, remove back char and proceed.
                forkIndexWhereFrontCharWasRemoved = i;
            }

            frontIndexOffset++;
            continue;
        }

        if (canRemoveBackChar)
        {
            backIndexOffset++;
            continue;
        }

        // Mismatch found after skipping one character - answer is false.
        return false;
    }

    return true;
}