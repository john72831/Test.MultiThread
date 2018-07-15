using System;
using System.Collections.Generic;
using System.Text;

namespace Test.MultiThread.Service
{
    public interface ITest
    {
        void BeginTest(int count, int round);
    }
}
