// https://leetcode.com/problems/flatten-nested-list-iterator/

//You are given a nested list of integers nestedList. Each element is either an integer or a list whose elements may also be integers or other lists. Implement an iterator to flatten it.

//Implement the NestedIterator class:

//NestedIterator(List<NestedInteger> nestedList) Initializes the iterator with the nested list nestedList.
//int next() Returns the next integer in the nested list.
//boolean hasNext() Returns true if there are still some integers in the nested list and false otherwise.
//Your code will be tested with the following pseudocode:

//initialize iterator with nestedList
//res = []
//while iterator.hasNext()
//append iterator.next() to the end of res
//return res
//If res matches the expected flattened list, then your code will be judged as correct.

//Example 1:
//Input: nestedList = [[1, 1],2,[1,1]]
//Output: [1,1,2,1,1]
//Explanation: By calling next repeatedly until hasNext returns false, the order of elements returned by next should be: [1,1,2,1,1].

//Example 2:
//Input: nestedList = [1,[4,[6]]]
//Output: [1,4,6]
//Explanation: By calling next repeatedly until hasNext returns false, the order of elements returned by next should be: [1,4,6].

//Constraints:
//1 <= nestedList.length <= 500
//The values of the integers in the nested list is in the range [-10^6, 10^6].

var item0 = new NestedIntegerClass(new List<NestedInteger>()); // []
var item1 = new NestedIntegerClass(new List<NestedInteger> { new NestedIntegerClass(new List<NestedInteger>()) }); // [[]]
var item2 = new NestedIntegerClass(new List<NestedInteger> { new NestedIntegerClass(1) }); // [1]
var item3 = new NestedIntegerClass(new List<NestedInteger> { new NestedIntegerClass(2), new NestedIntegerClass(2) }); // [2,2]
var item4 = new NestedIntegerClass(3); // 3
var item5 = new NestedIntegerClass(new List<NestedInteger>()); // []
var item6 = new NestedIntegerClass(new List<NestedInteger> { new NestedIntegerClass(4), new NestedIntegerClass(4), new NestedIntegerClass(4) }); // [4,4,4]
var list = new List<NestedInteger> { item0, item1, item2, item3, item4, item5, item6 }; // [[], [[]], [1], [2,2], 3, [], [4,4,4]]

//var list = new List<NestedInteger> { new NestedIntegerClass(new List<NestedInteger>()) }; // [[]]

list = new List<NestedInteger> { new NestedIntegerClass(list) };
list = new List<NestedInteger> { new NestedIntegerClass(list) };
var iterator = new NestedIterator(list);

while (iterator.HasNext())
{
    Console.WriteLine(iterator.Next());
}


public class NestedIntegerClass : NestedInteger
{
    private readonly bool _isInt;
    private readonly int _valueInt;
    private readonly IList<NestedInteger> _valueList;

    public NestedIntegerClass(int value)
    {
        _valueInt = value;
        _isInt = true;
    }

    public NestedIntegerClass(IList<NestedInteger> value)
    {
        _valueList = value;
    }

    public int GetInteger() => _valueInt;
    public IList<NestedInteger> GetList() => _valueList;
    public bool IsInteger() => _isInt;
}

// This is the interface that allows for creating nested lists.
// You should not implement it, or speculate about its implementation
public interface NestedInteger
{
    // @return true if this NestedInteger holds a single integer, rather than a nested list.
    bool IsInteger();

    // @return the single integer that this NestedInteger holds, if it holds a single integer
    // Return null if this NestedInteger holds a nested list
    int GetInteger();

    // @return the nested list that this NestedInteger holds, if it holds a nested list
    // Return null if this NestedInteger holds a single integer
    IList<NestedInteger> GetList();
}

public class NestedIterator
{
    private readonly IList<NestedInteger> _list;
    private int _nextPosition;

    /// <summary>
    /// Iterator for item at <see cref="_nextPosition"/> index if it is a list.
    /// </summary>
    private NestedIterator _nestedIterator;

    public NestedIterator(IList<NestedInteger> nestedList)
    {
        _list  = nestedList;
    }

    public bool HasNext()
    {
        while (true)
        {
            if (_nextPosition >= _list.Count)
                return false;

            var next = _list[_nextPosition];
            if (next.IsInteger())
                return true;

            // next is list
            var list = next.GetList();
            if (list.Count == 0 || !new NestedIterator(list).HasNext())
            {
                // list is empty, skip this item and go further
                _nextPosition++;
                continue;
            }

            // list is not empty
            return true;
        }
    }

    public int Next()
    {
        if (_nestedIterator != null)
        {
            return GetNextFromNestedIterator();
        }

        var current = _list[_nextPosition];
        if (current.IsInteger())
        {
            _nextPosition++;
            return current.GetInteger();
        }

        _nestedIterator = new NestedIterator(current.GetList());
        // according to HasNext() method implementation, _nextPosition cannot point to empty list, so we expect that _nestedIterator will return at least one item
        return GetNextFromNestedIterator();
    }

    private int GetNextFromNestedIterator()
    {
        System.Diagnostics.Debug.Assert(_nestedIterator != null);

        // NOTE: cannot use System.Diagnostics.Debug.Assert(_nestedIterator.HasNext()); here,
        // because actually HasNext() is a mutating method, and it is required to advance _nextPosition in _nestedIterator even in RELEASE configuration
        if (!_nestedIterator.HasNext())
            return int.MinValue;

        var result = _nestedIterator.Next();
        if (!_nestedIterator.HasNext())
        {
            _nestedIterator = null;
            _nextPosition++;
        }
        return result;
    }
}

/**
 * Your NestedIterator will be called like this:
 * NestedIterator i = new NestedIterator(nestedList);
 * while (i.HasNext()) v[f()] = i.Next();
 */