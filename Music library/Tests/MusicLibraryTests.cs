using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Base;

using NUnit.Framework;

namespace MusicLibrary.Tests
{
    public class MusicLibraryTests
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
                string fileName = $"{nameof(Song)}.xml";

                new XMLImportExport<Song>().Export(new List<Song>(), AppDomain.CurrentDomain.BaseDirectory, fileName);

                string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                Assert.IsTrue(File.Exists(absolutePath));
            }

            [Test]
            public void Export_NewFileWithExpectedContenCreated()
            {
                string fileName = $"{nameof(Song)}.xml";
                var worker = new XMLImportExport<Song>();
                var songs = new List<Song>
                {
                    new Song {Path = "Somepathlol"},
                    new Song {Path = "Somepathlol2"}
                };

                worker.Export(songs, AppDomain.CurrentDomain.BaseDirectory, fileName);
                var expected = new StringBuilder();

                expected.Append("<?xml version=\"1.0\"?>\r\n<ArrayOfSong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                foreach (var song in songs)
                {
                    expected.AppendFormat("\r\n  <Song>\r\n    <Path>{0}</Path>\r\n    <Lenght>0</Lenght>\r\n    <Year>0</Year>\r\n  </Song>", song.Path);
                }
                expected.Append("\r\n</ArrayOfSong>");

                string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

                string actual;
                using (var reader = new StreamReader(absolutePath))
                {
                    actual = reader.ReadToEnd();
                }

                Assert.AreEqual(expected.ToString(), actual);
            }

            [Test]
            public void Import_ValidFile_ReturnsCorrectlyDeserealizedSongs()
            {

            }
        }
    }
}
