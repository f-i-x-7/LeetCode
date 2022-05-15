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



new Solution().DeepestLeavesSum(new TreeNode(1, left: new TreeNode(2), right: new TreeNode(3)));


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
        return DeepestLeavesSum_DFS_Recursive(root);
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

    private int DeepestLeavesSum_DFS_Recursive(TreeNode root)
    {
        // Time: O(N) - need to visit all nodes
        // Space: O(H) for call stack
        var sumLevel = 0;
        var sum = 0;

        DFS(root, currentLevel: 0, ref sumLevel, ref sum);
        return sum;


        void DFS(TreeNode root, int currentLevel, ref int sumLevel, ref int sum)
        {
            // Post-order traversal
            if (root.left != null)
                DFS(root.left, currentLevel + 1, ref sumLevel, ref sum);
            if (root.right != null)
                DFS(root.right, currentLevel + 1, ref sumLevel, ref sum);

            if (currentLevel > sumLevel)
            {
                sum = root.val;
                sumLevel = currentLevel;
            }
            else if (currentLevel == sumLevel)
            {
                sum += root.val;
            }
        }
    }

    private int DeepestLeavesSum_DFS_Iterative(TreeNode root)
    {
        // Time: O(N) - need to visit all nodes
        // Space: O(H) for stack
        var stack = new Stack<(TreeNode Node, int Depth)>();

        // In-order traversal
        var sum = 0;
        var sumDepth = 0;
        var current = root;
        var currDepth = 0;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push((current, currDepth));
                current = current.left;
                currDepth++;
            }

            (current, currDepth) = stack.Pop();
            if (currDepth > sumDepth)
            {
                sum = current.val;
                sumDepth = currDepth;
            }
            else if (currDepth == sumDepth)
            {
                sum += current.val;
            }

            current = current.right;
            currDepth++;
        }

        return sum;
    }
}