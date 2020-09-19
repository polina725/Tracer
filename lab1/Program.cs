using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tracer
{
    class Program
    {

        private static ITracer tracer = new Tracer();

        static void Main(string[] args)
        {
            tracer.StartTrace();
            Console.WriteLine("Hello World!");
            Thread threadOfSomeMethod = new Thread(SomeMethod);
            threadOfSomeMethod.Start();
            AnotherMethod();
            threadOfSomeMethod.Join();
            tracer.StartTrace();
            ConcurrentDictionary<int,ThreadInfo> d = tracer.GetTraceResult().GetResults();
            foreach (KeyValuePair<int, ThreadInfo> entity in d) 
            {
                ThreadInfo t = entity.Value;
                Console.WriteLine("[ " + entity.Key + " , " + t.ToString() + " ]" );
            }
        }

        static void SomeMethod(object countObj)
        {
            tracer.StartTrace();
            int count;
            if (countObj == null)
                count = 0;
            else
                count = (int)countObj + 1;
            if (count < 5)
                SomeMethod(count);
            tracer.StartTrace();
        }

        static void AnotherMethod()
        {
            tracer.StartTrace();
            for (int i = 0; i < 5; i++)
                Console.WriteLine(i - 9);
            tracer.StartTrace();
        }

    }
}
