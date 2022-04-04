using SwappingNodesInALinkedList_001721;

https://leetcode.com/problems/swapping-nodes-in-a-linked-list/

// You are given the head of a linked list, and an integer k.
// Return the head of the linked list after swapping the values of the kth node from the beginning and the kth node from the end
// (the list is 1-indexed).

// Example 1:
// Input: head = [1, 2, 3, 4, 5], k = 2
// Output:[1,4,3,2,5]

// Example 2:
// Input: head = [7, 9, 6, 6, 7, 8, 3, 0, 9, 5], k = 5
// Output:[7,9,6,6,8,7,3,0,9,5]


// Constraints:
// The number of nodes in the list is n.
// 1 <= k <= n <= 10^5
// 0 <= Node.val <= 100



// Samples from task definition
Test(source: new[] { 1, 2, 3, 4, 5 }, k: 2, expectedResult: new[] { 1, 4, 3, 2, 5 }); // swap non-adjacent non-head & non-tail nodes
Test(source: new[] { 7, 9, 6, 6, 7, 8, 3, 0, 9, 5 }, k: 5, expectedResult: new[] { 7, 9, 6, 6, 8, 7, 3, 0, 9, 5 }); // swap adjacent non-head & non-tail nodes

// Own test cases
Test(source: new[] { 1 }, k: 1, expectedResult: new[] { 1 }); // no swapping
Test(source: new[] { 1, 2 }, k: 1, expectedResult: new[] { 2, 1 }); // swap adjacent head and tail
Test(source: new[] { 1, 2 }, k: 2, expectedResult: new[] { 2, 1 }); // swap adjacent head and tail, mirrored 'k'
Test(source: new[] { 1, 2, 3 }, k: 1, expectedResult: new[] { 3, 2, 1 }); // swap non-adjacent head and tail
Test(source: new[] { 1, 2, 3 }, k: 3, expectedResult: new[] { 3, 2, 1 }); // swap non-adjacent head and tail, mirrored 'k'

// Same as from task definition but k is mirrored
Test(source: new[] { 1, 2, 3, 4, 5 }, k: 4, expectedResult: new[] { 1, 4, 3, 2, 5 });  // swap non-adjacent non-head & non-tail nodes, mirrored 'k'
Test(source: new[] { 7, 9, 6, 6, 7, 8, 3, 0, 9, 5 }, k: 6, expectedResult: new[] { 7, 9, 6, 6, 8, 7, 3, 0, 9, 5 }); // swap adjacent non-head & non-tail nodes, mirrored 'k'

static void Test(int[] source, int k, int[] expectedResult)
{
    var result = Swap(source, k);
    var equal = Enumerable.SequenceEqual(expectedResult, result);

    if (!equal)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: k = {k}, Source = {ToString(source)}, Expected result = {ToString(expectedResult)}, Actual result = {ToString(result)}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: k = {k}, Source = {ToString(source)}, Result = {ToString(result)}");
    }


    static string ToString(int[] arr) => $"[{string.Join(",", arr)}]";
}

static int[] Swap(int[] items, int k)
{
    var resultAsLinkedList = Solution.SwapNodes(ConvertToLinkedList(items), k);

    var result = new int[items.Length];
    var i = 0;
    var node = resultAsLinkedList;
    while (node != null)
    {
        result[i] = node.val;
        node = node.next;
        i++;
    }

    return result;


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
}






