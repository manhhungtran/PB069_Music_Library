using System.IO;
using Base;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    public class UnitTests
    {
        [TestFixture]
        public class SongTests
        {
            [SetUp]
            public void SetUp()
            {

            }

            public void New_ReturnsCorrectlyMappedFile()
            {

            }


            [Test]
            public void New_NonExistentFile_ThrowsFileNotFoundException()
            {
                string filePath = "askjgaskjnvasg.mp3";
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                Assert.Throws<FileNotFoundException>(() => Song.New(filePath));
            }

            [TearDown]
            public void TearDown()
            {

            }
        }
    }
}
