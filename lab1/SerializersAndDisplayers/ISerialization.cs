using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer
{
    public interface ISerialization
    {
        public string Serialize(TraceResult result);
    }
}
