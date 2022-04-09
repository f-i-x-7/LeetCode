// https://leetcode.com/problems/top-k-frequent-elements/
//Given an integer array nums and an integer k, return the k most frequent elements. You may return the answer in any order.

//Example 1:
//Input: nums = [1, 1, 1, 2, 2, 3], k = 2
//Output:[1,2]

//Example 2:
//Input: nums = [1], k = 1
//Output:[1]

//Constraints:
//1 <= nums.length <= 10^5
//k is in the range[1, the number of unique elements in the array].
//It is guaranteed that the answer is unique.

//Follow up: Your algorithm's time complexity must be better than O(n log n), where n is the array's size.

Test(nums: new[] { 1, 1, 1, 2, 2, 3 }, k: 2, expectedResult: new[] { 1, 2 });
Test(nums: new[] { 1 }, k: 1, expectedResult: new[] { 1 });



static void Test(int[] nums, int k, int[] expectedResult)
{
    var numsOrdered = nums.OrderBy(x => x).ToArray();
    var actualResult = new Solution().TopKFrequent(nums, k).OrderBy(x => x).ToArray();

    if (!Enumerable.SequenceEqual(expectedResult, actualResult))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {ToStr(numsOrdered)}; K={k}; ExpectedResult={ToStr(expectedResult.OrderBy(x => x).ToArray())}; " +
            $"ActualResult={ToStr(actualResult)}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {ToStr(numsOrdered)}; K={k}; Result={ToStr(actualResult)}");
    }

    string ToStr(int[] arr) => $"[{string.Join(',', arr)}]";
}


public class Solution
{
    public int[] TopKFrequent(int[] nums, int k)
    {
        if (nums is null || nums.Length == 0 || k <= 0)
            return new int[0];

        return TopKFrequent_HashMapAndHeap1(nums, k);
    }

    private int[] TopKFrequent_HashMapAndHeap1(int[] nums, int k)
    {
        // Fill map, key is num and value is its frequency.
        // What is time complexity? O(N) iterations, O(1) average map insertion, but O(X) in worse case (where X is number of items in map)
        // Can it be considered as O(N^2)? I think no, it is better. Should be some way to approximate mathematicall and get time complexity?
        var map = new Dictionary<int, int>();
        for (var i = 0; i < nums.Length; i++)
        {
            var num = nums[i];
            map.TryGetValue(num, out var frequency);
            map[num] = frequency + 1;
        }

        // Use min heap of size K where priority is num frequency.
        // Time complexity is O(M * logK) where M is number of unique nums
        var heap = new PriorityQueue<int, int>(k);
        foreach (var (num, frequency) in map)
        {
            if (heap.Count < k)
                heap.Enqueue(element: num, priority: frequency);
            else
                heap.EnqueueDequeue(element: num, priority: frequency);
        }

        // Get all itemas from heap as unordered is O(K) operation?
        return heap.UnorderedItems.Select(x => x.Element).ToArray();
    }
}