// https://leetcode.com/problems/kth-largest-element-in-a-stream/

//Design a class to find the kth largest element in a stream. Note that it is the kth largest element in the sorted order, not the kth distinct element.

//Implement KthLargest class:

//KthLargest(int k, int[] nums) Initializes the object with the integer k and the stream of integers nums.
//int add(int val) Appends the integer val to the stream and returns the element representing the kth largest element in the stream.


//Example 1:
//Input
//["KthLargest", "add", "add", "add", "add", "add"]
//[[3, [4, 5, 8, 2]], [3], [5], [10], [9], [4]]
//Output
//[null, 4, 5, 5, 8, 8]

//Explanation
//KthLargest kthLargest = new KthLargest(3, [4, 5, 8, 2]);
//kthLargest.add(3);   // return 4
//kthLargest.add(5);   // return 5
//kthLargest.add(10);  // return 5
//kthLargest.add(9);   // return 8
//kthLargest.add(4);   // return 8


//Constraints:
//1 <= k <= 10^4
//0 <= nums.length <= 10^4
//- 10^4 <= nums[i] <= 10^4
//- 10^4 <= val <= 10^4
//At most 10^4 calls will be made to add.
//It is guaranteed that there will be at least k elements in the array when you search for the kth element.


using FluentAssertions;

var kthLargest = new KthLargest_Heap(3, new[] { 4, 5, 8, 2 }); // sorted descending: [8, 5, 4, 2]
kthLargest.Add(3).Should().Be(4);
kthLargest.Add(5).Should().Be(5);
kthLargest.Add(10).Should().Be(5);
kthLargest.Add(9).Should().Be(8);
kthLargest.Add(4).Should().Be(8);
Console.WriteLine("SUCCEEDED CASE #1");

kthLargest = new KthLargest_Heap(1, new int[0]);
kthLargest.Add(3).Should().Be(3);
kthLargest.Add(3).Should().Be(3);
kthLargest.Add(4).Should().Be(4);
kthLargest.Add(3).Should().Be(4);
kthLargest.Add(10).Should().Be(10);
kthLargest.Add(1).Should().Be(10);
Console.WriteLine("SUCCEEDED CASE #2");


// List-based, O(N * logN + M * N) where M is number of calls to Add() method
public class KthLargest
{
    /// <summary>
    /// List of 'k' size that stores top k items in stream (it is updated each time during Add() method call).
    /// </summary>
    private readonly List<int> list;
    private readonly int k;

    public KthLargest(int k, int[] nums)
    {
        if (k <= 0)
            throw new ArgumentOutOfRangeException(nameof(k));
        if (nums is null)
            throw new ArgumentNullException(nameof(nums));
        if (k > nums.Length + 1)
            throw new ArgumentException($"k={k}; nums.Length={nums.Length}; nums=[{string.Join(',', nums)}]");

        this.k = k;

        Array.Sort(nums); // O(N * logN)

        // O(k) list initialization
        list = new List<int>(k);

        if (k > nums.Length)
        {
            list.AddRange(nums);
        }
        else
        {
            // More readable approach is to use LINQ (e.g. 'list.AddRange(nums.Skip(nums.Length - k).Take(k));');
            // but may be, if Skip() is not optimized for arrays, this would be actually O(N) time.
            // So use imperative approach and skip items manually.
            for (var i = nums.Length - k; i < nums.Length; i++)
            {
                list.Add(nums[i]);
            }
        }
    }

