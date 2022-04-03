// https://leetcode.com/problems/two-sum/

// Given an array of integers nums and an integer target, return indices of the two numbers such that they add up to target.
// You may assume that each input would have exactly one solution, and you may not use the same element twice.
// You can return the answer in any order.
var array = new[] { 3, 3 };
var result = TwoSum(array, 6);
Console.WriteLine(result);



static int[] TwoSum(int[] nums, int target)
{
    return TwoSum_Impl2(nums, target);
}

static int[] TwoSum_Impl1(int[] nums, int target)
{
    // O(N^2) time, no additional memory
    for (int i = 0; i < nums.Length; i++)
    {
        for (int j = 0; j < nums.Length && i != j; j++)
        {
            if (nums[i] + nums[j] == target)
                return new[] { i, j };
        }
    }

    throw new Exception("No asnwer, but it should be according to task definition.");
}

static int[] TwoSum_Impl2(int[] nums, int target)
{
    // O(N * logN) amortized time, O(N) additional memory
    var sorted = new int[nums.Length];
    Array.Copy(nums, sorted, nums.Length); // O(N) time, O(N) memory
    Array.Sort(sorted); // assume that underlying sort algorithm is O(N * logN)

    // Next lines have amortized O(N * logN) time complexity
    for (int i = 0; i < sorted.Length; i++)
    {
        var num1 = sorted[i];
        var num2 = target - num1; // assume that num2 exists in array and num1 + num2 = target

        var indexOfNum2 = Array.BinarySearch(sorted, num2);
        // may be there is a possibility for slight optimization when indexOfNum2 = i? Didn't care for leetcode submission though
        if (indexOfNum2 >= 0 && indexOfNum2 != i) // yes, num2 exists in array, and it is different item
        {
            // need to find num1 and num2 indexes in initial array
            var indexes = IndexesOf(nums, num1, num2);
            return new[] { indexes.i1, indexes.i2 };
        }
    }

    //throw new Exception("No asnwer or possibly same indexes answer.");
    return new[] { -1, -1 };


    static (int i1, int i2) IndexesOf(int[] array, int item1, int item2)
    {
        var (i1, i2) = (-1, -1);

        for (int i = 0; i1 < 0 || i2 < 0; i++)
        {
            if (i1 < 0 && array[i] == item1)
                i1 = i;
            if (i2 < 0 && array[i] == item2 && i1 != i) // o not return same indexes if item1 == item2
                i2 = i;
        }

        return (i1, i2);
    }
}