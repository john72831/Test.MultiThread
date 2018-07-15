using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Test.MultiThread.Service
{
    public class TestExcutor
    {
        public void ExcuteTest(int count, int round) {
            var typeOfITest = typeof(ITest);

            Func<Type, bool> predicate = t => typeOfITest.IsAssignableFrom(t); // 找到符合IMapFrom<>

            var types = typeOfITest.Assembly.GetExportedTypes().Where(predicate).ToList();

            var tests = (from t in types
                        from i in t.GetInterfaces()
                        where !t.IsAbstract && !t.IsInterface
                        select (ITest)Activator.CreateInstance(t)).ToArray();

            foreach (var test in tests)
            {
                test.BeginTest(count, round);
            }
        }
    }
}
