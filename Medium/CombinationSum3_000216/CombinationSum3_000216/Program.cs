// https://leetcode.com/problems/combination-sum-iii/

//Find all valid combinations of k numbers that sum up to n such that the following conditions are true:

//Only numbers 1 through 9 are used.
//Each number is used at most once.
//Return a list of all possible valid combinations. The list must not contain the same combination twice, and the combinations may be returned in any order.

//Example 1:
//Input: k = 3, n = 7
//Output: [[1,2,4]]
//Explanation:
//1 + 2 + 4 = 7
//There are no other valid combinations.

//Example 2:
//Input: k = 3, n = 9
//Output: [[1,2,6],[1,3,5],[2,3,4]]
//Explanation:
//1 + 2 + 6 = 9
//1 + 3 + 5 = 9
//2 + 3 + 4 = 9
//There are no other valid combinations.

//Example 3:
//Input: k = 4, n = 1
//Output: []
//Explanation: There are no valid combinations.
//Using 4 different numbers in the range [1,9], the smallest sum we can get is 1+2+3+4 = 10 and since 10 > 1, there are no valid combination.

//Constraints:

//2 <= k <= 9
//1 <= n <= 60


var result = new Solution().CombinationSum3(4, 1);
Console.WriteLine(result);


public class Solution
{
    private static readonly int[] digits = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public IList<IList<int>> CombinationSum3(int k, int n)
    {
        if (k <= 1 || k > 9 || n <= 0 || n > 60)
            return Array.Empty<int[]>();

        return CombinationSum3_BruteForce(k, n);
    }

    public IList<IList<int>> CombinationSum3_BruteForce(int k, int n)
    {
        // Brute force all k combinations from 9 items, then filter them
        // Total combinations: Cnk = 9! / (k! * (9 - k)!)
        // Time complexity: O(C(9,k))?
        // k = 2 or 7: Cnk = 9! / (2! * 7!) = 8 * 9 / 2 = 36
        // k = 3 or 6: Cnk = 9! / (3! * 6!) = 7 * 8 * 9 / 6 = 7 * 4 * 3 = 84
        // k = 4 or 5: Cnk = 9! / (4! * 5!) = 6 * 7 * 8 * 9 / 24 = 6 * 7 * 3 = 126
        // k = 8: Cnk = 9! / (1! * 8!) = 9
        // k = 9: Cnk = 1
        // Space complexity: O(C(9,k))?

        return GetCombinations(digits, k).Where(c => c.Sum() == n).ToArray();
    }

    public IList<IList<int>> CombinationSum3_BruteForce_WithSeveralOptimizations(int k, int n)
    {
        // Time complexity: O(C(9,k))?
        // Space complexity: O(C(9,k))?

        // I've thought initially about some optimizations, and peeked into Leetcode solutions where they were. But I extended them a bit.
        return GetCombinations_Optimized(digits, k, targetSum: n);
    }

    // General purpose method to find combinations of k items from integer array of length n.
    // It is not optimized for current task.
    private static IList<IList<int>> GetCombinations(int[] items, int k)
    {
        if (k == items.Length)
            return new[] { items };
        if (k > items.Length || k < 1)
            return Array.Empty<int[]>();

        // items array should be sorted
        var combinations = new List<IList<int>>();
        AddCombinations(0, new List<int>());
        return combinations;


        void AddCombinations(int itemIndexInCombination, List<int> itemsInCombinationSoFar) // maintain list in sorted order
        {
            if (itemIndexInCombination == k)
            {
                combinations.Add(itemsInCombinationSoFar);
                return ;
            }

            var minBoundForNextAllowedItem = itemsInCombinationSoFar.LastOrDefault() + 1; // max(list) + 1

            var nextAllowedItemIndex = Array.BinarySearch(items, minBoundForNextAllowedItem);
            if (nextAllowedItemIndex < 0)
                nextAllowedItemIndex = ~nextAllowedItemIndex;

            if (nextAllowedItemIndex >= items.Length)
                return;

            for (var i = nextAllowedItemIndex; i < items.Length; i++)
            {
                var nextItem = items[i];
                var itemsInCombination = itemsInCombinationSoFar.ToList();
                itemsInCombination.Add(nextItem);
                AddCombinations(itemIndexInCombination + 1, itemsInCombination);
            }
        }
    }

    // Method that is optimized for this particular task, but still it is extensible.
    // What it is taken into consideration:
    // 1) There are no negative numbers participating in combinations.
    // 2) Array starts from integer 1, and each next item is greater than previous by 1.
    // P.1 and p.2 both allow to calculate sum on the fly and exit early, thus reducing amount of recursion calls.
    // P.2 replaces binary search with search by index.
    private static IList<IList<int>> GetCombinations_Optimized(int[] items, int k, int targetSum)
    {
        if (k > items.Length || k < 1)
            return Array.Empty<int[]>();

        // items array should be sorted
        var combinations = new List<IList<int>>();
        AddCombinations(0, new List<int>(), 0);
        return combinations;


        // Return false when sum is exceeded
        bool AddCombinations(int itemIndexInCombination, List<int> itemsInCombinationSoFar, int currentSum) // maintain list in sorted order
        {
            if (currentSum > targetSum)
                return false;

            if (itemIndexInCombination == k)
            {
                if (currentSum == targetSum)
                    combinations.Add(itemsInCombinationSoFar);
                return true;
            }

            var nextNumberIndex = itemsInCombinationSoFar.LastOrDefault(); // max(list)

            for (var i = nextNumberIndex; i < items.Length; i++)
            {
                var nextItem = items[i];
                var itemsInCombination = itemsInCombinationSoFar.ToList();
                itemsInCombination.Add(nextItem);

                if (!AddCombinations(itemIndexInCombination + 1, itemsInCombination, currentSum: currentSum + nextItem))
                    break;
            }

            return true;
        }
    }
}