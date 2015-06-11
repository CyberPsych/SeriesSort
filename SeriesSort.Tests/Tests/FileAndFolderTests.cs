using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Tests.Lib;

namespace SeriesSort.Tests.Tests
{
    public class FileAndFolderTests
    {
        [Test]
        public void ShouldListAllFileInFolder()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldListAllFileInFolder";
            HelperFileFolderTests.CreateTestFolder(testDirectory);

            HelperFileFolderTests.AddTestFileToTestFolder(testDirectory, new List<string> { "ShouldListAllFileInFolder_File1.txt", "ShouldListAllFileInFolder_File2.txt", "ShouldListAllFileInFolder_File3.txt", "ShouldListAllFileInFolder_File4.txt" }, "ShouldListAllFileInFolder");

            var fileCount = Directory.EnumerateFiles(testDirectory).Count();
            Assert.That(fileCount, Is.EqualTo(4));

            HelperFileFolderTests.DisposeTestFolder(testDirectory);
        }

        [Test]
        public void ShouldListAllEpisodesFileInFolder()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldListAllEpisodesFileInFolder";
            HelperFileFolderTests.CreateTestFolder(testDirectory);

            HelperFileFolderTests.AddTestFileToTestFolder(testDirectory, new List<string> { "TestShouldListAllEpisodesFileInFolder.S01E01.mp4", "NotASeries.mp4", "TestShouldListAllEpisodesFileInFolder.S01E02.mp4", "TestShouldListAllEpisodesFileInFolder.S01E03.mp4", "RedHearring.avi", "TestShouldListAllEpisodesFileInFolder.S01E04.mp4", "TestShouldListAllEpisodesFileInFolder.S01E04.nfo" }, "ShouldListAllFileInFolder");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodeHelper = new EpisodesHelper(dbContext);
                var episodeFiles = episodeHelper.GetEpisodeFiles(testDirectory, false);
                var fileCount = episodeFiles.Count();
                Assert.That(fileCount, Is.EqualTo(4));

                dbContext.Series.Remove(episodeFiles[0].Series);
                dbContext.SaveChanges();
                HelperFileFolderTests.DisposeTestFolder(testDirectory);
            }
        }

        [Test]
        public void ShouldMoveEpisodeToLibrary()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldMoveEpisodeToLibrary";
            const string startTestDirectory = testDirectory + @"\Start\";
            const string libraryTestDirectory = testDirectory + @"\Library";
            HelperFileFolderTests.CreateTestFolder(startTestDirectory);
            HelperFileFolderTests.AddTestFileToTestFolder(startTestDirectory, new List<string> { "The UnitTestS01E01.avi" }, "ShouldMoveEpisodeToLibrary");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodeFactory = new EpisodeFactory(dbContext);
                var testEpisode = episodeFactory.CreateNewEpisode(startTestDirectory + @"\The UnitTestS01E01.avi");

                var episodeHelper = new EpisodeHelper(testEpisode);
                episodeHelper.MoveToLibrary(libraryTestDirectory);

                const string expextedPathToMovedFile =
                    libraryTestDirectory + @"\The UnitTest\Season 01\The UnitTest S01E01.avi";
                Assert.That(testEpisode.FullPath, Is.EqualTo(expextedPathToMovedFile));
                Assert.That(File.Exists(expextedPathToMovedFile), Is.True);

                HelperFileFolderTests.DisposeTestFolder(testDirectory);

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldUpdateDbWhenMoveEpisodeToLibrary()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldUpdateDbWhenMoveEpisodeToLibrary";
            const string startTestDirectory = testDirectory + @"\Start\";
            const string libraryTestDirectory = testDirectory + @"\Library";
            HelperFileFolderTests.CreateTestFolder(startTestDirectory);
            HelperFileFolderTests.AddTestFileToTestFolder(startTestDirectory, new List<string> { "The UnitTestS01E01.avi" }, "ShouldMoveEpisodeToLibrary");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodeFactory = new EpisodeFactory(dbContext);
                var testEpisode = episodeFactory.CreateNewEpisode(startTestDirectory + @"\The UnitTestS01E01.avi");
                var creationPath = testEpisode.FullPath;

                var episodeHelper = new EpisodeHelper(testEpisode);
                episodeHelper.MoveToLibrary(libraryTestDirectory);

                dbContext.SaveChanges();

                Assert.That(creationPath, Is.Not.EqualTo(testEpisode.FullPath), "The path should have changed with the move");

                var episodePath = dbContext.Episodes.FirstOrDefault(a => a.EpisodeId == testEpisode.EpisodeId);
                Assert.That(episodePath, Is.Not.Null);
                string expextedPathToMovedFile = episodePath.FullPath;
                Assert.That(testEpisode.FullPath, Is.EqualTo(expextedPathToMovedFile));
                Assert.That(File.Exists(expextedPathToMovedFile), Is.True);

                HelperFileFolderTests.DisposeTestFolder(testDirectory);

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }
    }
}
