using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using TracerLib;

namespace SerializersAndDisplayers
{
    public class JsonSerialization : ISerialization
    {
        public string Serialize(TraceResult result)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = "";
                jsonString += JsonSerializer.Serialize(new Wrapper(result), options);

            return jsonString;
        }
    }

    class Wrapper
    {
        [JsonPropertyName("threads")]
        public List<ThreadInfo> threads { get; set; }

        public Wrapper(TraceResult result)
        {
            threads = new List<ThreadInfo>();
            foreach (KeyValuePair<int, ThreadInfo> entity in result.GetResults())
                threads.Add(entity.Value);
        }
    }
}
