// https://leetcode.com/problems/missing-number/

//Given an array nums containing n distinct numbers in the range [0, n], return the only number in the range that is missing from the array.

//Example 1:
//Input: nums = [3, 0, 1]
//Output: 2
//Explanation: n = 3 since there are 3 numbers, so all numbers are in the range [0,3]. 2 is the missing number in the range since it does not appear in nums.

//Example 2:
//Input: nums = [0, 1]
//Output: 2
//Explanation: n = 2 since there are 2 numbers, so all numbers are in the range [0,2]. 2 is the missing number in the range since it does not appear in nums.

//Example 3:
//Input: nums = [9, 6, 4, 2, 3, 5, 7, 0, 1]
//Output: 8
//Explanation: n = 9 since there are 9 numbers, so all numbers are in the range [0,9]. 8 is the missing number in the range since it does not appear in nums.

//Constraints:
//n == nums.length
//1 <= n <= 10^4
//0 <= nums[i] <= n
//All the numbers of nums are unique.

//Follow up: Could you implement a solution using only O(1) extra space complexity and O(n) runtime complexity?

Console.WriteLine("Hello, World!");


public class Solution
{
    public int MissingNumber(int[] nums)
    {
        return MissingNumber_Approach1(nums);
    }

    private int MissingNumber_Approach1(int[] nums)
    {
        if (nums is null || nums.Length == 0)
            return -1;

        // Time: O(N)
        // Space: O(N)
        var n = nums.Length;
        var allNums = new HashSet<int>(Enumerable.Range(0, n + 1));
        foreach (var num in nums)
        {
            var removed = allNums.Remove(num);
            System.Diagnostics.Debug.Assert(removed);
        }
        System.Diagnostics.Debug.Assert(allNums.Count == 1);
        return allNums.First();
    }

    private int MissingNumber_Approach2(int[] nums)
    {
        if (nums is null || nums.Length == 0)
            return -1;

        // Time: O(N * logN)
        // Space: O(1)

        // Only applicable if array can be modified (in-place sorting occurs)
        Array.Sort(nums);
        var n = nums.Length;
        for (var i = 0; i < n; i++)
        {
            if (nums[i] != i)
                return i;
        }
        return n;
    }

    private int MissingNumber_Approach3(int[] nums)
    {
        if (nums is null || nums.Length == 0)
            return -1;

        // Time: O(N)
        // Space: O(1)
        var n = nums.Length;
        var targetSum = Enumerable.Range(1, n).Sum();
        var actualSum = nums.Sum();
        System.Diagnostics.Debug.Assert(targetSum >= actualSum);
        return targetSum - actualSum;
    }
}
