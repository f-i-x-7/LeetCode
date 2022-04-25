﻿// https://leetcode.com/problems/peeking-iterator/

//Design an iterator that supports the peek operation on an existing iterator in addition to the hasNext and the next operations.

//Implement the PeekingIterator class:

//PeekingIterator(Iterator<int> nums) Initializes the object with the given integer iterator iterator.
//int next() Returns the next element in the array and moves the pointer to the next element.
//boolean hasNext() Returns true if there are still elements in the array.
//int peek() Returns the next element in the array without moving the pointer.
//Note: Each language may have a different implementation of the constructor and Iterator, but they all support the int next() and boolean hasNext() functions.

//Example 1:
//Input
//["PeekingIterator", "next", "peek", "next", "next", "hasNext"]
//[[[1, 2, 3]], [], [], [], [], []]
//Output
//[null, 1, 2, 2, 3, false]

//Explanation
//PeekingIterator peekingIterator = new PeekingIterator([1, 2, 3]); // [1,2,3]
//peekingIterator.next();    // return 1, the pointer moves to the next element [1,2,3].
//peekingIterator.peek();    // return 2, the pointer does not move [1,2,3].
//peekingIterator.next();    // return 2, the pointer moves to the next element [1,2,3]
//peekingIterator.next();    // return 3, the pointer moves to the next element [1,2,3]
//peekingIterator.hasNext(); // return False

//Constraints:

//1 <= nums.length <= 1000
//1 <= nums[i] <= 1000
//All the calls to next and peek are valid.
//At most 1000 calls will be made to next, hasNext, and peek.

//Follow up: How would you extend your design to be generic and work with all types, not just integer?


Console.WriteLine("Hello, World!");




// C# IEnumerator interface reference:
// https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator?view=netframework-4.8

class PeekingIterator
{
    private readonly IEnumerator<int> iterator;
    private bool lastMoveNextResult;

    // iterators refers to the first element of the array.
    public PeekingIterator(IEnumerator<int> iterator)
    {
        this.iterator = iterator;
        // Need to reset iterator to position before array start
        // (we need to call MoveNext() and determine whether array contains any elements; this is an extension to task definition - it states that array is not empty)
        this.iterator.Reset();
        lastMoveNextResult = this.iterator.MoveNext();
    }

    // Returns the next element in the iteration without advancing the iterator.
    public int Peek()
    {
        return iterator.Current;
    }

    // Returns the next element in the iteration and advances the iterator.
    public int Next()
    {
        var result = iterator.Current;
        lastMoveNextResult = iterator.MoveNext();
        return result;
    }

    // Returns false if the iterator is refering to the end of the array of true otherwise.
    public bool HasNext()
    {
        return lastMoveNextResult;
    }
}