// https://leetcode.com/problems/deepest-leaves-sum/

//Given the root of a binary tree, return the sum of values of its deepest leaves.

//Example 1:
//Input: root = [1, 2, 3, 4, 5, null, 6, 7, null, null, null, null, 8]
//Output: 15

//Example 2:
//Input: root = [6, 7, 8, 2, 7, 1, 3, 9, null, 1, 4, null, null, null, 5]
//Output: 19

//Constraints:
//The number of nodes in the tree is in the range [1, 10^4].
//1 <= Node.val <= 100

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
    public int DeepestLeavesSum(TreeNode root)
    {
        return DeepestLeavesSum_BFS_Recursive(root);
    }

    private int DeepestLeavesSum_BFS_Recursive(TreeNode root)
    {
        // Time: O(N) - need to visit all nodes
        // Space: O(N) for lists, O(H) for call stack
        return BFS(new List<TreeNode>() { root });


        int BFS(List<TreeNode> nodesAtPreviousLevel)
        {
            var nodesAtCurrentLevel = new List<TreeNode>();
            foreach (var node in nodesAtPreviousLevel)
            {
                if (node.left != null)
                    nodesAtCurrentLevel.Add(node.left);
                if (node.right != null)
                    nodesAtCurrentLevel.Add(node.right);
            }

            if (nodesAtCurrentLevel.Count == 0)
            {
                // Deepest level is reached
                return nodesAtPreviousLevel.Sum(n => n.val);
            }

            return BFS(nodesAtCurrentLevel);
        }
    }

    private int DeepestLeavesSum_BFS_Iterative(TreeNode root)
    {
        // Time: O(N) - need to visit all nodes
        // Space: O(N) - for lists
        var nodesAtPreviousLevel = new List<TreeNode>() { root };
        while (true)
        {
            var nodesAtCurrentLevel = new List<TreeNode>();
            foreach (var node in nodesAtPreviousLevel)
            {
                if (node.left != null)
                    nodesAtCurrentLevel.Add(node.left);
                if (node.right != null)
                    nodesAtCurrentLevel.Add(node.right);
            }

            if (nodesAtCurrentLevel.Count == 0)
            {
                // Deepest level is reached
                return nodesAtPreviousLevel.Sum(n => n.val);
            }

            nodesAtPreviousLevel = nodesAtCurrentLevel;
        }
    }
}