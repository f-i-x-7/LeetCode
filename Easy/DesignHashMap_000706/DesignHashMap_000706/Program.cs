// https://leetcode.com/problems/design-hashmap/

//Design a HashMap without using any built-in hash table libraries.

//Implement the MyHashMap class:

//MyHashMap() initializes the object with an empty map.
//void put(int key, int value) inserts a (key, value) pair into the HashMap.If the key already exists in the map, update the corresponding value.
//int get(int key) returns the value to which the specified key is mapped, or -1 if this map contains no mapping for the key.
//void remove(key) removes the key and its corresponding value if the map contains the mapping for the key.


//Example 1:

//Input
//["MyHashMap", "put", "put", "get", "get", "put", "get", "remove", "get"]
//[[], [1, 1], [2, 2], [1], [3], [2, 1], [2], [2], [2]]
//Output
//[null, null, null, 1, -1, null, 1, null, -1]

//Explanation
//MyHashMap myHashMap = new MyHashMap();
//myHashMap.put(1, 1); // The map is now [[1,1]]
//myHashMap.put(2, 2); // The map is now [[1,1], [2,2]]
//myHashMap.get(1);    // return 1, The map is now [[1,1], [2,2]]
//myHashMap.get(3);    // return -1 (i.e., not found), The map is now [[1,1], [2,2]]
//myHashMap.put(2, 1); // The map is now [[1,1], [2,1]] (i.e., update the existing value)
//myHashMap.get(2);    // return 1, The map is now [[1,1], [2,1]]
//myHashMap.remove(2); // remove the mapping for 2, The map is now [[1,1]]
//myHashMap.get(2);    // return -1 (i.e., not found), The map is now [[1,1]]


//Constraints:

//0 <= key, value <= 10^6
//At most 10^4 calls will be made to put, get, and remove.

var map = new MyHashMap();
foreach (var x in Enumerable.Range(0, 17))
{
    map.Put(x, x);
}






public class MyHashMap
{
    private sealed class ListNode
    {
        public int Key { get; set; }
        public int Value { get; set; }
        public ListNode Next { get; set; }
    }

    private const int DefaultSize = 16;
    private ListNode[] buckets = new ListNode[DefaultSize];
    private int resizeThreshold = DefaultSize * 2;
    private int count;

    public MyHashMap()
    {
    }

    public void Put(int key, int value)
    {
        var hash = key;
        var bucketIndex = GetBucketIndex(hash, buckets);

        ref var head = ref buckets[bucketIndex];
        var current = head;
        ListNode previous = null;
        while (current != null)
        {
            if (current.Key == key)
            {
                // Value already exists
                current.Value = value;
                return;
            }

            previous = current;
            current = current.Next;
        }

        // Value does not exist, need to add
        count++;
        if (count >= resizeThreshold)
        {
            var newSize = resizeThreshold;
            resizeThreshold *= 2;
            if (resizeThreshold < 0)
                resizeThreshold = int.MaxValue;

            ResizeAndThenAdd(newSize, key, value);
            return;
        }

        AddToBucket(key, value, ref head, tail: previous);
    }

    public int Get(int key)
    {
        var hash = key;
        var bucketIndex = GetBucketIndex(hash, buckets);

        var current = buckets[bucketIndex];
        while (current != null)
        {
            if (current.Key == key)
                return current.Value;
            current = current.Next;
        }
        return -1;
    }

    public void Remove(int key)
    {
        var hash = key;
        var bucketIndex = GetBucketIndex(hash, buckets);

        ref var head = ref buckets[bucketIndex];
        ListNode previous = null;
        var current = head;

        while (current != null)
        {
            if (current.Key == key)
            {
                // Key found, perform deletion
                count--;

                if (previous == null)
                {
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

    private IEnumerable<(int key, int value)> GetAllItems()
    {
        foreach (var head in buckets)
        {
            var current = head;
            while (current != null)
            {
                yield return (current.Key, current.Value);
                current = current.Next;
            }
        }
    }

    private void ResizeAndThenAdd(int newSize, int keyToAdd, int valueToAdd)
    {
        var newBuckets = new ListNode[newSize];

        foreach (var (key, value) in GetAllItems().Concat(new[] { (keyToAdd, valueToAdd) }))
        {
            var hash = key;
            var bucketIndex = GetBucketIndex(hash, newBuckets);

            ref var head = ref newBuckets[bucketIndex];
            var current = head;
            ListNode previous = null;

            while (current != null)
            {
                previous = current;
                current = current.Next;
            }

            AddToBucket(key, value, ref head, tail: previous);
        }

        buckets = newBuckets;
    }

    private void AddToBucket(int key, int value, ref ListNode head, ListNode tail)
    {
        var newNode = new ListNode { Key = key, Value = value };
        if (head == null)
            head = newNode;
        else
            tail.Next = newNode;
    }

    private static int GetBucketIndex(int hash, ListNode[] buckets)
    {
        return hash % buckets.Length;
    }
}

/**
 * Your MyHashMap object will be instantiated and called as such:
 * MyHashMap obj = new MyHashMap();
 * obj.Put(key,value);
 * int param_2 = obj.Get(key);
 * obj.Remove(key);
 */