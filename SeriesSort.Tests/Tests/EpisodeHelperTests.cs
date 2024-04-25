using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Model.Interface;
using SeriesSort.Tests.Lib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    public class EpisodeHelperTests
    {
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
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever()));
                var testEpisode = episodeFactory.CreateNewEpisode(startTestDirectory + @"\The UnitTestS01E01.avi");
                var creationPath = testEpisode.FullPath;
                dbContext.EpisodeFiles.Add(testEpisode);
                dbContext.SaveChanges();
                Assert.That(creationPath, Is.EqualTo(testEpisode.FullPath), "The path should be the path to where it was ceated.");

                var episodeHelper = new EpisodeFileLibraryMover(testEpisode);
                episodeHelper.MoveToLibrary(libraryTestDirectory);

                dbContext.SaveChanges();

                Assert.That(creationPath, Is.Not.EqualTo(testEpisode.FullPath), "The path should have changed with the move");

                var retrievedEpisode = dbContext.EpisodeFiles.FirstOrDefault(episode => episode.EpisodeId == testEpisode.EpisodeId);
                Assert.That(retrievedEpisode, Is.Not.Null);
                string expextedPathToMovedFile = retrievedEpisode.FullPath;
                Assert.That(testEpisode.FullPath, Is.EqualTo(expextedPathToMovedFile));
                Assert.That(File.Exists(expextedPathToMovedFile), Is.True);

                HelperFileFolderTests.DisposeTestFolder(testDirectory);

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }
    }
}
