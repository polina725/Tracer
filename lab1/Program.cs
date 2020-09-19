using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tracer
{
    class Program
    {

        static void Main(string[] args)
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            ITracer tracer = new Tracer();

            ShortCalcClass c1 = new ShortCalcClass(tracer);
            c1.SimpleMethod();

            LongCalcClass c2 = new LongCalcClass(tracer);
            c2.RecursiveMethod(null);

            Thread threadOfSomeMethod = new Thread(c2.RecursiveMethod);
            threadOfSomeMethod.Start();
            threadOfSomeMethod.Join();
            ConcurrentDictionary<int,ThreadInfo> d = tracer.GetTraceResult().GetResults();
            foreach (KeyValuePair<int, ThreadInfo> entity in d) 
            {
                ThreadInfo t = entity.Value;
                Console.WriteLine("[ " + entity.Key + " , " + t.ToString() + " ]" );
            }
        }

        public class LongCalcClass
        {
            private ITracer tracer;

            public LongCalcClass(ITracer tracerObj)
            {
                tracer = tracerObj;
            }

            public void RecursiveMethod(object countObj)
            {
                tracer.StartTrace();
                int count;
                if (countObj == null)
                    count = 0;
                else
                    count = (int)countObj + 1;
                if (count == 1)
                {
                    new ShortCalcClass(tracer).AnotherSimpleMethod();
                }
                Console.WriteLine(count);
                if (count < 5)
                    RecursiveMethod(count);
                tracer.StopTrace();
            }
        }

        public class ShortCalcClass
        {
            private ITracer tracer;

            public ShortCalcClass(ITracer tracerObj)
            {
                tracer = tracerObj;
            }

            public void SimpleMethod()
            {
                tracer.StartTrace();
                Thread.Sleep(10);
                tracer.StopTrace();
            }

            public void AnotherSimpleMethod()
            {
                tracer.StartTrace();
                Thread.Sleep(50);
                tracer.StopTrace();
            }
        }

    }
}
