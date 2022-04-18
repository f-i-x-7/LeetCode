// https://leetcode.com/problems/kth-smallest-element-in-a-bst/

//Given the root of a binary search tree, and an integer k, return the kth smallest value (1-indexed) of all the values of the nodes in the tree.

//Example 1:

//Input: root = [3, 1, 4, null, 2], k = 1
//Output: 1

//Example 2:
//Input: root = [5, 3, 6, 2, 4, null, null, 1], k = 3
//Output: 3

//Constraints:
//The number of nodes in the tree is n.
//1 <= k <= n <= 10^4
//0 <= Node.val <= 10^4

//Follow up: If the BST is modified often (i.e., we can do insert and delete operations) and you need to find the kth smallest frequently, how would you optimize?

Console.WriteLine("Hello, World!");



public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
    {
        this.val = val;
        this.left = left;
        this.right = right;
    }
}
 
public class Solution
{
    public int KthSmallest(TreeNode root, int k)
    {
        return KthSmallest_Recursive(root, k);
    }

    private int KthSmallest_Recursive(TreeNode root, int k)
    {
        // Do depth-first search in-order (e.g. traverse BST from smallest to greatest element) and find k-th node
        // Time complexity: O(H + k) - need to dive for entire depth and then process k elements
        // Space complexity: O(H) for call stack

        var nodeIndex = -1;
        return Find(root, k, ref nodeIndex);



        int Find(TreeNode node, int k, ref int nodeIndex) // nodeIndex is one-based
        {
            if (node == null)
                return -1;

            var leftResult = Find(node.left, k, ref nodeIndex);
            if (leftResult >= 0)
                return leftResult; // k-th smallest node was found in left subtree

            // May be current node is the k-th smallest?
            if (nodeIndex <= 0)
            {
                // Smallest element was not reached previously; may be now it is?
                if (node.left is null)
                    nodeIndex = 1;
            }
            else
            {
                nodeIndex++;
            }

            if (nodeIndex == k)
                return node.val;

            // k-th smallest node lies in right subtree
            return Find(node.right, k, ref nodeIndex);
        }
    }

    private int KthSmallest_Iterative(TreeNode root, int k)
    {
        // Again, do depth-first search in-order, but do not use recursion - use manually managed stack to store elements in traversal order.
        // Time complexity: O(H + k) - need to dive for entire depth and then process k elements
        // Space complexity: O(H) for manually managed stack

        var nodes = new Stack<TreeNode>();
        var current = root;
        while (true)
        {
            while (current != null)
            {
                nodes.Push(current);
                current = current.left;
            }

            System.Diagnostics.Debug.Assert(nodes.Count > 0);
            current = nodes.Pop();
            if (--k == 0)
            {
                // Node is found
                return current.val;
            }

            // Left subtree and node itself were inspected, move to right subtree
            current = current.right;
        }
    }
}