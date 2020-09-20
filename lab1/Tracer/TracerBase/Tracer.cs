using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    public class Tracer : ITracer
    {
        private TraceResult result;

        public Tracer()
        {
            result = new TraceResult();
        }

        public TraceResult GetTraceResult()
        {
            return result;
        }

        public void StartTrace()
        {
            result.StartTracing(Thread.CurrentThread.ManagedThreadId, new StackTrace().GetFrame(1).GetMethod());
        }

        public void StopTrace()
        {
            result.StopTracing(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
