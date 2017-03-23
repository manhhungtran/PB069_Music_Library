using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SharpFileSystem;

namespace Base
{
    internal class XMLImportExport<T> : IImportExport<T> where T : class, new()
    {
        private readonly IFileSystem _fileSystem;

        public XMLImportExport(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }


        public List<T> Import()
        {
            throw new NotImplementedException();
        }

        private StringWriter XMLSerialize(object o)
        {
            var xmlSerializer = new XmlSerializer(o.GetType());
            var xml = new StringWriter();
            xmlSerializer.Serialize(xml, o);

            return xml;
        }


        public void Export(List<T> data)
        {
            string fileName = $"{nameof(List<T>)}.xml";
            FileSystemPath fileSystemPath = FileSystemPath.Root.AppendFile(fileName);

            if (!_fileSystem.Exists(fileSystemPath))
            {
                _fileSystem.CreateFile(fileSystemPath);
            }

            var xmlSerializer = new XmlSerializer(typeof(List<T>));
            xmlSerializer.Serialize(_fileSystem.OpenFile(fileSystemPath, FileAccess.Write), data);
        }
    }
}
