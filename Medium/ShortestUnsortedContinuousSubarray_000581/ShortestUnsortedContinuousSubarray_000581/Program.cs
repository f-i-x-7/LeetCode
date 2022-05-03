// https://leetcode.com/problems/shortest-unsorted-continuous-subarray/

//Given an integer array nums, you need to find one continuous subarray that if you only sort this subarray in ascending order, then the whole array will be sorted in ascending order.

//Return the shortest such subarray and output its length.

//Example 1:
//Input: nums = [2, 6, 4, 8, 10, 9, 15]
//Output: 5
//Explanation: You need to sort[6, 4, 8, 10, 9] in ascending order to make the whole array sorted in ascending order.

//Example 2:
//Input: nums = [1, 2, 3, 4]
//Output: 0

//Example 3:
//Input: nums = [1]
//Output: 0

//Constraints:
//1 <= nums.length <= 10^4
//-10^5 <= nums[i] <= 10^5

//Follow up: Can you solve it in O(n) time complexity?



Test(new[] { 1 }, expectedResult: 0);
Test(new[] { 1, 2, 3, 4 }, expectedResult: 0);
Test(new[] { 2, 6, 4, 8, 10, 9, 15 }, expectedResult: 5); // [6, 4, 8, 10, 9]
Test(new[] { 2, 6, 8, 9, 10, 4, 15 }, expectedResult: 5); // [6, 8, 9, 10, 4]; brokenSortOrderStart=4; brokenSortOrderEnd=5

Test(new[] { 2, 1 }, expectedResult: 2);
Test(new[] { 3, 2, 1 }, expectedResult: 3);
Test(new[] { 4, 3, 2, 1 }, expectedResult: 4);
Test(new[] { 2, 1, 3 }, expectedResult: 2);
Test(new[] { 1, 3, 2 }, expectedResult: 2);

Test(new[] { 1, 3, 2, 2, 2 }, expectedResult: 4); // [3, 2, 2, 2]; fails without target index
Test(new[] { 2, 2, 1, 1, 3 }, expectedResult: 4); // [2, 2, 1, 1]
Test(new[] { 1, 3, 2, 3, 3 }, expectedResult: 2); // [3, 2]; fails with target index; brokenSortOrderStart=1; brokenSortOrderEnd=3
Test(new[] { 1, 3, 2, 4 }, expectedResult: 2); // [3, 2]; brokenSortOrderStart=1; brokenSortOrderEnd=3
Test(new[] { 1, 5, 2, 3, 4 }, expectedResult: 4); // [5, 2, 3, 4]; brokenSortOrderStart=1; brokenSortOrderEnd=2


static void Test(int[] nums, int expectedResult)
{
    var actualResult = new Solution().FindUnsortedSubarray(nums);

    if (expectedResult != actualResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {ToStr(nums)}; ExpectedResult={expectedResult}; ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {ToStr(nums)}; Result={expectedResult}");
    }


    static string ToStr(int[] arr) => '[' + string.Join(',', arr) + ']';
}

public class Solution
{
    public int FindUnsortedSubarray(int[] nums)
    {
        // O(N) time, O(1) space
        
        // 1) Iterate from start until sort order is broken (if not broken - array is sorted, return 0)
        // 2) Iterate from end until sort order is broken
        // 3) Indexes found at previous steps are lower bounds of subarray (actually it can be wider).
        //    Need to find subarray max and min values, and then find their target indices in sorted 'edges'.
        
        var brokenSortOrderStart = -1;
        for (var i = 0; i < nums.Length - 1; i++)
        {
            if (nums[i] > nums[i + 1])
            {
                brokenSortOrderStart = i;
                break;
            }
        }

        if (brokenSortOrderStart == -1)
            return 0; // array is already sorted

        var brokenSortOrderEnd = -1;
        for (var i = nums.Length - 1; i > 0; i--)
        {
            if (nums[i] < nums[i - 1])
            {
                brokenSortOrderEnd = i;
                break;
            }
        }

        // adjust end position - several items with same value are possible
        while (brokenSortOrderEnd < nums.Length - 1 && nums[brokenSortOrderEnd] == nums[brokenSortOrderEnd + 1])
        {
            brokenSortOrderEnd++;
        }

        System.Diagnostics.Debug.Assert(brokenSortOrderEnd > brokenSortOrderStart);

        if (brokenSortOrderStart == 0 && brokenSortOrderEnd == nums.Length - 1)
            return GetResult(brokenSortOrderStart, brokenSortOrderEnd);

        var min = int.MaxValue;
        var max = int.MinValue;

        for (var i = brokenSortOrderStart; i <= brokenSortOrderEnd; i++)
        {
            if (nums[i] < min)
                min = nums[i];
            if (nums[i] > max)
                max = nums[i];
        }

        var resultStart = brokenSortOrderStart;
        var resultEnd = brokenSortOrderEnd;

        if (resultStart != 0)
        {
            // NOTE: this part with binary search works for start bound, but for unification with end bound
            // linear scan logic is implemented.

            //var index = Array.BinarySearch(nums, 0, resultStart, min);
            //System.Diagnostics.Debug.Assert(index < 0);
            //index = ~index;

            // From docs:

            // If value is not found and value is less than one or more elements
            // in array, the negative number returned is the bitwise complement of the index
            // of the first element that is larger than value. If value is not found and value
            // is greater than all elements in array, the negative number returned is the bitwise
            // complement of (the index of the last element plus 1).

            //resultStart = index;

            for (var i = 0; i < brokenSortOrderStart; i++)
            {
                if (min < nums[i])
                {
                    resultStart = i;
                    break;
                }
            }
        }

        if (resultEnd != nums.Length - 1)
        {
            for (var i = nums.Length - 1; i > brokenSortOrderEnd; i--)
            {
                if (max > nums[i])
                {
                    resultEnd = i;
                    break;
                }
            }
        }


        return GetResult(resultStart, resultEnd);



        static int GetResult(int start, int end) => end - start + 1;
    }
}
