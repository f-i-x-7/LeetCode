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

    private static void Swap(TreeNode node1, TreeNode node2)
    {
        var tmp = node1.val;
        node1.val = node2.val;
        node2.val = tmp;
    }

    private static void Swap(List<TreeNode> nodesList, int index1, int index2) => Swap(nodesList[index1], nodesList[index2]);

    private static void FindAndSwap(List<TreeNode> nodesList)
    {
        int firstWrongIndex = -1;
        int secondWrongIndex = -1;

        // 2 elements need to be swapped. They are either adjacent or not

        for (var i = 0; i < nodesList.Count - 1; i++)
        {
            if (nodesList[i].val < nodesList[i + 1].val)
                continue;

            // We have found wrong items
            if (firstWrongIndex < 0)
            {
                firstWrongIndex = i;
                // At this point, assume that these adjacent elements need to be swapped.
                // This can actually be true, see 2nd example from Leetcode, in-order traversal would produce [1,3,2,4] list, here first wrong item = 3 and second wrong item = 2, and the need to be swapped
                secondWrongIndex = i + 1; 
            }
            else
            {
                // One more invalid pair found, this means that we actually need to swap not adjacent elements. 
                // E.g. 1st Leetcode sample, in-order traversal would produce [3,2,1] list;
                // at previous stage, 1st wrong item = 3 and second wrong item = 2; now we detect that 2 > 1, and we actually need to swap 3 and 1.
                secondWrongIndex = i + 1;
                break; // it is guaranteed that other elements have correct order
            }
        }

        System.Diagnostics.Debug.Assert(firstWrongIndex >= 0 && firstWrongIndex < nodesList.Count - 1);
        System.Diagnostics.Debug.Assert(secondWrongIndex > 0 && secondWrongIndex < nodesList.Count);
        System.Diagnostics.Debug.Assert(firstWrongIndex < secondWrongIndex);

        Swap(nodesList, firstWrongIndex, secondWrongIndex);
    }

    private void RecoverTree_RecursiveUsingList(TreeNode root)
    {
        // Time: O(N) - need to process all nodes and then all elements in list
        // Space: O(N) for list (and O(H) for call stack)

        var nodesList = new List<TreeNode>();
        InOrder(root, nodesList);
        System.Diagnostics.Debug.Assert(nodesList.Count >= 2);

        FindAndSwap(nodesList);


        static void InOrder(TreeNode current, List<TreeNode> nodesList)
        {
            if (current == null)
                return;

            InOrder(current.left, nodesList);
            nodesList.Add(current);
            InOrder(current.right, nodesList);
        }
    }

    private void RecoverTree_IterativeUsingList(TreeNode root)
    {
        // Time: O(N) - need to process all nodes and then all elements in list
        // Space: O(N) for list (and O(H) for stack)
        var nodesList = new List<TreeNode>();
        var current = root;
        var nodes = new Stack<TreeNode>();

        while (current != null || nodes.Count > 0)
        {
            while (current != null)
            {
                nodes.Push(current);
                current = current.left;
            }

            current = nodes.Pop();
            nodesList.Add(current);

            current = current.right;
        }

        FindAndSwap(nodesList);
    }

    private void RecoverTree_IterativeWithoutList(TreeNode root)
    {
        // Time: O(N) - need to process all nodes
        // Space: O(H) for stack

        var nodes = new Stack<TreeNode>();
        var current = root;

        // Previous node according to in-order traversal
        TreeNode previous = null;
        // Nodes whose values need to be swapped
        TreeNode firstWrongNode = null;
        TreeNode secondWrongNode = null;

        while (current != null || nodes.Count > 0)
        {
            while (current != null)
            {
                nodes.Push(current);
                current = current.left;
            }

            current = nodes.Pop();

            if (previous != null)
            {
                if (previous.val > current.val)
                {
                    // Invalid node found
                    if (firstWrongNode == null)
                    {
                        // At this point, assume that these adjacent nodes (in terms of in-order traversal) need to be swapped.
                        // This can actually be true, see 2nd example from Leetcode, in-order traversal would produce [1,3,2,4] list, here first wrong item = 3 and second wrong item = 2, and the need to be swapped
                        firstWrongNode = previous;
                        secondWrongNode = current;
                    }
                    else
                    {
                        // One more invalid pair found, this means that we actually need to swap not adjacent (in terms of in-order traversal) nodes. 
                        // E.g. 1st Leetcode sample, in-order traversal would produce [3,2,1] list;
                        // at previous stage, 1st wrong item = 3 and second wrong item = 2; now we detect that 2 > 1, and we actually need to swap 3 and 1.
                        secondWrongNode = current;
                        break; // it is guaranteed that other elements have correct order
                    }
                }
            }
            
            previous = current;

            current = current.right;
        }

        System.Diagnostics.Debug.Assert(firstWrongNode != null && secondWrongNode != null);

        Swap(firstWrongNode, secondWrongNode);
    }
}