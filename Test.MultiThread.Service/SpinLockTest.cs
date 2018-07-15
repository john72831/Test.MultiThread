using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Test.MultiThread.Service
{
    public class SpinLockTest : ITest
    {
        private int _count;
        private Queue<Data> _queue = new Queue<Data>();
        private Object _lock = new Object();
        private SpinLock _spinlock = new SpinLock();

        public void BeginTest(int count, int round)
        {
            _count = count;

            while ( round > 0)
            {
                UseLock();
                _queue.Clear();
                UseSpinLock();
                round--;
            }
        }

        private void UseSpinLock()
        {
            var sw = Stopwatch.StartNew();

            Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < _count; i++)
                        {
                            UpdateWithSpinLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    },
                    () =>
                    {
                        for (int i = 0; i < _count; i++)
                        {
                            UpdateWithSpinLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    }
                );

            sw.Stop();

            Console.WriteLine("elapsed ms with spinlock: {0}", sw.ElapsedMilliseconds);
        }

        private void UpdateWithSpinLock(Data d, int i)
        {
            bool lockTaken = false;
            try
            {
                _spinlock.Enter(ref lockTaken);
                _queue.Enqueue(d);
            }
            finally
            {
                if (lockTaken) {
                    _spinlock.Exit(false);
                }
            }
        }

        private void UseLock()
        {
            var sw = Stopwatch.StartNew();

            Parallel.Invoke(
                    () =>
                    {
                        for (int i = 0; i < _count; i++)
                        {
                            UpdateWithLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    },
                    () =>
                    {
                        for (int i = 0; i < _count; i++)
                        {
                            UpdateWithLock(new Data() { Name = i.ToString(), Number = i }, i);
                        }
                    }
                );

            sw.Stop();

            Console.WriteLine("elapsed ms with lock: {0}", sw.ElapsedMilliseconds);
        }

        private void UpdateWithLock(Data d, int i)
        {
            lock (_lock)
            {
                _queue.Enqueue(d);
            }
        }
    }

    class Data
    {
        public string Name { get; set; }
        public double Number { get; set; }
    }
}
