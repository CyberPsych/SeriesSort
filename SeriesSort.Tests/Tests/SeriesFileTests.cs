using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Model.Interface;
using SeriesSort.Tests.Lib;
using System.Collections.Generic;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    public class SeriesFileTests
    {

        [TestCase("SESAENTest.S03E01.avi", 3, 1, "SESAENTest")]
        [TestCase("SESAENTestS01E02.avi", 1, 2, "SESAENTest")]
        [TestCase("SESAENTestS01E03.HDTV.avi", 1, 3, "SESAENTest")]
        [TestCase("SESAENTest.S02E04.HDTV.StPaul.xxxxxxxxxxxx.avi", 2, 4, "SESAENTest")]
        [TestCase("SESAENTest.S02E04.HDTV.StPaul.720p.avi", 2, 4, "SESAENTest")]
        [TestCase("SESAENTestS02E05HDTVStPaul.mp4", 2, 5, "SESAENTest")]
        [TestCase("SESAENThe.TestS02E05HDTVStPaul.mp4", 2, 5, "SESAENThe Test")]
        [TestCase("SESAENThe.Test.S02E05HDTVStPaul.mp4", 2, 5, "SESAENThe Test")]
        [TestCase("SESAENThe_Test.S02E05HDTVStPaul.mp4", 2, 5, "SESAENThe Test")]
        [TestCase("SESAENThe_Test.s02e05HDTVStPaul.mp4", 2, 5, "SESAENThe Test")]
        [TestCase("SESAENThe Test S02E05 HDTVStPaul.mp4", 2, 5, "SESAENThe Test")]
        public void ShouldExtractSeriesAndEpisodeNumber(string fileName, int series, int episode, string seriesName)
        {
            using (var dbContext = new MediaModelDBContext())
            {
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var newEpisode = episodeFactory.CreateNewEpisode(fileName);

                Assert.That(newEpisode.Season, Is.EqualTo(series));
                Assert.That(newEpisode.EpisodeNumber, Is.EqualTo(episode));
                Assert.That(newEpisode.Series.SeriesName, Is.EqualTo(seriesName));

                dbContext.Series.Remove(newEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldReadSizeOfRealFile()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldReadSizeOfRealFile";
            HelperFileFolderTests.CreateTestFolder(testDirectory);

            HelperFileFolderTests.AddTestFileToTestFolder(testDirectory, new List<string> { "Test.ShouldReadSizeOfRealFile.S01E01.avi" }, "ShouldReadSizeOfRealFile");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodesHelper = new EpisodesHelper(dbContext);
                var episodeFiles = episodesHelper.GetEpisodeFiles(testDirectory, false);
                var episodeFile = episodeFiles[0];
                Assert.That(episodeFile.FileSize, Is.EqualTo(53));

                dbContext.Series.Remove(episodeFile.Series);
                dbContext.SaveChanges();
                HelperFileFolderTests.DisposeTestFolder(testDirectory);
            }
        }

        [Test]
        public void ShouldReadTheFullPathOfTheFile()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldReadTheFullPathOfTheFile";
            HelperFileFolderTests.CreateTestFolder(testDirectory);

            const string testFileName = "TestShouldReadTheFullPathOfTheFile.S01E01.avi";
            HelperFileFolderTests.AddTestFileToTestFolder(testDirectory, new List<string> { testFileName },
                "ShouldReadTheFullPathOfTheFile");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodesHelper = new EpisodesHelper(dbContext);
                var episodeFiles = episodesHelper.GetEpisodeFiles(testDirectory, false);
                var episodeFile = episodeFiles[0];
                var expected = string.Concat(testDirectory, "\\", testFileName);
                Assert.That(episodeFile.FullPath, Is.EqualTo(expected));

                dbContext.Series.Remove(episodeFile.Series);
                dbContext.SaveChanges();
                HelperFileFolderTests.DisposeTestFolder(testDirectory);
            }
        }

        [TestCase("Test.ShouldCreateStructurePathFromSeriesObj.S03E01.avi",
                @"C:\Test ShouldCreateStructurePathFromSeriesObj\Season 03\Test ShouldCreateStructurePathFromSeriesObj S03E01.avi")]
        [TestCase("UnitTest.ShouldCreateStructurePathFromSeriesObj.S11E01.avi",
                @"C:\UnitTest ShouldCreateStructurePathFromSeriesObj\Season 11\UnitTest ShouldCreateStructurePathFromSeriesObj S11E01.avi")]
        public void ShouldCreateStructurePathFromSeriesObj(string fileName, string path)
        {
            const string testDirectory = @"C:";
            using (var dbContext = new MediaModelDBContext())
            {
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var episodeFile = episodeFactory.CreateNewEpisode(fileName);

                Assert.That(episodeFile.CreateLibraryEpisodePath(testDirectory), Is.EqualTo(path));
                dbContext.Series.Remove(episodeFile.Series);
                dbContext.SaveChanges();
            }
        }
    }
}