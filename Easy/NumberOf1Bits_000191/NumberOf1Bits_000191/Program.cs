// https://leetcode.com/problems/number-of-1-bits/

//Write a function that takes an unsigned integer and returns the number of '1' bits it has (also known as the Hamming weight).

//Example 1:
//Input: n = 00000000000000000000000000001011
//Output: 3
//Explanation: The input binary string 00000000000000000000000000001011 has a total of three '1' bits.

//Example 2:
//Input: n = 00000000000000000000000010000000
//Output: 1
//Explanation: The input binary string 00000000000000000000000010000000 has a total of one '1' bit.

//Example 3:
//Input: n = 11111111111111111111111111111101
//Output: 31
//Explanation: The input binary string 11111111111111111111111111111101 has a total of thirty one '1' bits.

//Constraints:
//The input must be a binary string of length 32.


//Follow up: If this function is called many times, how would you optimize it?


public class Solution
{
    public int HammingWeight(uint n) => HammingWeight_BitwiseShift(n);

    private int HammingWeight_BitwiseShift(uint n)
    {
        var result = 0;
        for (var i = 0; i < 32; i++)
        {
            if (n % 2 == 1)
                result++;
            n >>= 1;
        }
        return result;
    }

    private static readonly uint[] Masks = new uint[]
    {
        0x00000001,
        0x00000002,
        0x00000004,
        0x00000008,

        0x00000010,
        0x00000020,
        0x00000040,
        0x00000080,

        0x00000100,
        0x00000200,
        0x00000400,
        0x00000800,

        0x00001000,
        0x00002000,
        0x00004000,
        0x00008000,

        0x00010000,
        0x00020000,
        0x00040000,
        0x00080000,

        0x00100000,
        0x00200000,
        0x00400000,
        0x00800000,

        0x01000000,
        0x02000000,
        0x04000000,
        0x08000000,

        0x10000000,
        0x20000000,
        0x40000000,
        0x80000000
    };

    private int HammingWeight_Masks(uint n)
    {
        var result = 0;
        for (var i = 0; i < Masks.Length; i++)
        {
            if ((n & Masks[i]) != 0)
                result++;
        }
        return result;
    }

    private int HammingWeight_BrianKernighan(uint n)
    {
        //Subtracting 1 from a decimal number flips all the bits after the rightmost set bit (which is 1) including the rightmost set bit.
        //for example : 
        //10 in binary is 00001010
        //9 in binary is 00001001
        //8 in binary is 00001000
        //7 in binary is 00000111
        //So if we subtract a number by 1 and do it bitwise &with itself(n & (n - 1)), we unset the rightmost set bit. If we do n & (n - 1) in a loop and count the number of times the loop executes, we get the set bit count. 
        //The beauty of this solution is the number of times it loops is equal to the number of set bits in a given integer.
        var result = 0;
        while (n != 0)
        {
            result++;
            n = n & (n - 1);
        }
        return result;
    }
}
