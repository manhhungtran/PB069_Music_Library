using System.Collections.Generic;

namespace Base
{
    public interface IImportExport<T> where T : class, new()
    {
        List<T> Import();

        string Export(List<T> data, string destinationPath, string fileName);
    }
}
