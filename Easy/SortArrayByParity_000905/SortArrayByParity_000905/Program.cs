// https://leetcode.com/problems/sort-array-by-parity/

//Given an integer array nums, move all the even integers at the beginning of the array followed by all the odd integers.

//Return any array that satisfies this condition.

//Example 1:
//Input: nums = [3, 1, 2, 4]
//Output: [2,4,3,1]
//Explanation: The outputs[4, 2, 3, 1], [2, 4, 1, 3], and[4, 2, 1, 3] would also be accepted.

//Example 2:
//Input: nums = [0]
//Output: [0]

//Constraints:
//1 <= nums.length <= 5000
//0 <= nums[i] <= 5000




Console.WriteLine("Hello, World!");


public class Solution
{
    public int[] SortArrayByParity(int[] nums)
    {
        // O(N) time, O(1) space; use two pointers, iterate from head and tail and check whether each element is on its correct position
        var head = 0;
        var tail = nums.Length - 1;

        while (head < tail)
        {
            var isHeadCorrect = IsEven(nums[head]);
            var isTailCorrect = IsOdd(nums[tail]);

            if (isHeadCorrect && isTailCorrect)
            {
                head++;
                tail--;
            }
            else if (!isHeadCorrect && !isTailCorrect)
            {
                Swap(head, tail);
                head++;
                tail--;
            }
            else if (isHeadCorrect)
            {
                // head is correct (even), tail is not.
                // Move head further - may be next position will lead to invalid head and items will be swapped.
                head++;
            }
            else
            {
                // tail is correct (odd), head is not.
                // Similar to previous case
                tail--;
            }
        }

        return nums;


        void Swap(int i, int j)
        {
            var tmp = nums[i];
            nums[i] = nums[j];
            nums[j] = tmp;
        }

        static bool IsOdd(int n) => n % 2 == 1;
        static bool IsEven(int n) => n % 2 == 0;
    }
}