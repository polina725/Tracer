using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Tracer 
{ 
    class CustomXmlSerializer : ISerialization
    {

        public string Serialize(TraceResult result)
        {
            XElement root = new XElement("root");
            foreach (KeyValuePair<int, ThreadInfo> entity in result.GetResults())
            {
                XElement el = new XElement("thread");
                XElement tmp = null;
                el.SetAttributeValue("id", entity.Key);
                el.SetAttributeValue("time", entity.Value.LifeTime);
                if (entity.Value.GetMethods().Count != 0)
                    tmp = SubMethod(entity.Value.GetMethods(), el);
                if (tmp != null)
                    root.Add(tmp);
            }
            XDocument doc = new XDocument();
            doc.Add(root);
            return root.ToString();
        }


        private XElement SubMethod(List<MethodInfo> list, XElement parent)
        {
            foreach (MethodInfo item in list)
            {
                XElement el = new XElement("method");
                el.SetAttributeValue("name", item.Name);
                el.SetAttributeValue("time", item.ExecutionTime);
                el.SetAttributeValue("class", item.ClassName);
                if (item.GetMethodsList().Count != 0)
                    el = SubMethod(item.GetMethodsList(), el);
                parent.Add(el);
            }
            return parent;
        }
    }
}
