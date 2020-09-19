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
        private readonly string name;
        private readonly string className;
        private long executionTime;

        public MethodInfo(MethodBase method)
        {
            methods = new List<MethodInfo>();
            name = method.Name;
            className = method.DeclaringType.Name;
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
            return executionTime = stopwatch.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            string str = "{ " + name + " , " + className + " , " + executionTime + " } ";
           foreach (MethodInfo m in methods)
                str += m.ToString();
            return str;
        }
    }
}
