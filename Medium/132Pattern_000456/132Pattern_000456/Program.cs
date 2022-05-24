// https://leetcode.com/problems/132-pattern/

//Given an array of n integers nums, a 132 pattern is a subsequence of three integers nums[i], nums[j] and nums[k] such that i < j < k and nums[i] < nums[k] < nums[j].

//Return true if there is a 132 pattern in nums, otherwise, return false.

//Example 1:
//Input: nums = [1, 2, 3, 4]
//Output: false
//Explanation: There is no 132 pattern in the sequence.

//Example 2:
//Input: nums = [3, 1, 4, 2]
//Output: true
//Explanation: There is a 132 pattern in the sequence: [1, 4, 2].

//Example 3:
//Input: nums = [-1, 3, 2, 0]
//Output: true
//Explanation: There are three 132 patterns in the sequence: [-1, 3, 2], [-1, 3, 0] and[-1, 2, 0].

//Constraints:
//n == nums.length
//1 <= n <= 2 * 10^5
//-10^9 <= nums[i] <= 10^9

Test(new[] { 1, 2, 3, 4 }, expectedResult: false);
Test(new[] { 3, 1, 4, 2 }, expectedResult: true);
Test(new[] { -1, 3, 2, 0 }, expectedResult: true);
Test(new[] { 42, 43, 6, 12, 3, 4, 6, 11, 20 }, expectedResult: true); // [6,12,11]



static void Test(int[] nums, bool expectedResult)
{
    var actualResult = new Solution().Find132pattern(nums);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ToStr(nums)}; ExpectedResult={expectedResult}; ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {ToStr(nums)}; Result={expectedResult}");
    }

    string ToStr(int[] nums) => '[' + string.Join(',', nums) + ']';
}


public class Solution
{
    public bool Find132pattern(int[] nums)
    {
        return Find132pattern_BetterBruteForce(nums);
    }

    public bool Find132pattern_Naive(int[] nums)
    {
        if (nums.Length < 3)
            return false;

        // O(N^3) time, O(1) space
        // Time limit is exceeded
        for (var i = 0; i < nums.Length - 2; i++)
        {
            for (var j = i + 1; j < nums.Length - 1; j++)
            {
                if (nums[i] >= nums[j])
                    continue;

                // nums[i] < nums[j]

                for (var k = j + 1; k < nums.Length; k++)
                {
                    if (nums[i] < nums[k] && nums[k] < nums[j])
                        return true;
                }
            }
        }

        return false;
    }

    public bool Find132pattern_BetterBruteForce(int[] nums)
    {
        if (nums.Length < 3)
            return false;

        // A bit changed 'Better brute force' approach from official Leetcode solution.
        // We only move j and k pointers, and for each j we try to use the minimum possible nums[i] (so that to maximize possibility of finding right k).

        // Time: O(N^2), space: O(1)
        // Time limit is still exceeded :(

        var minIthNum = int.MaxValue;

        for (var j = 1; j < nums.Length - 1; j++)
        {
            minIthNum = Math.Min(minIthNum, nums[j - 1]);

            if (minIthNum >= nums[j]) // ensure that nums[i] < nums[j]
                continue;

            for (var k = j + 1; k < nums.Length; k++)
            {
                if (nums[j] > nums[k] && minIthNum < nums[k])
                    return true;
            }
        }

        return false;
    }

    public bool Find132pattern_AnotherApproach(int[] nums)
    {
        if (nums.Length < 3)
            return false;

        // O(N^2) time, O(N) space

        // 1st step - O(N) time
        var numsSorted = new (int Num, int Index)[nums.Length];
        for (var i = 0; i < nums.Length; i++)
        {
            numsSorted[i] = (Num: nums[i], Index: i);
        }

        // 2nd step - O(N * logN) time
        Array.Sort(numsSorted, (x, y) =>
        {
            if (x.Num < y.Num)
                return -1;
            if (x.Num > y.Num)
                return 1;

            // For equal items preserve initial order
            return x.Index < y.Index ? - 1 : 1;
        });

        // 3rd step - O(N^2) time
        if (Check(moveHead: true, numsSorted))
            return true;

        return Check(moveHead: false, numsSorted);
        


        static bool Check(bool moveHead, (int Num, int Index)[] numsSorted)
        {
            var (head, tail) = (0, numsSorted.Length - 1);
            while (head < tail - 1) // leave at least one item between head and tail
            {
                var (headItem, tailItem) = (numsSorted[head], numsSorted[tail]);
                if (headItem.Index < tailItem.Index && headItem.Num < tailItem.Num) // check nums too because nums could be equal; and check them after index because this condition is more likely to be true
                {
                    // headItem.Index is i, tailItem.Index is j; with succeeded checks above, we ensure that i < j and nums[i] < nums[j]
                    for (var third = head + 1; third < tail; third++)
                    {
                        var thirdItem = numsSorted[third];

                        // thirdItem.Index is k
                        if (thirdItem.Index > tailItem.Index && headItem.Num < thirdItem.Num && thirdItem.Num < tailItem.Num) // check nums too, logic is the same as above
                        {
                            // here we ensure that k > j (i.e. i < j < k) and nums[i] < nums[k] < nums[j]
                            return true;
                        }
                    }
                }

                // TODO: what to move?
                // We should increase probability of picking right group of elements.
                // So it is better to move head when Index is decreasing (thus reducing i)
                // 

                if (moveHead)
                    head++;
                else
                    tail--;
            }
            return false;
        }
    }
}