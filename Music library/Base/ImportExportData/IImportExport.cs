using System.Collections.Generic;

namespace Base
{
    public interface IImportExport<T> where T : class, new()
    {
        /// <summary>
        /// Imports xml file to defined class
        /// </summary>
        HashSet<T> Import(string sourceFilePath);

        /// <summary>
        /// Creates xml file from given data
        /// </summary>
        /// <param name="data">What should be exported</param>
        /// <param name="destinationDirectoryPath">where it should be exported</param>
        /// <returns></returns>
        string Export(HashSet<T> data, string destinationDirectoryPath);
    }
}
