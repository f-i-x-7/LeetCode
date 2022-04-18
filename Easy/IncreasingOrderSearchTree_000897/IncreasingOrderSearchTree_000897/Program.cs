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



var root = new TreeNode(5, left: new TreeNode(1), right: new TreeNode(7));
//var root = new TreeNode(5, left: new TreeNode(1));
var result = new Solution().IncreasingBST(root);

Console.WriteLine("FINISHED");


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
        return IncreasingBST_RecursiveWithInPlaceProcessing_2(root);
    }

    private TreeNode IncreasingBST_RecursiveWithPostProcessing(TreeNode root)
    {
        // Do depth-first search and place all nodes into queue, then process nodes extracted from queue
        // Time: O(N) - need to process all nodes
        // Space: O(N) - all nodes are placed into queue, and also call stack requires O(H) space

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

    private TreeNode IncreasingBST_RecursiveWithInPlaceProcessing(TreeNode root)
    {
        // Peeked at Leetcode solution.
        // Traverse tree with depth-first search in-order, and append each visiting node into 'linked list'
        // Time: O(N) - need to process all nodes
        // Space: O(H) - for call stack
        var fake = new TreeNode(int.MinValue); // node holding min possible value, it would be the 1st node in transformed tree
        var current = fake;
        Process(root, ref current);
        return fake.right; // skip fake node


        // 'current' is last node in transformed tree (aka last node in result 'linked list')
        static void Process(TreeNode node, ref TreeNode current)
        {
            if (node == null)
                return;

            Process(node.left, ref current);

            // Left child was already visited, it is safe to break link.
            // And it is required to clear left link in visiting node, this automatically clears left link for last node in result 'linked list'
            // (e.g. imagine BST tree with root 5 and left node 1; if 'current.left = null' was written than link 5 -> 1 would be preserved.
            // And with this line, we always have 'current.left == null' equal to true:
            // 1) fake root (initially 'current' points to it) has null left node
            // 2) all real nodes appending to 'linked list' have cleared left child before append
            node.left = null;

            current.right = node; // append node being visited to 'linked list'
            current = node;
            Process(node.right, ref current);
        }
    }

    private TreeNode IncreasingBST_RecursiveWithInPlaceProcessing_2(TreeNode root)
    {
        // Inspired by previous solution, but no fake node introduced.
        TreeNode newRoot = null;
        TreeNode current = null;
        Process(root, ref current, ref newRoot);
        System.Diagnostics.Debug.Assert(newRoot != null);
        return newRoot;


        // 'current' is last node in transformed tree (aka last node in result 'linked list')
        static void Process(TreeNode node, ref TreeNode current, ref TreeNode newRoot)
        {
            if (node == null)
                return;

            Process(node.left, ref current, ref newRoot);

            if (newRoot == null)
            {
                // We are visiting the smallest value in BST
                System.Diagnostics.Debug.Assert(current == null);
                newRoot = node;
                current = node;
            }
            else
            {
                // Left child was already visited, it is safe to break link.
                // And it is required to clear left link in visiting node, this automatically clears left link for last node in result 'linked list'
                // (e.g. imagine BST tree with root 5 and left node 1; if 'current.left = null' was written than link 5 -> 1 would be preserved.
                // And with this line, we always have 'current.left == null' equal to true:
                // 1) node holding the smallest value in BST (initially 'current' points to it) has null left node
                // 2) all real nodes appending to 'linked list' have cleared left child before append
                node.left = null;

                current.right = node; // append node being visited to 'linked list'
                current = node;
            }
            Process(node.right, ref current, ref newRoot);
        }
    }
}