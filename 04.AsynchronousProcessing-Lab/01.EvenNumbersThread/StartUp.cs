namespace EvenNumbersThread
{
    using System;
    using System.Linq;
    using System.Threading;

    public class StartUp
    {
        public static void Main()
        {
            var numbers = Console.ReadLine().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var start = numbers[0];
            var end = numbers[1];

            var thread = new Thread(() => PrintNumbers(start, end));
            thread.Start();
            thread.Join();

            Console.WriteLine("Thread finished work!");
        }

        private static void PrintNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
            }
        }
    }
}