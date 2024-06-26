﻿using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Model.Interface;
using SeriesSort.Tests.Lib;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public void ShouldListEpisodeInSubDirectory()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldListEpisodeInSubDirectoryIfSet";
            const string subdirectory = testDirectory + @"\SubDirectory";
            HelperFileFolderTests.CreateTestFolder(testDirectory);
            HelperFileFolderTests.CreateTestFolder(subdirectory);

            HelperFileFolderTests.AddTestFileToTestFolder(subdirectory, new List<string> { "ShouldListEpisodeInSubDirectoryIfSet.S01E01.mp4" }, "ShouldListEpisodeInSubDirectoryIfSet");

            using (var dbContext = new MediaModelDBContext())
            {
                var episodeHelper = new EpisodesHelper(dbContext);
                var episodeFiles = episodeHelper.GetEpisodeFiles(testDirectory, true);
                var fileCount = episodeFiles.Count();
                Assert.That(fileCount, Is.EqualTo(1), "Did not find episode in subdirectory");

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
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var testEpisode = episodeFactory.CreateNewEpisode(startTestDirectory + @"\The UnitTestS01E01.avi");

                var episodeHelper = new EpisodeFileLibraryMover(testEpisode);
                episodeHelper.MoveToLibrary(libraryTestDirectory);

                const string expectedPathToMovedFile =
                    libraryTestDirectory + @"\The UnitTest\Season 01\The UnitTest S01E01.avi";
                Assert.That(testEpisode.FullPath, Is.EqualTo(expectedPathToMovedFile));
                Assert.That(File.Exists(expectedPathToMovedFile), Is.True);

                HelperFileFolderTests.DisposeTestFolder(testDirectory);

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldOverWriteFileIfExistsWhenMoveEpisodeToLibrary()
        {
            const string testDirectory = TestContstants.TestDir + @"\ShouldMoveEpisodeToLibrary";
            const string startTestDirectory = testDirectory + @"\Start\";
            HelperFileFolderTests.CreateTestFolder(startTestDirectory);
            HelperFileFolderTests.AddTestFileToTestFolder(startTestDirectory, new List<string> { "The UnitTestS01E01.avi" }, "ShouldMoveEpisodeToLibrary");


            const string libraryTestDirectory = testDirectory + @"\Library";
            const string expectedDirectory = libraryTestDirectory + @"\The UnitTest\Season 01";
            HelperFileFolderTests.CreateTestFolder(Path.GetDirectoryName(expectedDirectory));
            HelperFileFolderTests.AddTestFileToTestFolder(libraryTestDirectory, new List<string> { "The UnitTestS01E01.avi" }, "ShouldMoveEpisodeToLibrary");


            using (var dbContext = new MediaModelDBContext())
            {
                ISeriesQueryByShowName seriesQueryByNameFromDbContext = new SeriesQueryByNameFromDbContext(dbContext);
                var episodeFactory = new EpisodeFileFactory(
                    new EpisodeSeriesInformationExtractor(
                        seriesQueryByNameFromDbContext, new EpisodeFileNameCleaner(), new EpisodeFileInformationRetriever())); var testEpisode = episodeFactory.CreateNewEpisode(startTestDirectory + @"\The UnitTestS01E01.avi");

                var episodeHelper = new EpisodeFileLibraryMover(testEpisode);
                episodeHelper.MoveToLibrary(libraryTestDirectory);

                const string expectedFullPathToMovedFile = expectedDirectory + @"\The UnitTest S01E01.avi";
                Assert.That(testEpisode.FullPath, Is.EqualTo(expectedFullPathToMovedFile));
                Assert.That(File.Exists(expectedFullPathToMovedFile), Is.True);

                HelperFileFolderTests.DisposeTestFolder(testDirectory);

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }



    }
}
