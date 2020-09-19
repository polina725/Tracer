using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Tracer
{
    public class Tracer : ITracer
    {
        private TraceResult result;
        public static Tracer instanceOfTracer;

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
            StackTrace stTrace = new StackTrace(true);
            result.StartTracing(Thread.CurrentThread.ManagedThreadId, stTrace.GetFrame(1).GetMethod());
        }

        public void StopTrace()
        {
            result.StopTracing(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
