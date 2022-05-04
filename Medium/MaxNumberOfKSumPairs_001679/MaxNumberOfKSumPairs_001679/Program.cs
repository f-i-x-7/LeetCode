// https://leetcode.com/problems/max-number-of-k-sum-pairs/

//You are given an integer array nums and an integer k.

//In one operation, you can pick two numbers from the array whose sum equals k and remove them from the array.

//Return the maximum number of operations you can perform on the array.

//Example 1:
//Input: nums = [1, 2, 3, 4], k = 5
//Output: 2
//Explanation: Starting with nums = [1, 2, 3, 4]:
//-Remove numbers 1 and 4, then nums = [2, 3]
//-Remove numbers 2 and 3, then nums = []
//There are no more pairs that sum up to 5, hence a total of 2 operations.

//Example 2:
//Input: nums = [3, 1, 3, 4, 3], k = 6
//Output: 1
//Explanation: Starting with nums = [3, 1, 3, 4, 3]:
//-Remove the first two 3's, then nums = [1,4,3]
//There are no more pairs that sum up to 6, hence a total of 1 operation.

//Constraints:
//1 <= nums.length <= 10^5
//1 <= nums[i] <= 10^9
//1 <= k <= 10^9

Console.WriteLine("Hello, World!");


public class Solution
{
    public int MaxOperations(int[] nums, int k) => MaxOperations_First(nums, k);

    public int MaxOperations_First(int[] nums, int k)
    {
        // O(N * logN) time, O(1) space
        Array.Sort(nums);

        var result = 0;
        var head = 0;
        var tail = nums.Length - 1;

        while (head < tail)
        {
            var sum = nums[head] + nums[tail];

            if (sum < k)
            {
                head++;
            }
            else if (sum > k)
            {
                tail--;
            }
            else if (sum == k)
            {
                result++;
                // Advance both pointers - simulate that both items are removed from array.
                head++;
                tail--;
            }
        }

        return result;
    }

    public int MaxOperations_Second(int[] nums, int k)
    {
        // O(N) time, O(N) space
        // Implemented after reading hints at Leetcode.

        var numsCount = new Dictionary<int, int>();
        for (var i = 0; i < nums.Length; i++)
        {
            var num = nums[i];
            numsCount.TryGetValue(num, out var count);
            numsCount[num] = ++count;
        }

        var result = 0;
        foreach (var num in numsCount.Keys)
        {
            var count = numsCount[num];
            var complement = k - num;
            if (complement == num)
            {
                result += count / 2;
            }
            else
            {
                if (numsCount.TryGetValue(complement, out var complementCount))
                {
                    var numResult = Math.Min(count, complementCount);
                    result += numResult;

                    numsCount[num] -= numResult;
                    numsCount[complement] -= numResult;
                }
            }
        }

        return result;
    }
}