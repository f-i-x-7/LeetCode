// https://leetcode.com/problems/divide-two-integers/

//Given two integers dividend and divisor, divide two integers without using multiplication, division, and mod operator.

//The integer division should truncate toward zero, which means losing its fractional part. For example, 8.345 would be truncated to 8, and -2.7335 would be truncated to -2.

//Return the quotient after dividing dividend by divisor.

//Note: Assume we are dealing with an environment that could only store integers within the 32-bit signed integer range: [−231, 231 − 1]. For this problem, if the quotient is strictly greater than 231 - 1, then return 231 - 1, and if the quotient is strictly less than -231, then return -231.

//Example 1:
//Input: dividend = 10, divisor = 3
//Output: 3
//Explanation: 10/3 = 3.33333.. which is truncated to 3.

//Example 2:
//Input: dividend = 7, divisor = -3
//Output: -2
//Explanation: 7/-3 = -2.33333.. which is truncated to -2.

//Constraints:
//-2^31 <= dividend, divisor <= 2^31 - 1
//divisor ! = 0


Test(dividend: 10, divisor: 3, expectedResult: 3);
Test(dividend: 7, divisor: -3, expectedResult: -2);
Test(dividend: 1, divisor: 1, expectedResult: 1);
Test(dividend: -10, divisor: -3, expectedResult: 3);
Test(dividend: 10, divisor: 4, expectedResult: 2);
Test(dividend: 100, divisor: 16, expectedResult: 6);

Test(dividend: -2147483648, divisor: -1, expectedResult: 2147483647);
Test(dividend: -2147483648, divisor: 1, expectedResult: -2147483648);
Test(dividend: 2147483647, divisor: -1, expectedResult: -2147483647);
Test(dividend: 2147483647, divisor: 1, expectedResult: 2147483647);

Test(dividend: -2147483648, divisor: -2, expectedResult: 1073741824);
Test(dividend: -2147483648, divisor: 2, expectedResult: -1073741824);
Test(dividend: 2147483647, divisor: -2, expectedResult: -1073741823);
Test(dividend: 2147483647, divisor: 2, expectedResult: 1073741823);

Test(dividend: -2147483648, divisor: -2147483648, expectedResult: 1);
Test(dividend: -2147483648, divisor: 2147483647, expectedResult: -1);
Test(dividend: 2147483647, divisor: -2147483648, expectedResult: 0);
Test(dividend: 2147483647, divisor: 2147483647, expectedResult: 1);

Test(dividend: -2147483648, divisor: -3, expectedResult: 715827882);


void Test(int dividend, int divisor, int expectedResult)
{
    var actualResult = new Solution().Divide(dividend, divisor);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {dividend} / {divisor} should be {expectedResult}, but was {actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {dividend} / {divisor} = {actualResult}");
    }
}




public class Solution
{
    public int Divide(int dividend, int divisor)
    {
        // Not as optimized solution as in https://leetcode.com/problems/divide-two-integers/discuss/2089533/Easy-Solution-in-C%2B%2B
        // But still managed to break Time Limie Exceeded error after introduction of optimization for power of 2 divisors.

        if (dividend == 0 || divisor == 0)
            return 0;

        // Hot paths for 1 & -1 divisor - prevent looping
        if (divisor == 1)
            return dividend;
        if (divisor == -1)
            return dividend == int.MinValue ? int.MaxValue : -1 * dividend;

        // Special case for int.MinValue divisor
        if (divisor == int.MinValue)
            return dividend == int.MinValue ? 1 : 0;

        var sign = 1;
        var needAddOne = false;
        if (divisor < 0)
        {
            sign *= -1;

            if (divisor == int.MinValue)
                divisor = int.MaxValue;
            else
                divisor *= -1;
        }

        // Hot-path when divisor is a power of 2
        var divisorLog2 = Math.Log2(divisor);
        if (divisorLog2 == (int)divisorLog2)
        {
            return sign * (dividend >> ((int)divisorLog2));
        }

        if (dividend < 0)
        {
            sign *= -1;

            if (dividend == int.MinValue)
            {
                dividend = int.MaxValue;
                needAddOne = true;
            }
            else
                dividend *= -1;
        }

        var result = 0;
        while (dividend >= divisor)
        {
            dividend -= divisor;
            result++;
            if (needAddOne)
            {
                dividend++;
                needAddOne = false;
            }
        }
        return sign * result;
    }
}