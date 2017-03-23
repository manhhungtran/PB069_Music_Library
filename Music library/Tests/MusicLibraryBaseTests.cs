using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using NUnit.Framework;
using SharpFileSystem;
using SharpFileSystem.FileSystems;

namespace MusicLibrary.Tests
{
    public class MusicLibraryBaseTests
    {
        protected const string FilePath1 = @"/a/song1.mp3";
        protected const string FilePath2 = @"/a/song2.mp3";
        protected const string FilePath3 = @"/a/aa/song3.mp3";
        protected const string FilePath4 = @"/b/song4.mp3";
        protected const string FilePath5 = @"/b/bb/song5.mp3";
        protected const string FilePath6 = @"/b/bb/bbb/song6.mp3";
        protected const string FilePath7 = @"/c/cc/song7.mp3";
        protected const string FilePath8 = @"/c/song8.mp3";

        private List<string> _filePaths;
        protected List<Song> _songs;
        protected static IFileSystem FileSystem;

        protected static Song CreateSong(string filePath)
        {
            return Song.New(filePath, FileSystem);
        }

        [SetUp]
        protected void SetUp()
        {
            _filePaths = new List<string>
            {
                FilePath1,
                FilePath2,
                FilePath3,
                FilePath4,
                FilePath5,
                FilePath6,
                FilePath7,
                FilePath8
            };

            _songs = new List<Song>();
            FileSystem = new MemoryFileSystem();

            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("a"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("a").AppendDirectory("aa"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("a").AppendDirectory("aa").AppendDirectory("aaa"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("b"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("b").AppendDirectory("bb"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("b").AppendDirectory("bb").AppendDirectory("bbb"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("c"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("c").AppendDirectory("cc"));
            FileSystem.CreateDirectory(FileSystemPath.Root.AppendDirectory("c").AppendDirectory("cc").AppendDirectory("ccc"));

            foreach (string filePath in _filePaths)
            {
                FileSystemPath fileSystemPath = FileSystemPath.Parse(filePath);
                FileSystem.CreateFile(fileSystemPath);
                _songs.Add(new Song
                {
                    Path = filePath
                });
            }
        }

        [TearDown]
        protected void TearDown()
        {
            FileSystem.Dispose();
            _songs.Clear();
        }
    }
}
