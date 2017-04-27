using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;

namespace ApiLogic
{
    public interface ILibraryManager
    {
        void InitializeLibrary();
        void AddRootPath(string directoryPath);
        void RemoveRootPath(string path);
        ISongManager GetSongManager();
        void Save();
        void Load(string filePath);
        List<string> GetAllRoots();
    }
}
