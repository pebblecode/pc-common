using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using PebbleCode.Framework.IoC;

namespace PebbleCode.Framework.Utilities
{
    public static class SerialisationUtils
    {
        /// <summary>
        /// Deserialise an object from xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml)
            where T : class
        {
            if (xml == null) return null;

            // Legacy supoort for old CT PropertyBag instances that were serialised to the db.
            // The class is identical and so can just be deserialised as if it were a PebbleCode PropertyBag.
            xml = xml.Replace("<PropertyBag xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/CT.Framework.Collections\">",
                "<PropertyBag xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/PebbleCode.Framework.Collections\">");

            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    return (T)dcs.ReadObject(xr);
                }
            }
        }

        /// <summary>
        /// Serilaise an object to XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToXml<T>(T data)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            StringBuilder output = new StringBuilder();
            using (XmlWriter xw = XmlWriter.Create(output))
            {
                dcs.WriteObject(xw, data);
            }
            return output.ToString();
        }

        /// <summary>
        /// Deserialise an object from binary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static T FromBinary<T>(byte[] binary)
        {
            if (binary == null)
                return default(T);

            BinaryFormatter bf = new BinaryFormatter();
            bf.Binder = Kernel.Get<SerializationBinder>();
            
            // GPJ: You can attach a binder here if you want to see what's being deserialised.
            // Can be useful for debugging serialisation problems.
            // Create a new class derived from SerializationBinder and attach here...
            // bf.Binder = new DebugBinder();

            using (MemoryStream ms = new MemoryStream(binary))
            {
                return (T)bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Serilaise an object to binary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToBinary<T>(T data)
        {
            if (data == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, data);
                return ms.ToArray();
            }
        }
    }
}
