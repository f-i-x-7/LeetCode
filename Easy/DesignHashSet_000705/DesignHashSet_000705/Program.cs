// https://leetcode.com/problems/design-hashset/

//Design a HashSet without using any built-in hash table libraries.

//Implement MyHashSet class:

//void add(key) Inserts the value key into the HashSet.
//bool contains(key) Returns whether the value key exists in the HashSet or not.
//void remove(key) Removes the value key in the HashSet.If key does not exist in the HashSet, do nothing.


//Example 1:

//Input
//["MyHashSet", "add", "add", "contains", "contains", "add", "contains", "remove", "contains"]
//[[], [1], [2], [1], [3], [2], [2], [2], [2]]
//Output
//[null, null, null, true, false, null, true, null, false]

//Explanation
//MyHashSet myHashSet = new MyHashSet();
//myHashSet.add(1);      // set = [1]
//myHashSet.add(2);      // set = [1, 2]
//myHashSet.contains(1); // return True
//myHashSet.contains(3); // return False, (not found)
//myHashSet.add(2);      // set = [1, 2]
//myHashSet.contains(2); // return True
//myHashSet.remove(2);   // set = [1]
//myHashSet.contains(2); // return False, (already removed)


//Constraints:

//0 <= key <= 10^6
//At most 10^4 calls will be made to add, remove, and contains.

Console.WriteLine("Hello, World!");



public class MyHashSet
{
    private sealed class ListNode
    {
        public int Value { get; set; }
        public ListNode Next { get; set; }
    }

    private const int InitialCapacity = 16;

    private ListNode[] buckets = new ListNode[InitialCapacity];
    private int resizeThreshold = InitialCapacity * 2;
    private int count;

    public MyHashSet()
    {
    }

    public void Add(int key)
    {
        var hash = key;
        var bucketIndex = hash % buckets.Length;
        ref var head = ref buckets[bucketIndex];
        var current = head;
        ListNode tail = null;

        while (current != null)
        {
            if (current.Value == key)
                return;

            tail = current;
            current = current.Next;
        }

        // key does not exist in hash set
        if (count == int.MaxValue)
            throw new Exception("Collection is full.");

        count++;

        if (count <= resizeThreshold)
        {
            // No resize needed
            AddIntoBucket(key, ref head, tail);
        }
        else
        {
            ResizeAndThenAdd(key);
        }
    }

    public void Remove(int key)
    {
        var hash = key;
        var bucketIndex = hash % buckets.Length;
        ref var head = ref buckets[bucketIndex];
        var current = head;
        ListNode previous = null;

        while (current != null)
        {
            if (current.Value == key)
            {
                // key is found, need to remove it
                count--;
                if (previous == null)
                {
                    // head holds key
                    head = head.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
                return;
            }

            previous = current;
            current = current.Next;
        }
    }

    public bool Contains(int key)
    {
        var hash = key;
        var bucketIndex = hash % buckets.Length;
        var current = buckets[bucketIndex];

        while (current != null)
        {
            if (current.Value == key)
                return true;

            current = current.Next;
        }

        return false;
    }

    private IEnumerable<int> GetAllKeys()
    {
        foreach (var head in buckets)
        {
            var current = head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }

    private void ResizeAndThenAdd(int keyToAdd)
    {
        var newBuckets = new ListNode[resizeThreshold];

        resizeThreshold = count * 2;
        if (resizeThreshold < 0)
            resizeThreshold = int.MaxValue;

        foreach (var key in GetAllKeys().Concat(new[] { keyToAdd }))
        {
            var hash = key;
            var bucketIndex = hash % newBuckets.Length;
            ref var head = ref newBuckets[bucketIndex];
            var current = head;
            ListNode tail = null;

            while (current != null)
            {
                tail = current;
                current = current.Next;
            }

            AddIntoBucket(key, ref head, tail);
        }

        buckets = newBuckets;
    }

    private void AddIntoBucket(int key, ref ListNode head, ListNode tail)
    {
        var newNode = new ListNode { Value = key };
        if (head == null)
            head = newNode;
        else
            tail.Next = newNode;
    }
}