    // O(N) because of list manipulations (removal fron the front of list, insertion into the middle, etc.)
    public int Add(int val)
    {
        if (k > list.Count)
        {
            System.Diagnostics.Debug.Assert(k == list.Count + 1, "because gap of value 1 is only allowed");
            InsertIntoSortedList(val);
            return list[0];
        }

        var min = list[0];
        if (val <= min)
            return min;

        var max = list[list.Count - 1];
        if (val >= max)
        {
            list.RemoveAt(0);
            list.Add(val);
            return list[0];
        }

        // 'val' is somewhere in the middle of top k-th values.
        InsertIntoSortedList(val);
        list.RemoveAt(0);

        return list[0];



        void InsertIntoSortedList(int val)
        {
            var index = list.BinarySearch(val); // O(log k)

            //  Return result is:
            //  zero-based index of item in the sorted System.Collections.Generic.List<T>,
            //  if item is found; otherwise, a negative number that is the bitwise complement
            //  of the index of the next element that is larger than item or, if there is no
            //  larger element, the bitwise complement of System.Collections.Generic.List<T>.Count.
            if (index < 0)
            {
                // Such element does not exist, returned value is a bitwise complement of the index of the next element that is larger than passed value.
                index = ~index;

                // If list.Count < k then index here actually can have zero value.
                System.Diagnostics.Debug.Assert(list.Count < k || index > 0 && index < list.Count,
                    "index cannot be equal to list.Count because this means that there is no larger element in list, " +
                    "but this case already should be handled before");
            }

            if (index >= 0)
            {
                list.Insert(index, val);
            }
        }
    }
}

// When I thought abouth this task, I remembered heap, but I thought only about max heap of size N, and decided that it will not be useful actually
// (because of k * logN calls to pop items every time and then push again).
// Min heap solution is peeked from Leetcode official solution, this is much better than List-based solution.
public class KthLargest_Heap
{
    /// <summary>
    /// Min heap of size 'k', its top element is k-th maximum in stream, and it can be obtained in O(1).
    /// </summary>
    private readonly PriorityQueue<int, int> heap;
    private readonly int k;

    public KthLargest_Heap(int k, int[] nums)
    {
        if (k <= 0)
            throw new ArgumentOutOfRangeException(nameof(k));
        if (nums is null)
            throw new ArgumentNullException(nameof(nums));
        if (k > nums.Length + 1)
            throw new ArgumentException($"k={k}; nums.Length={nums.Length}; nums=[{string.Join(',', nums)}]");

        this.k = k;

        // Min heap with size 'k' initilization
        heap = new PriorityQueue<int, int>(k); // default comparer means min heap

        // Approach 1
        // Not very effective approach, O((2N-k) * logK) time to initialize heap:
        // Part 1 - fill with data, O(N * logk)
        //foreach (var num in nums)
        //{
        //    heap.Enqueue(num, num);
        //}
        // Part 2 - remove smalles elements, O((N-k) * logk)
        //while (heap.Count > k)
        //{
        //    heap.Dequeue();
        //}

        // Approach 2, O((min(N, k) + N - k)) * logK), if k << N this is way more efficient that approach #1
        // Part 1 - insert no more than 'k' elements, time complexity O(min(N, k) * logk)
        var countOfNumbersToPlaceInHeap = Math.Min(k, nums.Length);
        for (var i = 0; i < countOfNumbersToPlaceInHeap; i++)
        {
            heap.Enqueue(nums[i], nums[i]);
        }
        // Part 2 - add remaining elements if N > k, time complexity O((N-k) * logk)
        if (nums.Length > countOfNumbersToPlaceInHeap)
        {
            for (var i = countOfNumbersToPlaceInHeap; i < nums.Length; i++)
            {
                heap.EnqueueDequeue(nums[i], nums[i]);
            }
        }
    }

    // O(logk)
    public int Add(int val)
    {
        if (heap.Count == k)
        {
            heap.EnqueueDequeue(val, val);
        }
        else
        {
            System.Diagnostics.Debug.Assert(heap.Count == k - 1);
            heap.Enqueue(val, val);
        }

        return heap.Peek();
    }
}


/**
 * Your KthLargest object will be instantiated and called as such:
 * KthLargest obj = new KthLargest(k, nums);
 * int param_1 = obj.Add(val);
 */