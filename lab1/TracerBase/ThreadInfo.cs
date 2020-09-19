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

        private long lifeTime;

        public ThreadInfo()
        {
            methods = new List<MethodInfo>();
            callStack = new Stack<MethodInfo>();
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
            callStack.Peek().Stop();
            callStack.Pop();
        }

        public override string ToString()
        {
            string str = "";
            foreach (MethodInfo m in methods)
                str += m.ToString();
            return str;
        }
    }
}
