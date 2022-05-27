// https://leetcode.com/problems/longest-increasing-subsequence/

//Given an integer array nums, return the length of the longest strictly increasing subsequence.

//A subsequence is a sequence that can be derived from an array by deleting some or no elements without changing the order of the remaining elements. For example, [3, 6, 2, 7] is a subsequence of the array [0,3,1,6,2,2,7].

//Example 1:
//Input: nums = [10, 9, 2, 5, 3, 7, 101, 18]
//Output: 4
//Explanation: The longest increasing subsequence is [2,3,7,101], therefore the length is 4.

//Example 2:
//Input: nums = [0, 1, 0, 3, 2, 3]
//Output: 4

//Example 3:
//Input: nums = [7, 7, 7, 7, 7, 7, 7]
//Output: 1

//Constraints:
//1 <= nums.length <= 2500
//-10^4 <= nums[i] <= 10^4

//Follow up: Can you come up with an algorithm that runs in O(n log(n)) time complexity?

new Solution().LengthOfLIS(new[] { 1, 3, 6, 7, 9, 4, 10, 5, 6 }); // 1,3,6,7,9,10 => expected answer is 6
//new Solution().LengthOfLIS(new[] { 10, 9, 2, 5, 3, 7, 101, 18 });
//new Solution().LengthOfLIS(new[] { 0, 1, 0, 3, 2, 3 });


public class Solution
{
    public int LengthOfLIS(int[] nums)
    {
        return LengthOfLIS_SubSquareTime(nums);
    }

    private int LengthOfLIS_SquareTime(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;

        // Time: O(N^2)
        // Space: O(N)
        
        // dp[i] is length of longest increasing sequence from nums[0] to nums[i] inclusively
        var dp = new int[nums.Length];
        dp[0] = 1;
        for (var i = 1; i < nums.Length; i++)
        {
            var previousMaxSubsequenceLength = 0;
            for (var j = 0; j < i; j++)
            {
                if (nums[i] > nums[j])
                    previousMaxSubsequenceLength = Math.Max(previousMaxSubsequenceLength, dp[j]);
            }
            dp[i] = previousMaxSubsequenceLength + 1;
        }
        return dp.Max();
    }

    private int LengthOfLIS_SubSquareTime(int[] nums)
    {
        if (nums == null || nums.Length == 0)
            return 0;

        // Time: O(N * logN)
        // Space: O(N)
        // Used a hint from community member's solution to another problem https://leetcode.com/problems/russian-doll-envelopes/discuss/2071477/Java-PythonBest-Explanation-with-Pictures
        // Hint states that array binary search should be used.

        var sorted = nums.ToArray(); // O(N) time
        Array.Sort(sorted); // O(N * logN) time

        // NOTE: technically we require only one array, as Array.BinarySearch() method has overload accepting IComparer,
        // so we can search indexes in sorted array using this array that stores both nums and initial indexes.
        // NOTE: such an array with initial indexes is required in order to do final search correctly
        // (if there is the same num as local maximum at then end, but it is located somewhere in the middle, it spoils everything)
        var sortedWithIndexes = nums.Select((n, i) => (Num: n, Index: i)).ToArray(); // O(N) time
        Array.Sort(sortedWithIndexes, (x, y) =>
        {
            var numsComparisonResult = x.Num - y.Num;
            if (numsComparisonResult != 0)
                return numsComparisonResult;
            return x.Index - y.Index;
        }); // O(N * logN) time

        // Find where decreasing sequence at the end of array starts, find local maximum.
        // O(N) time, O(1) space
        var indexOfLocalMaximumAtTheEndOfInitialArray = nums.Length - 1;
        for (var i = nums.Length - 2; i >= 0; i--)
        {
            if (nums[i] >= nums[i + 1])
                indexOfLocalMaximumAtTheEndOfInitialArray = i;
            else
                break;
        }

        // Find index of this item at sorted array.
        // O(logN) time, O(N) space
        var indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray = Array.BinarySearch(sorted, nums[indexOfLocalMaximumAtTheEndOfInitialArray]);
        System.Diagnostics.Debug.Assert(indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray >= 0); // actually this can be zero only if array is 1-element length
        System.Diagnostics.Debug.Assert(indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray < sorted.Length);

        // Now we should iterate initial array up to indexOfLocalMaximumAtTheEnd and for each item find its index in sorted array.
        // If it is lesser than indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray then remember it.
        // Find the minimum amongst all remembered indexes. Thus we will find the widest subsequence.
        // Then scan initial array and compute length of that subsequence.
        // O(N * logN) time, O(1) space
        var subsequenceStartIndexInSortedArray = int.MaxValue;
        for (var i = 0; i < indexOfLocalMaximumAtTheEndOfInitialArray; i++)
        {
            var indexInSortedArray = Array.BinarySearch(sorted, nums[i]);
            if (indexInSortedArray < indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray)
            {
                // This means that nums[i] < nums[indexOfLocalMaximumAtTheEndOfInitialArray], and such subsequence should be considered.
                subsequenceStartIndexInSortedArray = Math.Min(subsequenceStartIndexInSortedArray, indexInSortedArray);
            }
        }

        if (subsequenceStartIndexInSortedArray == int.MaxValue)
            return 1; // there is no increasing subsequence in initial array

        // FAILS AT 1ST TEST CASE! SUCCEEDS AT 3RD TEST CASE!

        // O(N) time, O(1) space
        //var subsequenceLength = 1;
        //var lastInitialIndex = sortedWithIndexes[subsequenceStartIndexInSortedArray].Index;
        //var lastNum = sortedWithIndexes[subsequenceStartIndexInSortedArray].Num;
        //for (var i = subsequenceStartIndexInSortedArray + 1; i <= indexInSortedArrayOfLocalMaximumAtTheEndOfInitialArray; i++)
        //{
        //    if (sortedWithIndexes[i].Index > lastInitialIndex && sortedWithIndexes[i].Num > lastNum)
        //    {
        //        subsequenceLength++;
        //        lastInitialIndex = sortedWithIndexes[i].Index;
        //        lastNum = sortedWithIndexes[i].Num;
        //    }
        //}


        // SUCCEEDS AT 1ST TEST CASE! FAILS AT 3RD TEST CASE!
        var subsequenceStart = sorted[subsequenceStartIndexInSortedArray];
        var subsequenceStartIndex = -1;
        for (var i = 0; i <= indexOfLocalMaximumAtTheEndOfInitialArray; i++)
        {
            if (nums[i] == subsequenceStart)
            {
                subsequenceStartIndex = i;
                break;
            }
        }

        var subsequenceLength = 1;
        var lastItemInSubsequence = subsequenceStart;
        for (var i = subsequenceStartIndex + 1; i <= indexOfLocalMaximumAtTheEndOfInitialArray; i++)
        {
            if (nums[i] > lastItemInSubsequence)
            {
                subsequenceLength++;
                lastItemInSubsequence = nums[i];
            }
        }

        return subsequenceLength;
    }
}