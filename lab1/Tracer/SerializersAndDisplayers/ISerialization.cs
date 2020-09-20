using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer
{
    public interface ISerialization
    {
        public void Serialize(TraceResult result);
    }
}
