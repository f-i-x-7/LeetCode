// https://leetcode.com/problems/middle-of-the-linked-list/

//Given the head of a singly linked list, return the middle node of the linked list.

//If there are two middle nodes, return the second middle node.

//Example 1:
//Input: head = [1, 2, 3, 4, 5]
//Output:[3,4,5]
//Explanation: The middle node of the list is node 3.

//Example 2:
//Input: head = [1, 2, 3, 4, 5, 6]
//Output:[4,5,6]
//Explanation: Since the list has two middle nodes with values 3 and 4, we return the second one.

//Constraints:
//The number of nodes in the list is in the range [1, 100].
//1 <= Node.val <= 100


Test(new[] { 1, 2, 3, 4, 5 }, expectedResult: new[] { 3, 4, 5 });
Test(new[] { 1, 2, 3, 4, 5, 6 }, expectedResult: new[] { 4, 5, 6 });



static void Test(int[] source, int[] expectedResult)
{
    var resultAsLinkedList = Solution.MiddleNode(ConvertToLinkedList(source));
    var result = ConvertToArray(resultAsLinkedList);
    var equal = Enumerable.SequenceEqual(expectedResult, result);

    if (!equal)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: Source = {ToString(source)}; Expected result = {ToString(expectedResult)}; Actual result = {ToString(result)}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: Source = {ToString(source)}; Result = {ToString(result)}");
    }


    static string ToString(int[] arr) => $"[{string.Join(",", arr)}]";
}



static ListNode ConvertToLinkedList(int[] items)
{
    var node = new ListNode(items[0]);
    var head = node;
    var i = 1;

    while (i < items.Length)
    {
        node.next = new ListNode(items[i]);
        node = node.next;
        i++;
    }

    return head;
}

static int[] ConvertToArray(ListNode list)
{
    var result = new List<int>();
    var current = list;
    while (current != null)
    {
        result.Add(current.val);
        current = current.next;
    }
    return result.ToArray();
}

public class ListNode
{
    public int val;
    public ListNode next;
    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }
}

public static class Solution
{
    public static ListNode MiddleNode(ListNode head)
    {
        var current = head;
        var middle = head;

        //Mapping of list length to one-based index of middle:
        // 1 : 1 - the only middle
        // 2 : 2 - second middle
        // 3 : 2 - the only middle
        // 4 : 3 - second middle
        // 5 : 3 - the only middle
        // 6 : 4 - second middle
        // 7 : 4 - the only middle
        // ...

        // We see the pattern here. So iterate list and update middle pointer according to pattern (update it every 2nd iteration, in other words - twice slowly).
        // O(N) time, O(1) space.

        // After solution, I checked suggested Leetcode solutions, and there is similar approach - move one of the pointer twice faster.

        var currentIndexOneBased = 1;
        while (current != null)
        {
            if (currentIndexOneBased % 2 == 0)
                middle = middle.next;

            current = current.next;
            currentIndexOneBased++;
        }

        return middle;
    }
}