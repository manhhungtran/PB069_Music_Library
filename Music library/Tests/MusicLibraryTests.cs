using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Base;

using NUnit.Framework;
using SharpFileSystem;
using SharpFileSystem.IO;

namespace MusicLibrary.Tests
{
    public class MusicLibraryTests
    {
        [TestFixture]
        public class SongTests : MusicLibraryBaseTests
        {
            [Test]
            public void New_StandartFile_ReturnsCorrectlyMappedFile()
            {
                var song = CreateSong(FilePath1);

                Assert.AreEqual(FilePath1, song.Name);
                Assert.AreEqual(FilePath1, song.Path);
            }


            [Test]
            public void New_NonExistentFile_ThrowsFileNotFoundException()
            {
                if (FileSystem.Exists(FileSystemPath.Parse(FilePath1)))
                {
                    FileSystem.Delete(FileSystemPath.Parse(FilePath1));
                }

                Assert.Throws<FileNotFoundException>(() => Song.New(FilePath1));
            }
        }

        [TestFixture]
        public class XMLImportExportTests : MusicLibraryBaseTests
        {
            [Test]
            public void Export_NewFileCreated()
            {
                var worker = new XMLImportExport<Song>(FileSystem);

                worker.Export(_songs);

                Assert.IsTrue(FileSystem.Exists(FileSystemPath.Root.AppendFile($"{nameof(List<Song>)}.xml")));
            }

            [Test]
            public void Export_NewFileWithExpectedContenCreated()
            {
                var worker = new XMLImportExport<Song>(FileSystem);

                worker.Export(_songs);
                var expected = new StringBuilder();
                expected.Append("<?xml version=\"1.0\"?>\r\n<ArrayOfSong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                foreach (var song in _songs)
                {
                    expected.AppendFormat("\r\n  <Song>\r\n    <Path>{0}</Path>\r\n    <Lenght>0</Lenght>\r\n    <Year>0</Year>\r\n  </Song>", song.Path);
                }
                expected.Append("\r\n</ArrayOfSong>");

                Assert.AreEqual(expected.ToString(), FileSystem.OpenFile(FileSystemPath.Root.AppendFile($"{nameof(List<Song>)}.xml"), FileAccess.Read).ReadAllText());
            }
        }
    }
}
