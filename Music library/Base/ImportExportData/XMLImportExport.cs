using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Base
{
    /// <summary>
    /// XML Helper class
    /// </summary>
    public class XMLImportExport<T> : IImportExport<T> where T : class, new()
    {
        public HashSet<T> Import(string sourceFilePath)
        {
            HashSet<T> result;

            using (StreamReader reader = new StreamReader(sourceFilePath))
            {
                var serializer = new XmlSerializer(typeof(HashSet<T>));
                result = serializer.Deserialize(reader) as HashSet<T>;
            }

            return result;
        }

        public string Export(HashSet<T> data, string destinationPath)
        {
            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }

            using (FileStream writer = File.OpenWrite(destinationPath))
            {
                var xmlSerializer = new XmlSerializer(typeof(HashSet<T>));
                xmlSerializer.Serialize(writer, data);
            }

            return destinationPath;
        }
    }
}
