// https://leetcode.com/problems/container-with-most-water/

//You are given an integer array height of length n. There are n vertical lines drawn such that the two endpoints of the ith line are (i, 0) and(i, height[i]).
//Find two lines that together with the x-axis form a container, such that the container contains the most water.
//Return the maximum amount of water a container can store.
//Notice that you may not slant the container.

//Example 1:
//Input: height = [1, 8, 6, 2, 5, 4, 8, 3, 7]
//Output: 49
//Explanation: The above vertical lines are represented by array [1,8,6,2,5,4,8,3,7]. In this case, the max area of water (blue section) the container can contain is 49.

//Example 2:
//Input: height = [1, 1]
//Output: 1

//Constraints:
//n == height.length
//2 <= n <= 10^5
//0 <= height[i] <= 10^4


Test(new[] { 1, 1 }, 1);
Test(new[] { 1, 2 }, 1);
Test(new[] { 3, 1 }, 1);
Test(new[] { 5, 10 }, 5);
Test(new[] { 10, 5 }, 5);
Test(new[] { 1, 8, 6, 2, 5, 4, 8, 3, 7 }, 49);
Test(new[] { 1, 2, 4, 3 }, 4);
Test(new[] { 1, 3, 2, 5, 25, 24, 5 }, 24);


static void Test(int[] arr, int expectedResult)
{
    var actualResult = Solution.MaxArea(arr);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED! {ToString(arr)}, Expected={expectedResult}, Actual={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS! {ToString(arr)}, MaxArea={expectedResult}");
    }
}

static string ToString(int[] arr) => $"[{string.Join(",", arr)}]";


public static class Solution
{
    public static int MaxArea(int[] height)
    {
        if (height is null || height.Length < 2)
            return -1;

        var result = -1;
        var head = 0;
        var tail = height.Length - 1;

        while (head < tail)
        {
            //Console.WriteLine(head + " " + tail);

            // Calculate volume at current position
            var currentVolume = (tail - head) * Math.Min(height[head], height[tail]);
            result = Math.Max(result, currentVolume);

            // Determine where to move next.


            // FAILED OLD APPROACH #1 - go to the side where array item is greater
            //var headDifference = height[head + 1] - height[head];
            //var tailDifference = height[tail - 1] - height[tail];
            //if (headDifference > tailDifference)
            //    head++;
            //else
            //    tail--;

            // FAILED OLD APPROACH #2 - go to the side there next value of area will be greater
            // We should try to maximize the currentVolume at the next iteration.
            // currentVolume is a product. We cannot affect one multiplicand (tail - head) as it will decrease by 1. But another multiplicand is under our control.
            // What we should try is to move to the side where Math.Min(height[head], height[tail]) will be greater.
            //var minHeightIfHeadIsMoved = Math.Min(height[head + 1], height[tail]);
            //var minHeightIfTailIsMoved = Math.Min(height[head], height[tail - 1]);

            //if (minHeightIfHeadIsMoved > minHeightIfTailIsMoved)
            //    head++;
            //else
            //    tail--;

            // Valid approach, unfortunately I didn't manage to come up with this and peeked it at discussions & solution sections at Leetcode website.
            // Good description from solution section:
            // Initially we consider the area constituting the exterior most lines.
            // Now, to maximize the area, we need to consider the area between the lines of larger lengths.
            // If we try to move the pointer at the longer line inwards, we won't gain any increase in area, since it is limited by the shorter line.
            // But moving the shorter line's pointer could turn out to be beneficial, as per the same argument, despite the reduction in the width.
            // This is done since a relatively longer line obtained by moving the shorter line's pointer might overcome the reduction in area caused by the width reduction.
            if (height[head] < height[tail])
                head++;
            else
                tail--;
        }

        return result;
    }
}