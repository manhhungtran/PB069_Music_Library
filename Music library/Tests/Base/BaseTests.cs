using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Base;

using NUnit.Framework;

namespace MusicLibrary.Tests
{
    public class BaseTests
    {
        [TestFixture]
        public class SongTests : MusicLibraryBaseTests
        {
            [Test]
            public void New_NonExistentFile_ThrowsFileNotFoundException()
            {
                Assert.Throws<FileNotFoundException>(() => Song.New("fakefile.mp3"));
            }
        }

        [TestFixture]
        public class XMLImportExportTests : MusicLibraryBaseTests
        {
            [Test]
            public void Export_NewFileCreated()
            {
                string fileName = $"/{nameof(Song)}.xml";

                new XMLImportExport<Song>().Export(new HashSet<Song>(), fileName);

                string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                Assert.IsTrue(File.Exists(absolutePath));
            }

            [Test]
            public void Export_NewFileWithExpectedContenCreated()
            {
                string fileName = $"/{nameof(Song)}.xml";
                var worker = new XMLImportExport<Song>();
                var songs = new HashSet<Song>()
                {
                    new Song {Path = "Somepathlol"},
                    new Song {Path = "Somepathlol2"}
                };

                worker.Export(songs, fileName);
                var expected = new StringBuilder();

                expected.Append("<?xml version=\"1.0\"?>\r\n<ArrayOfSong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                foreach (var song in songs)
                {
                    expected.AppendFormat("\r\n  <Song>\r\n    <Path>{0}</Path>\r\n    <Lenght>0</Lenght>\r\n    <Year>0</Year>\r\n  </Song>", song.Path);
                }
                expected.Append("\r\n</ArrayOfSong>");

                string actual;
                using (var reader = new StreamReader(fileName))
                {
                    actual = reader.ReadToEnd();
                }

                Assert.AreEqual(expected.ToString(), actual);
            }

            [Test]
            public void Import_ValidFile_ReturnsCorrectlyDeserealizedSongs()
            {
                string fileName = $"/{nameof(Song)}.xml";
                var worker = new XMLImportExport<Song>();
                var songs = new HashSet<Song>
                {
                    new Song {Path = "Somepathlol"},
                    new Song {Path = "Somepathlol2"}
                };

                worker.Export(songs, fileName);
                string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                var actual = worker.Import(absolutePath);

                CollectionAssert.AreEqual(songs, actual);
            }
        }

        [TestFixture]
        public class StringExtensionsTest
        {
            [TestCase(@"c:\foo", @"c:\", true)]
            [TestCase(@"c:\foo", @"c:", true)]
            [TestCase(@"c:\foo", @"c:\foo\", true)]
            [TestCase(@"c:\foo", @"c:\foo", true)]
            [TestCase(@"c:\foo\bar\", @"c:\foo\", true)]
            [TestCase(@"c:\foo\", @"c:\foo", true)]
            [TestCase(@"c:\foo\bar", @"c:\foo\", true)]
            [TestCase(@"c:\FOO\a.txt", @"c:\foo", true)]
            [TestCase(@"c:/foo/a.txt", @"c:\foo", true)]
            [TestCase(@"c:\foo\a.txt", @"c:\foo", true)]
            [TestCase(@"c:\foobar\a.txt", @"c:\foo", false)]
            [TestCase(@"c:\foobar", @"c:\foo", false)]
            [TestCase(@"c:\foobar\a.txt", @"c:\foo\", false)]
            [TestCase(@"c:\foo\a.txt", @"c:\foobar", false)]
            [TestCase(@"c:\foo\..\bar\baz", @"c:\bar", true)]
            [TestCase(@"c:\foo\a.txt", @"c:\foobar\", false)]
            [TestCase(@"c:\foo\..\bar\baz", @"c:\foo", false)]
            [TestCase(@"c:\foo\..\bar\baz", @"c:\barr", false)]
            public void IsSubPathOfTest(string path, string baseDirPath, bool expectedResult)
            {
                Assert.AreEqual(expectedResult, path.IsSubPathOf(baseDirPath));
            }
        }
    }
}
