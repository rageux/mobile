using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RSSreader.Helpers
{
    public class StorageHelper
    {
        public static string SerializeListToXml(List<string> List)
        {
            try
            {
                XmlSerializer xmlIzer = new XmlSerializer(typeof(List<string>));
                var writer = new StringWriter();
                xmlIzer.Serialize(writer, List);
                return writer.ToString();
            }

            catch (Exception exc)
            {
                return String.Empty;
            }
        }

        public static List<string> DeserializeXmlToList(string listAsXml)
        {
            try
            {
                XmlSerializer xmlIzer = new XmlSerializer(typeof(List<string>));
                XmlReader xmlRead = XmlReader.Create(new StringReader(listAsXml));
                List<string> myList = new List<string>();
                myList = (xmlIzer.Deserialize(xmlRead)) as List<string>;
                return myList;
            }

            catch (Exception exc)
            {
                List<string> emptyList = new List<string>();
                return emptyList;
            }
        }

        public static List<string> GetStoredURL()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            Object value = localSettings.Values["storedURL"];

            if (value == null)
                return null;
            else
                return DeserializeXmlToList((string)value);
        }

        public static List<string> GetStoredName()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            Object value = localSettings.Values["name"];

            if (value == null)
                return null;
            else
                return DeserializeXmlToList((string)value);
        }

        public static void StoreURL(List<string> list)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["storedURL"] = SerializeListToXml(list);
        }

        public static void StoreName(List<string> list)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            localSettings.Values["name"] = SerializeListToXml(list);
        }

    }
}

