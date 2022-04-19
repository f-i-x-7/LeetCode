// https://leetcode.com/problems/recover-binary-search-tree/

//You are given the root of a binary search tree (BST), where the values of exactly two nodes of the tree were swapped by mistake.
//Recover the tree without changing its structure.

//Example 1:
//Input: root = [1, 3, null, null, 2]
//Output:[3,1,null,null,2]
//Explanation: 3 cannot be a left child of 1 because 3 > 1. Swapping 1 and 3 makes the BST valid.

//Example 2:
//Input: root = [3, 1, 4, null, null, 2]
//Output:[2,1,4,null,null,3]
//Explanation: 2 cannot be in the right subtree of 3 because 2 < 3. Swapping 2 and 3 makes the BST valid.

//Constraints:
//The number of nodes in the tree is in the range [2, 1000].
//-2^31 <= Node.val <= 2^31 - 1



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
    public void RecoverTree(TreeNode root)
    {
        RecoverTree_RecursiveUsingList(root);
    }

    // Main idea (unfortunately peeked out at Leetcode): while traversing tree in-order, one should observe sorted values.
    // If that is not true - this node is in wrong position.

    private static void Swap(List<TreeNode> nodesList, int index1, int index2)
    {
        var tmp = nodesList[index1].val;
        nodesList[index1].val = nodesList[index2].val;
        nodesList[index2].val = tmp;
    }

    private void RecoverTree_RecursiveUsingList(TreeNode root)
    {
        // Time: O(N) - need to process all nodes and then all elements in list
        // Space: O(N) for list (and O(H) for call stack)

        var nodesList = new List<TreeNode>();
        InOrder(root, nodesList);
        System.Diagnostics.Debug.Assert(nodesList.Count >= 2);

        int firstWrongIndex = -1;
        int secondWrongIndex = -1;

        for (var i = 0; i < nodesList.Count - 1; i++)
        {
            if (nodesList[i].val < nodesList[i + 1].val)
                continue;

            // i-th item is wrong
            if (firstWrongIndex < 0)
            {
                firstWrongIndex = i;
            }
            else
            {
                secondWrongIndex = i;
                break;
            }
        }

        System.Diagnostics.Debug.Assert(firstWrongIndex >= 0 && firstWrongIndex < nodesList.Count - 1);

        if (secondWrongIndex < 0)
        {
            // secondWrongIndex can be not initialized, see 2nd example from Leetcode,
            // in-order traversal would produce [1,3,2,4] list.
            // Need to swap first wrong item with its successor.
            Swap(nodesList, firstWrongIndex, firstWrongIndex + 1);
        }
        else
        {
            System.Diagnostics.Debug.Assert(firstWrongIndex < secondWrongIndex);
            System.Diagnostics.Debug.Assert(secondWrongIndex > 0 && secondWrongIndex < nodesList.Count - 1);
            // E.g. 1st Leetcode sample,
            // in-order traversal would produce [3,2,1] list.
            // So actually we need to swap first wrong item with successor of 2nd wrong item
            Swap(nodesList, firstWrongIndex, secondWrongIndex + 1);
        }


        static void InOrder(TreeNode current, List<TreeNode> nodesList)
        {
            if (current == null)
                return;

            InOrder(current.left, nodesList);
            nodesList.Add(current);
            InOrder(current.right, nodesList);
        }
    }
}