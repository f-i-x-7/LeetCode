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
        if (nums is null || nums.Length == 0 || k <= 0 || k > nums.Length)
            return new int[0];

        return TopKFrequent_HashMapAndHeap2(nums, k);
    }

    private int[] TopKFrequent_HashMapAndHeap1(int[] nums, int k)
    {
        var map = GetFrequencyMap(nums, k);

        // Use min heap of size K where priority is num frequency.
        // Total time complexity of heap manipulations is O(M * logK + K)
        // Time complexity is O(M * logK) where M is number of unique nums
        var heap = new PriorityQueue<int, int>(k);
        foreach (var (num, frequency) in map)
        {
            if (heap.Count < k)
                heap.Enqueue(element: num, priority: frequency);
            else
                heap.EnqueueDequeue(element: num, priority: frequency);
        }

        // Get all items from heap as unordered is O(K) operation?
        return heap.UnorderedItems.Select(x => x.Element).ToArray();
    }

    private int[] TopKFrequent_HashMapAndHeap2(int[] nums, int k)
    {
        // Fill map same way as in approach 1
        var map = GetFrequencyMap(nums, k);

        // Use min heap of size K where priority is num frequency.
        // In general, this is more optimized approach comparing to approach #1: O((M-K) * logK + 2 * K)
        // (may be it can perform worse if K << M, then (M-K) ~= M, and 2 * K in this approach can outweight K from 1st approach)
        // 1) Prefill heap with K items - O(K)
        var firstKHeapItems = map
            .Take(k)
            .Select(kv => (Num: kv.Key, Frequency: kv.Value))
            .ToArray(); // use ToArray() so that PriorityQueue can have access to number of items
        var heap = new PriorityQueue<int, int>(firstKHeapItems);

        // 2) Add additional items - O((M-K) * logK) where M is number of unique nums.
        foreach (var (num, frequency) in map.Skip(k))
        {
            heap.EnqueueDequeue(element: num, priority: frequency);
        }

        // Get all items from heap as unordered is O(K) operation?
        return heap.UnorderedItems.Select(x => x.Element).ToArray();
    }

    private Dictionary<int, int> GetFrequencyMap(int[] nums, int k)
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
        System.Diagnostics.Debug.Assert(k <= map.Count, "Task constraint is violated: K is greater than number of unique nums");
        return map;
    }
}