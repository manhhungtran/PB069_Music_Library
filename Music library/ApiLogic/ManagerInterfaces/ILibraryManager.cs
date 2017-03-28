namespace ApiLogic
{
    public interface ILibraryManager
    {
        void InitializeLibrary();
        void AddRootPath(string directoryPath);
        void RemoveRootPath(string path);
    }
}