// Definition for singly - linked list.
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
    public static ListNode SwapNodes(ListNode head, int k)
    {
        if (k <= 0)
            throw new ArgumentOutOfRangeException(nameof(k));
        if (head is null)
            throw new ArgumentException(nameof(head));

        return SwapNodes_Impl2(head, k);
    }

    public static ListNode SwapNodes_Impl1(ListNode head, int k)
    {
        // O(N) time complexity, O(k) additional adjacent space required (because of 'k+1' size of circular buffer), seemed most optimal here.
        // Other non-optimal strategies that came into mind:
        // 1) Materialize linked list into List<T> (requires O(N) additional adjacent space), then easily find k-th element from behind. Still O(N) time complexity.
        // 2) Convert singly linked list into double linked list, determine its count, then to define k-th item from tail either iterate from tail or from head (depends on k/N value).
        // O(N) additional space, time complexity up to O(N + N/2)
        var (predecessorOfToSwapFromStart, predecessorOfToSwapFromEnd, tail) = IterateLinkedList(head, k);

        var toSwapFromStart = predecessorOfToSwapFromStart is null ? head : predecessorOfToSwapFromStart.next;
        var toSwapFromEnd = predecessorOfToSwapFromEnd is null ? head : predecessorOfToSwapFromEnd.next;
        
        var result = Swap(head: head,
            tail: tail,
            node1: toSwapFromStart,
            node2: toSwapFromEnd,
            nodeBeforeNode1: predecessorOfToSwapFromStart,
            nodeBeforeNode2: predecessorOfToSwapFromEnd);

        return result;



        static (ListNode predecessorOfToSwapFromStart, ListNode predecessorOfToSwapFromEnd, ListNode tail) IterateLinkedList(ListNode head, int k)
        {
            ListNode current = head;
            int i = 1;
            ListNode previous = null;
            ListNode predecessorOfToSwapFromStart = null;
            ListNode predecessorOfToSwapFromEnd = null;

            // We can use circular buffer of capacity 'k' and push every node during iteration into its back.
            // Then, in the end, front of buffer will contain k-th item from tail of linked list.
            // But as our linked list is singly, then we need also predecessor of k-th item from tail.
            // So use 'k+1' capacity, and do not forget that 'k+1' can be greater than N.
            CircularBuffer<ListNode> lastKPlusOneNodes = new(k + 1);

            while (current != null)
            {
                if (i == k)
                {
                    predecessorOfToSwapFromStart = previous;
                }

                lastKPlusOneNodes.PushBack(current);

                previous = current;
                current = current.next;
                i++;
            }

            System.Diagnostics.Debug.Assert(lastKPlusOneNodes.Size >= 1);
            if (lastKPlusOneNodes.Size == k + 1)
            {
                predecessorOfToSwapFromEnd = lastKPlusOneNodes.PeekFront();
            }
            else
            {
                // It is found out that k == N
                System.Diagnostics.Debug.Assert(lastKPlusOneNodes.Size == k);
                predecessorOfToSwapFromEnd = null;
            }

            return (predecessorOfToSwapFromStart: predecessorOfToSwapFromStart, predecessorOfToSwapFromEnd: predecessorOfToSwapFromEnd, tail: lastKPlusOneNodes.PeekBack());
        }

        // Return result is head of new list
        static ListNode Swap(ListNode head, ListNode tail, ListNode node1, ListNode node2, ListNode nodeBeforeNode1, ListNode nodeBeforeNode2)
        {
            System.Diagnostics.Debug.Assert(nodeBeforeNode1 == null || nodeBeforeNode1.next == node1);
            System.Diagnostics.Debug.Assert(nodeBeforeNode2 == null || nodeBeforeNode2.next == node2);

            if (node1 == node2)
                return head; // No swapping

            var nodeAfterNode1BeforeSwap = node1.next;
            var nodeAfterNode2BeforeSwap = node2.next;

            if (node1 == head)
            {
                System.Diagnostics.Debug.Assert(nodeBeforeNode1 == null, "because node1 is head");

                if (node2 == tail)
                {
                    System.Diagnostics.Debug.Assert(nodeAfterNode2BeforeSwap == null, "because node2 is tail");
                    System.Diagnostics.Debug.Assert(nodeBeforeNode2 != null, "because node2 is tail and node1 != node2");

                    // Swapping head & tail
                    if (head.next == tail)
                    {
                        // Adjacent head & tail
                        head.next = null;
                        tail.next = head;
                    }
                    else
                    {
                        // Non-adjacent head & tail
                        node1.next = null;
                        node2.next = nodeAfterNode1BeforeSwap; // TODO: fails here if head & tail are adjacent (cyclic reference)
                        nodeBeforeNode2.next = node1;
                    }
                    return tail;
                }

                System.Diagnostics.Debug.Fail("In this task it is not possible to swap head/tail with non-head/non-tail");
            }

            if (node2 == head)
            {
                System.Diagnostics.Debug.Assert(nodeBeforeNode2 == null, "because node2 is head");

                if (node1 == tail)
                {
                    System.Diagnostics.Debug.Assert(nodeAfterNode1BeforeSwap == null, "because node1 is tail");
                    System.Diagnostics.Debug.Assert(nodeBeforeNode1 != null, "because node1 is tail and node1 != node2");

                    // Swapping head & tail
                    if (head.next == tail)
                    {
                        // Adjacent head & tail
                        head.next = null;
                        tail.next = head;
                    }
                    else
                    {
                        node2.next = null;
                        node1.next = nodeAfterNode2BeforeSwap; // TODO: fails here if head & tail are adjacent (cyclic reference)
                        nodeBeforeNode1.next = node2;
                    }
                    return tail;
                }

                System.Diagnostics.Debug.Fail("In this task it is not possible to swap head/tail with non-head/non-tail");
            }

            // Swapping nodes are not head and/or tail. Head of linked list is not modified
            if (nodeBeforeNode2 == node1)
            {
                // Swapping nodes are adjacent: ... -> node1 -> node2 -> ...
                // Should be:                   ... -> node2 -> node1 -> ...
                nodeBeforeNode1.next = node2;
                node2.next = node1;
                node1.next = nodeAfterNode2BeforeSwap;

                return head;
            }

            if (nodeBeforeNode1 == node2)
            {
                // Swapping nodes are adjacent: ... -> node2 -> node1 -> ...
                // Should be:                   ... -> node1 -> node2 -> ...
                nodeBeforeNode2.next = node1;
                node1.next = node2;
                node2.next = nodeAfterNode1BeforeSwap;

                return head;
            }

            // Swapping nodes are adjacent: ... -> node1 -> ... -. node2 -> ...
            // Should be:                   ... -> node2 -> ... -> node1 -> ...
            // Or vice versa (same code will work)
            nodeBeforeNode1.next = node2;
            node2.next = nodeAfterNode1BeforeSwap; //will be cyclic reference when node1.next == node2!

            nodeBeforeNode2.next = node1;
            node1.next = nodeAfterNode2BeforeSwap;

            return head;
        }
    }

    public static ListNode SwapNodes_Impl2(ListNode head, int k)
    {
        // Solution seen at Leetcode after implementing own solution #1. Differences are:
        // 1) Do not swap list nodes, swap just values that are stored in nodes. It simplifies swapping code a lot, but may be not acceptable sometimes in real world.
        // But definitely - such possibility should be always considered during real-world tasks.
        // 2) Do not use additional memory to define k-th item from tail of list, just use "hacky iteration".

        var (leftTarget, rightTarget) = IterateLinkedList(head, k);

        var tmp = leftTarget.val;
        leftTarget.val = rightTarget.val;
        rightTarget.val = tmp;

        return head;


        static (ListNode leftTarget, ListNode rightTarget) IterateLinkedList(ListNode head, int k)
        {
            var leftTarget = head;
            var rightTarget = head;
            var current = head;
            var i = 1;

            while (current != null)
            {
                // what I used before:
                //if (i == k)
                //    leftTarget = current;

                // and right target was determined in completely different way
                
                // Solution that were suggested at Leetcode:
                // For leftTarget actually suggested code can perform poorlier IMHO:
                if (i < k)
                    leftTarget = leftTarget.next;

                // This is what I called "hacky", very interesting approach, for rightTarget.
                //
                // List has N items, we iterating from head (moving current pointer) and skip first k iterations, then started to move rightTarget pointer.
                // If k == 1 then at then end of iteration current pointer will be null (tail reached) and rightTarget will point to tail.
                // If K == N then at then end of iteration current pointer will be null (tail reached) and rightTarget will still point to head (not updated during this iteration).
                if (i > k)
                    rightTarget = rightTarget.next;

                i++;
                current = current.next;
            }

            return (leftTarget, rightTarget);
        }
    }
}