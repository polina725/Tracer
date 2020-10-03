using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TracerLib;
using SerializersAndDisplayers;
using System.Collections.Generic;

namespace Testing
{
    [TestClass]
    public class TracerTest
    {

        private ITracer tracer;
        private CustomXmlSerializer serializer;
        private Program.ShortCalcClass shortCalc;
        private Program.LongCalcClass longCalc;
        private Thread[] threads;

        [TestInitialize]
        public void Setup()
        {
            tracer = new Tracer();

            shortCalc = new Program.ShortCalcClass(tracer);

            longCalc = new Program.LongCalcClass(tracer);

            serializer = new CustomXmlSerializer();
            threads = new Thread[2] { new Thread(longCalc.LongRecursiveMethod), new Thread(shortCalc.SimpleMethodWithAnotherSimleMethod) };
        }

        [TestMethod]
        public void OneThreadTestWithShortRecursion()
        {
            longCalc.ShortRecursiveMethod(null);
            Assert.AreEqual(1, tracer.GetTraceResult().GetResults().Count);
            Assert.AreEqual(File.ReadAllText("OneThreadTestWithShortRecursion.txt"), serializer.TestSerialize(tracer.GetTraceResult()));
        }

        [TestMethod]
        public void TwoThreadTest()
        {
            shortCalc.SimpleMethod();
            threads[1].Start();
            threads[1].Join();
            Assert.AreEqual(2, tracer.GetTraceResult().GetResults().Count);
            Assert.AreEqual(File.ReadAllText("TwoThreadTest.txt"), serializer.TestSerialize(tracer.GetTraceResult()));
        }

        [TestMethod]
        public void ThreeThreadTest()
        {
            shortCalc.SimpleMethodWithShortRecursiveMethod();
            threads[0].Start();
            threads[1].Start();
            threads[0].Join();
            threads[1].Join();
            Assert.AreEqual(3, tracer.GetTraceResult().GetResults().Count);
        }

        [TestMethod]
        public void OneThreadWithTimeChecking()
        {
            longCalc.ShortRecursiveMethod(null);
            string xml = serializer.Serialize(tracer.GetTraceResult());
            bool timeCorrect = true;
            foreach(var pair in tracer.GetTraceResult().GetResults())
                if (pair.Value.Methods.Count != 0)
                    CheckTime(pair.Value.Methods,timeCorrect);
            Assert.IsTrue(timeCorrect);
        }

        private bool CheckTime(List<MethodInfo> methodsList,bool timeCorrect)
        {
            foreach(MethodInfo m in methodsList)
            {
                if (m.Methods.Count != 0)
                    CheckTime(m.Methods,timeCorrect);
                if (m.ExecutionTime < 10 || !timeCorrect)
                    return timeCorrect && false;
            }
            return timeCorrect && true;
        }
    }
}
