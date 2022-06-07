// https://leetcode.com/problems/intersection-of-two-linked-lists/

//Given the heads of two singly linked-lists headA and headB, return the node at which the two lists intersect. If the two linked lists have no intersection at all, return null.

//For example, the following two linked lists begin to intersect at node c1:

//The test cases are generated such that there are no cycles anywhere in the entire linked structure.

//Note that the linked lists must retain their original structure after the function returns.

//Custom Judge:

//The inputs to the judge are given as follows (your program is not given these inputs):

//intersectVal - The value of the node where the intersection occurs. This is 0 if there is no intersected node.
//listA - The first linked list.
//listB - The second linked list.
//skipA - The number of nodes to skip ahead in listA (starting from the head) to get to the intersected node.
//skipB - The number of nodes to skip ahead in listB (starting from the head) to get to the intersected node.
//The judge will then create the linked structure based on these inputs and pass the two heads, headA and headB to your program. If you correctly return the intersected node, then your solution will be accepted.


//Example 1:
//Input: intersectVal = 8, listA = [4, 1, 8, 4, 5], listB = [5, 6, 1, 8, 4, 5], skipA = 2, skipB = 3
//Output: Intersected at '8'
//Explanation: The intersected node 's value is 8 (note that this must not be 0 if the two lists intersect).
//From the head of A, it reads as [4,1,8,4,5]. From the head of B, it reads as [5,6,1,8,4,5]. There are 2 nodes before the intersected node in A; There are 3 nodes before the intersected node in B.

//Example 2:
//Input: intersectVal = 2, listA = [1, 9, 1, 2, 4], listB = [3, 2, 4], skipA = 3, skipB = 1
//Output: Intersected at '2'
//Explanation: The intersected node 's value is 2 (note that this must not be 0 if the two lists intersect).
//From the head of A, it reads as [1,9,1,2,4]. From the head of B, it reads as [3,2,4]. There are 3 nodes before the intersected node in A; There are 1 node before the intersected node in B.

//Example 3:
//Input: intersectVal = 0, listA = [2, 6, 4], listB = [1, 5], skipA = 3, skipB = 2
//Output: No intersection
//Explanation: From the head of A, it reads as [2,6,4]. From the head of B, it reads as [1,5]. Since the two lists do not intersect, intersectVal must be 0, while skipA and skipB can be arbitrary values.
//Explanation: The two lists do not intersect, so return null.

//Constraints:
//The number of nodes of listA is in the m.
//The number of nodes of listB is in the n.
//1 <= m, n <= 3 * 10^4
//1 <= Node.val <= 10^5
//0 <= skipA < m
//0 <= skipB < n
//intersectVal is 0 if listA and listB do not intersect.
//intersectVal == listA[skipA] == listB[skipB] if listA and listB intersect.



//Follow up: Could you write a solution that runs in O(m + n) time and use only O(1) memory?


Console.WriteLine("Hello, World!");



//Definition for singly-linked list.
public class ListNode
{
    public int val;
    public ListNode next;
    public ListNode(int x) { val = x; }
}
 
public class Solution
{
    public ListNode GetIntersectionNode_NaiveImpl1(ListNode headA, ListNode headB)
    {
        if (headA is null | headB is null)
            return null;

        // Time: O(m * n)
        // Space: O(1)
        // Just brute force
        var currentA = headA;
        var currentB = headB;

        while (currentA != null)
        {
            while (currentB != null)
            {
                if (currentA == currentB)
                    return currentA;

                currentB = currentB.next;
            }

            currentA = currentA.next;
            currentB = headB;
        }

        return null;
    }

    public ListNode GetIntersectionNode_NaiveImpl2(ListNode headA, ListNode headB)
    {
        if (headA is null | headB is null)
            return null;

        // Time: O(m + n + min(m, n))
        // Space: O(m + n)
        // Almost brute force: materialize all elements into dynamic arrays, then iterate from end of them until difference is found.
        var convertedA = TraverseAndConvert(headA);
        var convertedB = TraverseAndConvert(headB);

        var countToCheck = Math.Min(convertedA.Count, convertedB.Count);
        ListNode previous = null;
        for (var i = 0; i < countToCheck; i++)
        {
            var indexA = convertedA.Count - 1 - i;
            var indexB = convertedB.Count - 1 - i;

            if (convertedA[indexA] != convertedB[indexB])
            {
                return previous;
            }

            previous = convertedA[indexA];
        }

        return previous;


        static List<ListNode> TraverseAndConvert(ListNode head)
        {
            var result = new List<ListNode>();
            while (head != null)
            {
                result.Add(head);
                head = head.next;
            }
            return result;
        }
    }

    public ListNode GetIntersectionNode_Optimal(ListNode headA, ListNode headB)
    {
        // Peeked at Leetcode discussions
        // Time: O(m + n)
        // Space: O(1)
        // Traverse both linked lists at the same time until nodes are not same pointers. If one of the lists is finished and other is not - switch to another list.
        // This means that technically double traversal can occur.
        // Example:
        // Linked list A = a1 -> a2 -> a3 -> c1 -> c2 -> c3
        // Linked list B = b1 -> c1 -> c2 -> c3
        // So actually what would be traversed is:
        // a1 -> a2 -> a3 -> c1 -> c2 -> c3 -> b1 -> c1 -> c2 -> c3
        // b1 -> c1 -> c2 -> c3 -> a1 -> a2 -> a3 -> c1 -> c2 -> c3
        // We see that if 2 lists have intersection, then with this approach we'll get same length and same ending of lists.

        var currentA = headA;
        var currentB = headB;

        while (true)
        {
            if (currentA == currentB)
                return currentA;

            currentA = currentA is null ? headB : currentA.next;
            currentB = currentB is null ? headA : currentB.next;
        }
    }
}