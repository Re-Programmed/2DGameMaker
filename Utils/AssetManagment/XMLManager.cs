using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace _2DGameMaker.Utils.AssetManagment
{
    public static class XMLManager
    {
        public static string ToXML(object encode)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(encode.GetType());
                serializer.Serialize(stringwriter, encode);
                return stringwriter.ToString();
            }
        }

        public static T LoadFromXMLString<T>(string xmlText)
        {
            using (var stringReader = new System.IO.StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }
    }
}
