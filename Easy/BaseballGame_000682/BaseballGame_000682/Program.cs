// https://leetcode.com/problems/baseball-game/

//You are keeping score for a baseball game with strange rules. The game consists of several rounds, where the scores of past rounds may affect future rounds' scores.

//At the beginning of the game, you start with an empty record. You are given a list of strings ops, where ops[i] is the ith operation you must apply to the record and is one of the following:

//An integer x - Record a new score of x.
//"+" - Record a new score that is the sum of the previous two scores. It is guaranteed there will always be two previous scores.
//"D" - Record a new score that is double the previous score.It is guaranteed there will always be a previous score.
//"C" - Invalidate the previous score, removing it from the record.It is guaranteed there will always be a previous score.
//Return the sum of all the scores on the record.




//Example 1:
//Input: ops = ["5", "2", "C", "D", "+"]
//Output: 30
//Explanation:
//"5" - Add 5 to the record, record is now[5].
//"2" - Add 2 to the record, record is now[5, 2].
//"C" - Invalidate and remove the previous score, record is now[5].
//"D" - Add 2 * 5 = 10 to the record, record is now[5, 10].
//"+" - Add 5 + 10 = 15 to the record, record is now[5, 10, 15].
//The total sum is 5 + 10 + 15 = 30.

//Example 2:
//Input: ops = ["5", "-2", "4", "C", "D", "9", "+", "+"]
//Output: 27
//Explanation:
//"5" - Add 5 to the record, record is now[5].
//"-2" - Add -2 to the record, record is now[5, -2].
//"4" - Add 4 to the record, record is now[5, -2, 4].
//"C" - Invalidate and remove the previous score, record is now[5, -2].
//"D" - Add 2 * -2 = -4 to the record, record is now[5, -2, -4].
//"9" - Add 9 to the record, record is now[5, -2, -4, 9].
//"+" - Add -4 + 9 = 5 to the record, record is now[5, -2, -4, 9, 5].
//"+" - Add 9 + 5 = 14 to the record, record is now[5, -2, -4, 9, 5, 14].
//The total sum is 5 + -2 + -4 + 9 + 5 + 14 = 27.

//Example 3:
//Input: ops = ["1"]
//Output: 1



//Constraints:
//1 <= ops.length <= 1000
//ops[i] is "C", "D", "+", or a string representing an integer in the range [-3 * 10^4, 3 * 10^4].
//For operation "+", there will always be at least two previous scores on the record.
//For operations "C" and "D", there will always be at least one previous score on the record.


Test(new[] { "5", "2", "C", "D", "+" }, expectedResult: 30);
Test(new[] { "5", "-2", "4", "C", "D", "9", "+", "+" }, expectedResult: 27);
Test(new[] { "1" }, expectedResult: 1);



static void Test(string[] ops, int expectedResult)
{
    var actualResult = new Solution().CalPoints(ops);
    if (actualResult != expectedResult)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"FAILED: {ToStr(ops)}; ExpectedResult={expectedResult}; ActualResult={actualResult}");
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine($"SUCCESS: {ToStr(ops)}; Result={actualResult}");
    }

    string ToStr(string[] arr) => $"[{string.Join(',', arr)}]";
}



public class Solution
{
    public int CalPoints(string[] ops)
    {
        if (ops is null || ops.Length == 0)
            return 0;

        // O(N) time, O(N) space
        // O(1) space is impossible because there can be a lot of clear operations, so we need to track all scores in record with list.
        // I decided to use singly linked list instead of ArrayList, just to train to work with it + to ensure predictable time of each iteration.
        var records = new List<int>();

        foreach (var op in ops)
        {
            switch (op)
            {
                case "+":
                    System.Diagnostics.Debug.Assert(records.Count >= 2);
                    var sum = records[records.Count - 2] + records[records.Count - 1];
                    records.Add(sum);
                    break;

                case "D":
                    System.Diagnostics.Debug.Assert(records.Count >= 1);
                    var doubled = records[records.Count - 1] * 2;
                    records.Add(doubled);
                    break;

                case "C":
                    System.Diagnostics.Debug.Assert(records.Count >= 1);
                    records.RemoveAt(records.Count - 1);
                    break;

                default:
                    if (int.TryParse(op, out var num))
                        records.Add(num);
                    break;
            }
        }

        return records.Sum();
    }
}
