// https://leetcode.com/problems/roman-to-integer/

//Roman numerals are represented by seven different symbols: I, V, X, L, C, D and M.

//Symbol       Value
//I             1
//V             5
//X             10
//L             50
//C             100
//D             500
//M             1000
//For example, 2 is written as II in Roman numeral, just two one's added together. 12 is written as XII, which is simply X + II. The number 27 is written as XXVII, which is XX + V + II.

//Roman numerals are usually written largest to smallest from left to right.
//However, the numeral for four is not IIII. Instead, the number four is written as IV.
//Because the one is before the five we subtract it making four. The same principle applies to the number nine, which is written as IX. There are six instances where subtraction is used:

// * I can be placed before V (5) and X(10) to make 4 and 9. 
// * X can be placed before L (50) and C(100) to make 40 and 90. 
// * C can be placed before D (500) and M(1000) to make 400 and 900.

//Given a roman numeral, convert it to an integer.

//Example 1:

//Input: s = "III"
//Output: 3
//Explanation: III = 3.

//Example 2:
//Input: s = "LVIII"
//Output: 58
//Explanation: L = 50, V = 5, III = 3.

// Example 3:
//Input: s = "MCMXCIV"
//Output: 1994
//Explanation: M = 1000, CM = 900, XC = 90 and IV = 4.


//Constraints:
//1 <= s.length <= 15
//s contains only the characters ('I', 'V', 'X', 'L', 'C', 'D', 'M').
//It is guaranteed that s is a valid roman numeral in the range [1, 3999].


Test("I", 1);
Test("II", 2);
Test("III", 3);
Test("IV", 4);
Test("V", 5);
Test("VII", 7);
Test("IX", 9);
Test("X", 10);
Test("XIII", 13);
Test("XIV", 14);
Test("XVII", 17);
Test("XIX", 19);
Test("XXXIX", 39);
Test("XL", 40);
Test("XLIX", 49);




void Test(string s, int expectedResult)
{
    var actualResult = Solution.RomanToInt(s);

    if (expectedResult != actualResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {s}; ExpectedResult={expectedResult}; ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {s}; Result={expectedResult}");
    }
}

public static class Solution
{
    public static int RomanToInt(string s)
    {
        if (string.IsNullOrEmpty(s))
            return -1;

        var result = 0;

        var previous = 0;
        for (var i = 0; i < s.Length; i++)
        {
            var current = RomanToInt(s[i]);
            if (previous != 0 && previous < current)
            {
                // IX, XL, etc.; this means previous char was part of two char combination, calculate value of this combination and contribute to result.
                // Ignore previous==0 in 'if' condition, this is required for 1st char & for cases when we are reading next char after two char combination
                result += current - previous;
                // Two char combination is finished, clear stored data (otherwise cases like IV will work incorrectly)
                previous = 0;
            }
            else
            {
                // current >= previous
                // This means that previous char is not part of two char combination, and previous char value contributes to result.
                result += previous;
                // Remember current char, may be we are in the beginning of two char combination
                previous = current;
            }
        }
        result += previous; // If last char was not part of two char combination

        return result;


        static int RomanToInt(char c) => c switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => int.MinValue
        };
    }
}