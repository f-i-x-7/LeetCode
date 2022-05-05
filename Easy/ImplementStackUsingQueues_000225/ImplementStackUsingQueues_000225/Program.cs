// https://leetcode.com/problems/implement-stack-using-queues/

//Implement a last-in-first-out (LIFO) stack using only two queues.The implemented stack should support all the functions of a normal stack (push, top, pop, and empty).

//Implement the MyStack class:

//void push(int x) Pushes element x to the top of the stack.
//int pop() Removes the element on the top of the stack and returns it.
//int top() Returns the element on the top of the stack.
//boolean empty() Returns true if the stack is empty, false otherwise.

//Notes:
//You must use only standard operations of a queue, which means that only push to back, peek/pop from front, size and is empty operations are valid.
//Depending on your language, the queue may not be supported natively. You may simulate a queue using a list or deque(double-ended queue) as long as you use only a queue's standard operations.

//Example 1:
//Input
//["MyStack", "push", "push", "top", "pop", "empty"]
//[[], [1], [2], [], [], []]
//Output
//[null, null, null, 2, 2, false]
//Explanation
//MyStack myStack = new MyStack();
//myStack.push(1);
//myStack.push(2);
//myStack.top(); // return 2
//myStack.pop(); // return 2
//myStack.empty(); // return False

//Constraints:
//1 <= x <= 9
//At most 100 calls will be made to push, pop, top, and empty.
//All the calls to pop and top are valid.


var x = new MyStack();
x.Push(1);
x.Push(2);
Console.WriteLine(x.Pop()); // 2
x.Push(3);
Console.WriteLine(x.Pop()); // 3
Console.WriteLine(x.Pop()); // 1
x.Push(4);
Console.WriteLine(x.Top()); // 4
Console.WriteLine(x.Top()); // 4
x.Push(5);
x.Push(6);
x.Push(7);
x.Push(8);
Console.WriteLine(x.Top()); // 8
Console.WriteLine(x.Pop()); // 8
Console.WriteLine(x.Pop()); // 7
Console.WriteLine(x.Pop()); // 6
x.Push(9);
Console.WriteLine(x.Pop()); // 9

public class MyStack
{
    private Queue<int> stackEmulation = new();

    public MyStack()
    {
    }

    public void Push(int x)
    {
        var additionalQueue = new Queue<int>();
        additionalQueue.Enqueue(x); // last added item should be first in stack, so add it to queue beginning
        
        // Continue to maintain LIFO order
        while (stackEmulation.Count > 0)
        {
            var next = stackEmulation.Dequeue();
            additionalQueue.Enqueue(next);
        }

        stackEmulation = additionalQueue;
    }

    public int Pop() => stackEmulation.Dequeue();

    public int Top() => stackEmulation.Peek();

    public bool Empty() => stackEmulation.Count == 0;
}

/**
 * Your MyStack object will be instantiated and called as such:
 * MyStack obj = new MyStack();
 * obj.Push(x);
 * int param_2 = obj.Pop();
 * int param_3 = obj.Top();
 * bool param_4 = obj.Empty();
 */