using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    public class MethodInfo
    {
        private readonly List<MethodInfo> methods;
        private Stopwatch stopwatch;

        internal string Name { get; }
        internal string ClassName { get; }
        internal long ExecutionTime { get; private set; }

        public MethodInfo(MethodBase method)
        {
            methods = new List<MethodInfo>();
            Name = method.Name;
            ClassName = method.DeclaringType.Name;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void Add(MethodInfo method)
        {
            methods.Add(method);
        }

        public long Stop()
        {
            stopwatch.Stop();
            return ExecutionTime = stopwatch.ElapsedMilliseconds;
        }  
        
        public List<MethodInfo> GetMethodsList()
        {
            return methods;
        }
    }
}
