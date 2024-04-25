using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Model.Interface;
using SeriesSort.Model.Model;
using System;
using System.Linq;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    class EpisodeTests
    {
        [Test]
        public void ShouldReturnANewEpisode()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                const string fileName = "UnitTestShouldReturnANewEpisodeS01E01.avi";
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var newEpisode = episodeFactory.CreateNewEpisode(fileName);
                Assert.That(newEpisode, Is.Not.Null);
                dbContext.Series.Remove(newEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldReturnANewEpisodeWithASeries()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                const string fileName = "UnitTestShouldReturnANewEpisodeWithASeriesS01E01.avi";
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); EpisodeFile newEpisodeFile = episodeFactory.CreateNewEpisode(fileName);
                Assert.That(newEpisodeFile, Is.Not.Null);
                Assert.That(newEpisodeFile.Series, Is.Not.Null);
                dbContext.Series.Remove(newEpisodeFile.Series);
                dbContext.SaveChanges();
            }
        }


        [Test]
        public void ShouldGetEpisodeIdBackWhenSaveInDb()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var testEpisode = episodeFactory.CreateNewEpisode("T:\\AAA\\UnitTestShouldGetEpisodeIdBackWhenSaveInDbS01E01.avi");
                testEpisode.CreateDateTime = DateTime.Now;
                testEpisode.FileSize = 100;

                dbContext.EpisodeFiles.Add(testEpisode);
                dbContext.SaveChanges();

                Assert.That(testEpisode.EpisodeId, Is.Not.EqualTo(0), "EpisodeId not returned on save");

                var series = testEpisode.Series;
                dbContext.EpisodeFiles.Remove(testEpisode);
                dbContext.Series.Remove(series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldExtractTheFileExtensionForEpisode()
        {
            using (var dbContext = new MediaModelDBContext())
            {

                const string fileName = "UnitTestShouldReturnANewEpisodeS01E01.avi";
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); EpisodeFile newEpisodeFile = episodeFactory.CreateNewEpisode(fileName);
                var actual = newEpisodeFile.FileExtention;
                Assert.That(actual, Is.EqualTo(@"avi"));
            }
        }

        [Test]
        public void ShouldReturnTheEpisodePath()
        {
            using (var dbContext = new MediaModelDBContext())
            {

                const string testDirectory = @"C:";
                const string fullPath = testDirectory + @"\" + @"\UnitTest.ShouldReturnTheEpisodePath.S01E01.avi";
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var newEpisode = episodeFactory.CreateNewEpisode(fullPath);
                var actual = newEpisode.CreateLibraryEpisodePath(testDirectory);
                Assert.That(actual, Is.EqualTo(testDirectory + @"\UnitTest ShouldReturnTheEpisodePath\Season 01\UnitTest ShouldReturnTheEpisodePath S01E01.avi"));

                dbContext.Series.Remove(newEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [TestCase("The.ShouldReturnValidEpisode.S01E01.avi", true, Description = "Valid episode name")]
        [TestCase("RedHerring.avi", false, Description = "Red hearring, not a valid name")]
        [TestCase("The.ShouldReturnValidEpisode.S01E01.nfo", false, Description = "Not a valid extention")]
        public void ShouldReturnValidEpisode(string fileName, bool isValidEpisodeExpected)
        {
            using (var dbContext = new MediaModelDBContext())
            {

                const string testDirectory = @"C:";
                var fullPath = testDirectory + @"\" + fileName;
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var newEpisode = episodeFactory.CreateNewEpisode(fullPath);
                var actual = newEpisode.IsValidEpisode();
                Assert.That(actual, Is.EqualTo(isValidEpisodeExpected));

                if (newEpisode.IsValidEpisode())
                {
                    dbContext.Series.Remove(newEpisode.Series);
                    dbContext.SaveChanges();
                }
            }
        }

        [TestCase("Test.S01E01.avi", true, true, Description = "No Sample, excludeSampleSetting true")]
        [TestCase("Test.S01E01.avi", false, true, Description = "No Sample, excludeSampleSetting false")]
        [TestCase("TestSampleTest.S01E01.avi", true, true, Description = "No Sample, excludeSampleSetting true has Sample in Name")]
        [TestCase("Test.Sample.S01E01.avi", true, false, Description = "Sample, excludeSampleSetting true")]
        [TestCase("Test.Sample.S01E01.avi", false, true, Description = "Sample, excludeSampleSetting false")]
        public void ShouldNotBeAValidEpisodeIfSample_IfNoSampleSettingSet(string fileName, bool excludeSampleSetting, bool isValidEpisodeExpected)
        {
            using (var dbContext = new MediaModelDBContext())
            {
                Settings.ExcludeSampleFiles = excludeSampleSetting;
                const string testDirectory = @"C:";
                var fullPath = testDirectory + @"\" + fileName;
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var newEpisode = episodeFactory.CreateNewEpisode(fullPath);
                var actual = newEpisode.IsValidEpisode();
                Assert.That(actual, Is.EqualTo(isValidEpisodeExpected));

                if (newEpisode.IsValidEpisode())
                {
                    dbContext.Series.Remove(newEpisode.Series);
                    dbContext.SaveChanges();
                }
            }
        }

        [Test]
        public void ShouldStoreEpisodeInDb()
        {
            int episodeId;
            using (var dbContext = new MediaModelDBContext())
            {
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var testEpisode = episodeFactory.CreateNewEpisode("T:\\AAA\\UnitTestShouldStoreEpisodeInDbS01E01.avi");
                testEpisode.CreateDateTime = DateTime.Now;
                testEpisode.FileSize = 100;


                dbContext.EpisodeFiles.Add(testEpisode);
                dbContext.SaveChanges();
                episodeId = testEpisode.EpisodeId;
            }

            using (var dbContextExpected = new MediaModelDBContext())
            {
                var retrievedEpisode = dbContextExpected.EpisodeFiles.First(episodeFile => episodeFile.EpisodeId == episodeId);

                Assert.That(retrievedEpisode, Is.Not.Null);

                var series = retrievedEpisode.Series;
                dbContextExpected.EpisodeFiles.Remove(retrievedEpisode);
                dbContextExpected.Series.Remove(series);
                dbContextExpected.SaveChanges();
            }
        }

        [Test]
        public void ShouldGetEpisodePathForDuplicate()
        {
            throw new NotImplementedException();

            //var e = new EpisodeFile();
            //e.GetEpisodePathForDuplicate("todo", 1);

            //Assert.That("Not Implemented", Is.True);
        }


        [Test]
        public void ShouldGetNextEpisodePathForDuplicate()
        {
            throw new NotImplementedException();

            //var e = new EpisodeFile();
            //e.GetNextEpisodePathForDuplicate("todo");

            //Assert.That("Not Implemented", Is.True);
        }
    }
}
