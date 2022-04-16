// https://leetcode.com/problems/convert-bst-to-greater-tree/

//Given the root of a Binary Search Tree (BST), convert it to a Greater Tree such that every key of the original BST
//is changed to the original key plus the sum of all keys greater than the original key in BST.

//As a reminder, a binary search tree is a tree that satisfies these constraints:

//The left subtree of a node contains only nodes with keys less than the node's key.
//The right subtree of a node contains only nodes with keys greater than the node's key.
//Both the left and right subtrees must also be binary search trees.

//Example 1:
//Input: root = [4, 1, 6, 0, 2, 5, 7, null, null, null, 3, null, null, null, 8]
//Output:[30,36,21,36,35,26,15,null,null,null,33,null,null,null,8]

//Example 2:
//Input: root = [0, null, 1]
//Output:[1,null,1]

//Constraints:
//The number of nodes in the tree is in the range [0, 10^4].
//-10^4 <= Node.val <= 10^4
//All the values in the tree are unique.
//root is guaranteed to be a valid binary search tree.

//Note: This question is the same as 1038: https://leetcode.com/problems/binary-search-tree-to-greater-sum-tree/

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
    public TreeNode ConvertBST(TreeNode root)
    {
        return ConvertBST_Recursive(root);
    }

    private TreeNode ConvertBST_Recursive(TreeNode root)
    {
        // Time complexity: O(N) (need to visit every node)
        // Space complexity (required amount of call stack): O(H)=O(logN) best (when tree is balanced), O(N) worst (edge case of imbalanced tree - linked list)
        var sum = 0;
        Process(root, ref sum);
        return root;


        static void Process(TreeNode node, ref int sum)
        {
            if (node == null)
                return;

            // Search for greater nodes, e.g. do depth-first search from right side
            if (node.right != null)
            {
                Process(node.right, ref sum);
            }

            sum += node.val;
            node.val = sum;

            Process(node.left, ref sum);
        }
    }

    private TreeNode ConvertBST_IterateWithStack(TreeNode root)
    {
        // Time complexity: O(N) (need to visit every node)
        // Space complexity (for manually managed stack): O(H)=O(logN) best (when tree is balanced), O(N) worst (edge case of imbalanced tree - linked list)
        var sum = 0;
        var nodes = new Stack<TreeNode>();

        var current = root;

        while (current != null || nodes.Count > 0)
        {
            while (current != null)
            {
                // Search for greater nodes, e.g. do depth-first search from right side
                nodes.Push(current);
                current = current.right;
            }

            current = nodes.Pop();
            sum += current.val;
            current.val = sum;
            current = current.left;
        }

        return root;
    }

    // TODO: Approach #3 Reverse Morris In-order Traversal [Accepted], see Solution at Leetcode
}