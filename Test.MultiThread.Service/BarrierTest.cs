using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.MultiThread.Service
{
    public class BarrierTest : ITest
    {
        private int _count;
        static string[] words1 = new string[] { "brown", "jumped", "haha", "yaya" };
        static string[] words2 = new string[] { "the", "fox", "quick", "gogo" };
        static string[] words3 = new string[] { "dog", "lazy", "the", "over" };
        static string[] words4 = new string[] { "dog", "lazy", "the", "over" };
        static string solution = "brown jumped haha yaya gogo the quick fox over the lazy dog over the lazy dog.";

        static bool success = false;

        public void BeginTest(int count, int round)
        {
            _count = count;

            while (round > 0)
            {
                var t1 = Task.Run(() => Solve(words1));
                var t2 = Task.Run(() => Solve(words2));
                var t3 = Task.Run(() => Solve(words3));
                var t4 = Task.Run(() => Solve(words4));
                Task.WaitAll(t1, t2, t3, t4);
                success = false;
                round--;
            }
        }

        private void Solve(string[] wordArray)
        {
            while (success == false)
            {
                var random = new Random();

                for (int i = wordArray.Length - 1; i > 0; i--)
                {
                    int swapIndex = random.Next(i + 1);
                    string temp = wordArray[i];
                    wordArray[i] = wordArray[swapIndex];
                    wordArray[swapIndex] = temp;
                }

                // We need to stop here to examine results
                // of all thread activity. This is done in the post-phase
                // delegate that is defined in the Barrier constructor.
                barrier.SignalAndWait();

                barrier2.SignalAndWait();
            }
        }

        static Barrier barrier = new Barrier(4, (b) =>
        {
            var sb = new StringBuilder();

            for (int i = 0; i < words1.Length; i++)
            {
                sb.Append(words1[i]);
                sb.Append(" ");
            }

            for (int i = 0; i < words2.Length; i++)
            {
                sb.Append(words2[i]);
                sb.Append(" ");
            }

            for (int i = 0; i < words3.Length; i++)
            {
                sb.Append(words3[i]);
                sb.Append(" ");
            }

            for (int i = 0; i < words4.Length; i++)
            {
                sb.Append(words4[i]);

                if (i < words4.Length - 1)
                    sb.Append(" ");
            }

            sb.Append(".");
#if TRACE
            System.Diagnostics.Trace.WriteLine(sb.ToString());
#endif
            Console.CursorLeft = 0;
            Console.Write("Current phase1: {0}", barrier.CurrentPhaseNumber);

            if (String.CompareOrdinal(solution, sb.ToString()) == 0)
            {
                success = true;
                Console.WriteLine(Environment.NewLine + "The solution was found in {0} attempts", barrier.CurrentPhaseNumber);
            }
        });

        static Barrier barrier2 = new Barrier(4, (b) =>
        {
            Console.CursorLeft = 0;
            Console.Write("Current phase2: {0}", barrier2.CurrentPhaseNumber);
        });
    }
}
