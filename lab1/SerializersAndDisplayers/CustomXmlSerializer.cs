using System.Collections.Generic;
using System.Xml.Linq;
using TracerLib;

namespace SerializersAndDisplayers 
{ 
    public class CustomXmlSerializer : ISerialization
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
                    tmp = SubMethod(entity.Value.GetMethods(), el,true);
                if (tmp != null)
                    root.Add(tmp);
            }
            XDocument doc = new XDocument();
            doc.Add(root);
            return root.ToString();
        }


        private XElement SubMethod(List<MethodInfo> list, XElement parent,bool timeIncluded)
        {
            foreach (MethodInfo item in list)
            {
                XElement el = new XElement("method");
                el.SetAttributeValue("name", item.Name);
                if (timeIncluded)
                    el.SetAttributeValue("time", item.ExecutionTime);   
                el.SetAttributeValue("class", item.ClassName);
                if (item.GetMethodsList().Count != 0)
                    el = SubMethod(item.GetMethodsList(), el,timeIncluded);
                parent.Add(el);
            }
            return parent;
        }

        public string TestSerialize(TraceResult result)
        {
            XElement root = new XElement("root");
            foreach (KeyValuePair<int, ThreadInfo> entity in result.GetResults())
            {
                XElement el = new XElement("thread");
                XElement tmp = null;
                if (entity.Value.GetMethods().Count != 0)
                    tmp = SubMethod(entity.Value.GetMethods(), el,false);
                if (tmp != null)
                    root.Add(tmp);
            }
            XDocument doc = new XDocument();
            doc.Add(root);
            return root.ToString();
        }

    }
}
