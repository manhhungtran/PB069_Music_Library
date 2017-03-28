using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiLogic;
using Base;
using NUnit.Framework;

namespace MusicLibrary.Tests
{
    public class APITests
    {
        [TestFixture]
        public class LibraryTests
        {
            private const string SOME_PATH = "/somePath";
            private const string SOME_PATH_SUBPATH = "/somePath/subPath";
            private const string SOME_PATH_SUBPATH2 = "/somePath/subsubPath";
            private const string ANOTHER_PATH = "/anotherPath";
            private const string NON_EXISTENT_PATH = "/FAKEpATH";
            private static LibraryManager _manager;

            [SetUp]
            public void SetUp()
            {
                Directory.CreateDirectory(SOME_PATH);
                Directory.CreateDirectory(SOME_PATH_SUBPATH);
                Directory.CreateDirectory(SOME_PATH_SUBPATH2);
                Directory.CreateDirectory(ANOTHER_PATH);
            }

            [TearDown]
            public void TearDown()
            {
                Directory.Delete(SOME_PATH_SUBPATH);
                Directory.Delete(SOME_PATH_SUBPATH2);
                Directory.Delete(SOME_PATH);
                Directory.Delete(ANOTHER_PATH);
            }

            [Test]
            public void CreateLibraryWithoutParams_LibraryIsNotNull()
            {
                _manager = new LibraryManager();
                Assert.NotNull(_manager?._library);
            }

            [Test]
            public void CreateLibraryWithPaths_LibraryContainsPaths()
            {
                var paths = new List<string> {SOME_PATH};
                _manager = new LibraryManager(paths);

                HashSet<string> actualRoots = _manager._library.RootPath;

                CollectionAssert.AreEqual(paths, actualRoots);
            }

            [Test]
            public void CreateLibrary_NullParam_ThrowsNullException()
            {
                Assert.Throws<NullReferenceException>(() => new LibraryManager(null));
            }

            [Test]
            public void CreateLibrary_NonExistentPath_ThrowsException()
            {
                Assert.Throws<DirectoryNotFoundException>(
                    () => new LibraryManager(new List<string>{NON_EXISTENT_PATH}));
            }

            [Test]
            public void AddRootToPath_PathWasAddedToCollectionOfRoots()
            {
                var paths = new List<string> {SOME_PATH};
                _manager = new LibraryManager(paths);

                paths.Add(ANOTHER_PATH);
                _manager.AddRootPath(ANOTHER_PATH);

                HashSet<string> actualRoots = _manager._library.RootPath;

                CollectionAssert.AreEqual(paths, actualRoots);
            }

            [Test]
            public void AddRootToPath_NonExistentAth_ThrowsException()
            {
                _manager = new LibraryManager();
                Assert.Throws<DirectoryNotFoundException>(() => _manager.AddRootPath(NON_EXISTENT_PATH));
            }

            [Test]
            public void AddRootToPath_SubDirectoryOfAlreadyMappedPaths_ThrowsException()
            {
                _manager = new LibraryManager();
                _manager.AddRootPath(SOME_PATH);

                Assert.Throws<DirectoryIsSubPathException>(() => _manager.AddRootPath(SOME_PATH_SUBPATH));
            }

            [Test]
            public void AddRootToPath_ParentDirectoryOfAlreadyMappedPaths_DeletesAllubPAthsAndAddsNewPath()
            {
                _manager = new LibraryManager();
                _manager.AddRootPath(SOME_PATH_SUBPATH2);
                _manager.AddRootPath(SOME_PATH_SUBPATH);
                _manager.AddRootPath(SOME_PATH);

                Assert.Contains(SOME_PATH, _manager._library.RootPath.ToList());
                Assert.False(_manager._library.RootPath.Any(x => x == SOME_PATH_SUBPATH));
                Assert.False(_manager._library.RootPath.Any(x => x == SOME_PATH_SUBPATH2));
            }
        }

        [TestFixture]
        public class PlayListTests: BaseTests
        {
            private IPlaylistManager _playlistManager;
            private Playlist playlist1;
            private Playlist playlist2;

            [SetUp]
            public void SetUp()
            {
                _playlistManager = new PlaylistManager();
                playlist1 = new Playlist
                {
                    Id = 1,
                    Name = "somePlaylist",
                    Songs = new List<string>()
                };

                playlist2 = new Playlist
                {
                    Id = 2,
                    Name = "anotherSomePlaylist",
                    Songs = new List<string>()
                };
            }

            [TearDown]
            public void TearDown()
            {
                _playlistManager = null;
                playlist1 = null;
                playlist2 = null;
            }


            [Test]
            public void CreatePlaylist()
            {
                _playlistManager.CreatePlaylist(playlist2);

                Assert.Contains(playlist2, _playlistManager.GetAllPlaylists().ToList());
            }

            [Test]
            public void EditPlaylist()
            {
                _playlistManager.CreatePlaylist(playlist2);
                playlist2.Name = "a";
                _playlistManager.EditPlaylist(playlist2);

                Assert.AreEqual(playlist2, _playlistManager.GetPlaylistByName("a"));
            }

            [Test]
            public void Gets()
            {
                _playlistManager.CreatePlaylist(playlist1);
                _playlistManager.CreatePlaylist(playlist2);

                Assert.IsEmpty(_playlistManager.GetAllSongsInPlaylist(playlist1.Id));
                Assert.IsEmpty(_playlistManager.GetAllSongsInPlaylist(playlist2.Id));
                Assert.AreEqual(2, _playlistManager.GetAllPlaylists().Count);
            }

            public void Delete()
            {
                _playlistManager.CreatePlaylist(playlist1);
                _playlistManager.CreatePlaylist(playlist2);
                _playlistManager.DeletePlaylist(playlist2.Id);

                Assert.AreEqual(1, _playlistManager.GetAllPlaylists().Count);
            }
        }
    }
}
