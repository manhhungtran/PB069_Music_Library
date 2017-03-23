using System.Collections.Generic;

namespace Base
{
    public interface IImportExport<T> where T : class, new()
    {
        List<T> Import();

        void Export(List<T> data);
    }
}
