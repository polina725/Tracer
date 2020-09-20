using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Xml;
using System.IO;

namespace Tracer
{
    class Program
    {

        static void Main(string[] args)
        {
            ITracer tracer = new Tracer();
            MethodInfo m = new MethodInfo(new StackFrame().GetMethod());
            
            ShortCalcClass c1 = new ShortCalcClass(tracer);
            c1.SimpleMethod();

            LongCalcClass c2 = new LongCalcClass(tracer);
            c2.RecursiveMethod(null);

            Thread threadOfSomeMethod = new Thread(c2.RecursiveMethod);
            threadOfSomeMethod.Start();
            threadOfSomeMethod.Join();
            ConcurrentDictionary<int,ThreadInfo> d = tracer.GetTraceResult().GetResults();
            ISerialization serializer = new CustomXmlSerializer();
            string str = serializer.Serialize(tracer.GetTraceResult());
            serializer = new JsonSerialization();
            string str1 = serializer.Serialize(tracer.GetTraceResult());
            Displayer displ = new Displayer();
            displ.Display(Console.Out, str1+"\n"+str);
            displ.Display(new FileStream("methods.json",FileMode.Create),str1);
            displ.Display(new FileStream("methods.xml",FileMode.Create),str);
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
                Console.WriteLine(count);
                if (count < 5)
                    RecursiveMethod(count);
                if (count == 1)
                {
                    new ShortCalcClass(tracer).AnotherSimpleMethod();
                }
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
            //    AnotherSimpleMethod();
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
