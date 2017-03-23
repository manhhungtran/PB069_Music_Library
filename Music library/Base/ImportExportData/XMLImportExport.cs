using System;
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
        public List<T> Import()
        {
            throw new NotImplementedException();
        }

        public string Export(List<T> data, string destinationPath, string fileName)
        {
            string filePath = Path.Combine(destinationPath, fileName);

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
