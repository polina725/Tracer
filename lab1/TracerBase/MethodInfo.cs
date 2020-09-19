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
        private readonly String name;
        private readonly String className;
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

        public void Stop()
        {
            stopwatch.Stop();
            executionTime = stopwatch.ElapsedMilliseconds;
        }

        public override string ToString()
        {
            string str = "";
            foreach (MethodInfo m in methods)
                str += "{ "+ m.name + " , " + className + " }, ";
            return str;
        }
    }
}
