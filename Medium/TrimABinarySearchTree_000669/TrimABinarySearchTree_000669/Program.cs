// https://leetcode.com/problems/trim-a-binary-search-tree/

//Given the root of a binary search tree and the lowest and highest boundaries as low and high, trim the tree so that all its elements lies in [low, high].
//Trimming the tree should not change the relative structure of the elements that will remain in the tree
//(i.e., any node's descendant should remain a descendant). It can be proven that there is a unique answer.

//Return the root of the trimmed binary search tree. Note that the root may change depending on the given bounds.

//Example 1:
//Input: root = [1, 0, 2], low = 1, high = 2
//Output:[1,null,2]

//Example 2:
//Input: root = [3, 0, 4, null, 2, null, null, 1], low = 1, high = 3
//Output:[3,2,null,1]

//Constraints:
//The number of nodes in the tree in the range [1, 10^4].
//0 <= Node.val <= 10^4
//The value of each node in the tree is unique.
//root is guaranteed to be a valid binary search tree.
//0 <= low <= high <= 10^4

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
    public TreeNode TrimBST(TreeNode root, int low, int high)
    {
        return TrimBST_Recursive(root, low, high);
    }

    private TreeNode TrimBST_Recursive(TreeNode root, int low, int high)
    {
        if (root is null)
            return null;

        // Time complexity: up to O(N) (visiting all nodes can be required)
        // Space complexity (required amount of call stack): O(H) = O(logN) best (balanced tree), O(N) worst case (imbalanced tree as linked list)

        if (root.val < low)
        {
            // root does not lie in bounds, and its left subtree too (because all its nodes have values less than root).
            // Try with right subtree
            return TrimBST_Recursive(root.right, low, high);
        }

        if (root.val > high)
        {
            // root does not lie in bounds, and its right subtree too (because all its nodes have values greater than root).
            // Try with left subtree
            return TrimBST_Recursive(root.left, low, high);
        }

        // root itself should be preserved, but that's possibly not true for its children
        root.left = TrimBST_Recursive(root.left, low, high);
        root.right = TrimBST_Recursive(root.right, low, high);
        return root;
    }

    private TreeNode TrimBST_IterativeWithStack(TreeNode root, int low, int high)
    {
        var nodes = new Stack<TreeNode>();
        var current = root;

        // TODO
        return root;

        // Depth-first search with stack
        while (current != null || nodes.Count > 0)
        {
            while (current != null)
            {
                nodes.Push(current);
                current = current.left;
            }
            
            current = nodes.Pop();
            // 
            current = current.right;
        }
    }
}