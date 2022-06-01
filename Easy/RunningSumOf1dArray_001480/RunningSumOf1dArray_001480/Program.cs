// https://leetcode.com/problems/running-sum-of-1d-array/

//Given an array nums. We define a running sum of an array as runningSum[i] = sum(nums[0]…nums[i]).

//Return the running sum of nums.

//Example 1:
//Input: nums = [1, 2, 3, 4]
//Output: [1,3,6,10]
//Explanation: Running sum is obtained as follows: [1, 1+2, 1+2+3, 1+2+3+4].

//Example 2:
//Input: nums = [1, 1, 1, 1, 1]
//Output: [1,2,3,4,5]
//Explanation: Running sum is obtained as follows: [1, 1+1, 1+1+1, 1+1+1+1, 1+1+1+1+1].

//Example 3:
//Input: nums = [3, 1, 2, 10, 1]
//Output: [3,4,6,16,17]

//Constraints:

//1 <= nums.length <= 1000
//-10^  <= nums[i] <= 10^6

Console.WriteLine("Hello, World!");


public class Solution
{
    public int[] RunningSum(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return Array.Empty<int>();

        var result = new int[nums.Length];
        result[0] = nums[0];
        for (var i = 1; i < nums.Length; i++)
        {
            result[i] = nums[i] + result[i - 1];
        }
        return result;
    }
}