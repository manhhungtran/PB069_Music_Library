using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base;
using NUnit.Framework;

namespace MusicLibrary.Tests
{
    public class MusicLibraryBaseTests
    {
        protected List<string> AllFiles;

        [SetUp]
        protected void SetUp()
        {
            AllFiles = Directory.EnumerateFiles("./", "*.mp3", SearchOption.AllDirectories).ToList();
        }

        [TearDown]
        protected void TearDown()
        {
            AllFiles.Clear();
            if (File.Exists($"{nameof(Song)}.xml"))
            {
                File.Delete($"{nameof(Song)}.xml");

            }
        }
    }
}
