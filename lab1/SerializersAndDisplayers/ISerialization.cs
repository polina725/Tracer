using TracerLib;

namespace SerializersAndDisplayers
{
    public interface ISerialization
    {
        public string Serialize(TraceResult result);
    }
}
