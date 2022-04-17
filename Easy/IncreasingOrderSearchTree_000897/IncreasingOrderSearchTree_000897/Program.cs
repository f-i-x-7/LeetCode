// https://leetcode.com/problems/increasing-order-search-tree/

//Given the root of a binary search tree, rearrange the tree in in-order so that the leftmost node in the tree is now the root of the tree,
//and every node has no left child and only one right child.

//Example 1:
//Input: root = [5, 3, 6, 2, 4, null, 8, 1, null, null, null, 7, 9]
//Output:[1,null,2,null,3,null,4,null,5,null,6,null,7,null,8,null,9]

//Example 2:
//Input: root = [5, 1, 7]
//Output:[1,null,5,null,7]

//Constraints:
//The number of nodes in the given tree will be in the range [1, 100].
//0 <= Node.val <= 1000

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
    public TreeNode IncreasingBST(TreeNode root)
    {
        // Do depth-first search and place all nodes into queue, then process nodes extracted from queue
        // Time: O(N) - need to process all nodes
        // Space: O(N) - all nodes are placed into queue, and algo call stack will require from O(H) to O(N) space

        var orderedNodes = new Queue<TreeNode>();
        Process(root, orderedNodes);

        System.Diagnostics.Debug.Assert(orderedNodes.Count > 0);

        var current = orderedNodes.Dequeue();
        var newRoot = current;
        while (orderedNodes.Count > 0)
        {
            var next = orderedNodes.Dequeue();
            System.Diagnostics.Debug.Assert(current.val < next.val);
            current.left = null;
            current.right = next;
            current = next;
        }

        current.left = null;
        current.right = null;
        return newRoot;
        

        static void Process(TreeNode node, Queue<TreeNode> orderedNodes)
        {
            if (node == null)
                return;

            Process(node.left, orderedNodes);
            orderedNodes.Enqueue(node);
            Process(node.right, orderedNodes);
        }
    }
}