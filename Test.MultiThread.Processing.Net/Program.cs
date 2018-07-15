using System;
using System.Reflection;
using Test.MultiThread.Service;

namespace Test.MultiThread.Processing.Net
{
    class Program
    {
        static void Main(string[] args)
        {
            var testExcutor = new TestExcutor();
            testExcutor.ExcuteTest(100_000, 3);

            Console.WriteLine("Press a key");
            Console.ReadKey();
        }
    }
}
