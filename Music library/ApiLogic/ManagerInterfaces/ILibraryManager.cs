using System.Collections.Generic;

namespace ApiLogic
{
    public interface ILibraryManager
    {
        void InitializeLibrary(IEnumerable<string> paths);
        void AddRootPath(string path);
        void RemoveRootPath(string path);
    }
}
