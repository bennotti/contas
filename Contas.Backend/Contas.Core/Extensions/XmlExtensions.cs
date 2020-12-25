using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Contas.Core.Extensions
{
    public static class XmlExtensions
    {
        public static string ToXML<T>(this T obj)
        {
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding
            };

            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, obj);
                }
                return encoding.GetString(stream.ToArray());
            }

        }
        public static T XmlToObject<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            T result;

            using (TextReader reader = new StringReader(xml))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }


        //public static T LoadFromXMLString<T>(this string xmlText)
        //{
        //    using (var stringReader = new System.IO.StringReader(xmlText))
        //    {
        //        var serializer = new XmlSerializer(typeof(T));
        //        return serializer.Deserialize(stringReader) as T;
        //    }
        //}
    }
}
