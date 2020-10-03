using System;
using System.Threading;
using System.IO;

using SerializersAndDisplayers;
using System.Diagnostics;

namespace TracerLib
{
    public class Program
    {

        static void Main(string[] args)
        {
            ITracer tracer = new Tracer();

            Thread[] threads = {
                                    new Thread(new LongCalcClass(tracer).ShortRecursiveMethod),
                                    new Thread(new LongCalcClass(tracer).LongRecursiveMethod),
                                    new Thread(new ShortCalcClass(tracer).SimpleMethodWithAnotherSimleMethod)
                                };
            foreach (Thread th in threads)
                th.Start();
            ShortCalcClass c1 = new ShortCalcClass(tracer);
            c1.SimpleMethod();

            LongCalcClass c2 = new LongCalcClass(tracer);
            c2.ShortRecursiveMethod(null);

            foreach (Thread th in threads)
                th.Join();

            ISerialization serializer = new CustomXmlSerializer();
            string xmlStr = serializer.Serialize(tracer.GetTraceResult());

            serializer = new JsonSerialization();
            string jsonStr = serializer.Serialize(tracer.GetTraceResult());

            Displayer displ = new Displayer();
            displ.Display(Console.Out, jsonStr + "\n" + xmlStr);
            displ.Display(new FileStream("methods.json", FileMode.Create), jsonStr);
            displ.Display(new FileStream("methods.xml", FileMode.Create), xmlStr);
        }

        public class LongCalcClass
        {
            private ITracer tracer;

            public LongCalcClass(ITracer tracerObj)
            {
                tracer = tracerObj;
            }

            public void ShortRecursiveMethod(object countObj)
            {
                tracer.StartTrace();
                Thread.Sleep(10);
                int count;
                if (countObj == null)
                    count = 0;
                else
                    count = (int)countObj + 1;
                if (count < 5)
                    ShortRecursiveMethod(count);
                tracer.StopTrace();
            }

            public void LongRecursiveMethod(object countObj)
            {
                tracer.StartTrace();
                Thread.Sleep(2);
                int count;
                if (countObj == null)
                    count = 0;
                else
                    count = (int)countObj + 1;
                if (count % 2 == 0)
                    new ShortCalcClass(tracer).AnotherSimpleMethod();
                if (count < 5)
                {
                    Console.WriteLine("work " + count);
                    LongRecursiveMethod(count);
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
                Thread.Sleep(20);
                tracer.StopTrace();
            }

            public void SimpleMethodWithAnotherSimleMethod()
            {
                tracer.StartTrace();
                AnotherSimpleMethod();
                Thread.Sleep(50);
                tracer.StopTrace();
            }

            public void AnotherSimpleMethod()
            {
                tracer.StartTrace();
                Thread.Sleep(5);
                tracer.StopTrace();
            }

            public void SimpleMethodWithShortRecursiveMethod()
            {
                tracer.StartTrace();
                new LongCalcClass(tracer).ShortRecursiveMethod(null);
                Thread.Sleep(10);
                tracer.StopTrace();
            }
        }

    }
}
