// https://leetcode.com/problems/find-a-corresponding-node-of-a-binary-tree-in-a-clone-of-that-tree/

//Given two binary trees original and cloned and given a reference to a node target in the original tree.

//The cloned tree is a copy of the original tree.

//Return a reference to the same node in the cloned tree.

//Note that you are not allowed to change any of the two trees or the target node and the answer must be a reference to a node in the cloned tree.

//Example 1:
//Input: tree = [7, 4, 3, null, null, 6, 19], target = 3
//Output: 3
//Explanation: In all examples the original and cloned trees are shown. The target node is a green node from the original tree. The answer is the yellow node from the cloned tree.

//Example 2:
//Input: tree = [7], target = 7
//Output: 7

//Example 3:
//Input: tree = [8, null, 6, null, 5, null, 4, null, 3, null, 2, null, 1], target = 4
//Output: 4

//Constraints:
//The number of nodes in the tree is in the range [1, 10^4].
//The values of the nodes of the tree are unique.
//target node is a node from the original tree and is not null.

//Follow up: Could you solve the problem if repeated values on the tree are allowed?

Console.WriteLine("Hello, World!");





public class TreeNode
{
    public int val;
    public TreeNode left;
    public TreeNode right;
    public TreeNode(int x) { val = x; }
}

public class Solution
{
    public TreeNode GetTargetCopy(TreeNode original, TreeNode cloned, TreeNode target)
    {
        // If there are unique values, then the task can be transformed to finding specific value in cloned tree.
        // It can be accomplished by regular DFS/BFS. This is too simple.
        // So follow-up is implemented.

        // Traverse both trees at the same time (in-order DFS).

        var originalNodes = new Stack<TreeNode>();
        var clonedNodes = new Stack<TreeNode>();

        var originalCurrent = original;
        var clonedCurrent = cloned;

        while (originalCurrent != null || originalNodes.Count > 0)
        {
            while (originalCurrent != null)
            {
                originalNodes.Push(originalCurrent);
                clonedNodes.Push(clonedCurrent);

                originalCurrent = originalCurrent.left;
                clonedCurrent = clonedCurrent.left;
            }

            originalCurrent = originalNodes.Pop();
            clonedCurrent = clonedNodes.Pop();

            if (originalCurrent == target)
                return clonedCurrent;

            originalCurrent = originalCurrent.right;
            clonedCurrent = clonedCurrent.right;
        }

        return null;
    }
}