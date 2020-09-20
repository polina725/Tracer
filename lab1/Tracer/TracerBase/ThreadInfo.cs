using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tracer
{
    public class ThreadInfo
    {
        private List<MethodInfo> methods;
        private Stack<MethodInfo> callStack;

        internal long LifeTime { get; private set; }

        public ThreadInfo()
        {
            methods = new List<MethodInfo>();
            callStack = new Stack<MethodInfo>();
            LifeTime = 0;
        }

        public void StartTracingMethod(MethodInfo method) 
        {

            if (callStack.Count == 0)
                methods.Add(method);
            else
                callStack.Peek().Add(method);
            callStack.Push(method);
        }

        public void StopTracingMethod()
        {
            LifeTime += callStack.Peek().Stop();
            callStack.Pop();
        }

        public List<MethodInfo> GetMethods()
        {
            return methods;
        }
    }
}
