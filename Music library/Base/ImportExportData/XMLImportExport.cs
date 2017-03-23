using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Base
{
    /// <summary>
    /// XML Helper class
    /// </summary>
    internal class XMLImportExport<T> : IImportExport<T> where T : class, new()
    {
        public List<T> Import(string sourceFilePath)
        {
            List<T> result;

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (StreamReader reader = new StreamReader(sourceFilePath))
            {
                result = (List<T>) serializer.Deserialize(reader);
            }
            return result;
        }

        public string Export(List<T> data, string destinationDirectoryPath, string fileName)
        {
            string filePath = Path.Combine(destinationDirectoryPath, fileName);

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            using (var reader = File.OpenWrite(filePath))
            {
                xmlSerializer.Serialize(reader, data);
            }

            return filePath;
        }
    }
}
