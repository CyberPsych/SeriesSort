﻿using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using SeriesSort.Tests.Lib;
using System;
using System.Linq;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    class SeriesTests
    {
        //[SetUp]
        //public void Init()
        //{
        //    var a = new MediaFormat
        //    {
        //        FormatName = "Video - AVI",
        //        Extension = "avi"
        //    };
        //    var b = new MediaFormat
        //    {
        //        FormatName = "Video - MP4",
        //        Extension = "mp4"
        //    };
        //    var c = new MediaFormat
        //    {
        //        FormatName = "Video - MPeg",
        //        Extension = "mpg"
        //    };
        //    using (var db = new MediaModelDBContext())
        //    {
        //        var formats = new List<MediaFormat> { a, b, c };
        //        foreach (var format in formats)
        //        {
        //            if (db.MediaFormats.Count(x => x.Extension == format.Extension) == 0)
        //            {
        //                db.MediaFormats.Add(format);
        //            }
        //        }
        //    }
        //}

        //[TearDown]
        //public void cleanUp()
        //{
        //    using (var db = new MediaModelDBContext())
        //    {
        //        var testSeries = db.Series.Where(s => s.SeriesPath.StartsWith(TestContstants.TestDir));
        //        db.Series.RemoveRange(testSeries);
        //    }
        //    Directory.Delete(TestContstants.TestDir, true);
        //}

        [Test]
        public void ShouldExtractSeriesInfo()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                var episodeFactory = new EpisodeFactory(dbContext);
                var testEpisode = episodeFactory.CreateNewEpisode("UnitTestShouldExtractSeriesInfoS01E01");
                Assert.That(testEpisode.Series, Is.Not.Null);
                Assert.That(testEpisode.Series.SeriesName, Is.EqualTo("UnitTestShouldExtractSeriesInfo"));

                dbContext.Series.Remove(testEpisode.Series);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldGetEpisodeIdBackWhenSaveInDb()
        {
            using (var dbContext = new MediaModelDBContext())
            {

                var episodeFactory = new EpisodeFactory(dbContext);
                var testEpisode = episodeFactory.CreateNewEpisode("T:\\AAA\\UnitTestShouldGetEpisodeIdBackWhenSaveInDbS01E01.avi");
                testEpisode.CreateDateTime = DateTime.Now;
                testEpisode.FileSize = 100;

                dbContext.Episodes.Add(testEpisode);
                dbContext.SaveChanges();

                Assert.That(testEpisode.EpisodeId, Is.Not.EqualTo(0), "EpisodeId not returned on save");

                var series = testEpisode.Series;
                dbContext.Episodes.Remove(testEpisode);
                dbContext.Series.Remove(series);
                dbContext.SaveChanges();
            }
        }



        [Test]
        public void ShouldStoreSeriesInDb()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                const string testDirectory = TestContstants.TestDir + @"\ShouldStoreSeriesInDb";
                var episodeFactory = new EpisodeFactory(dbContext);
                var testEpisode = episodeFactory.CreateNewEpisode(testDirectory + @"\UnitTestShouldStoreSeriesInDbS01E01.avi");
                testEpisode.CreateDateTime = DateTime.Now;
                testEpisode.FileSize = 100;


                dbContext.Episodes.Add(testEpisode);
                dbContext.SaveChanges();

                var newSeriesId = testEpisode.Series.SeriesId;
                var retrievedSeries = dbContext.Series.First(x => x.SeriesId == newSeriesId);

                Assert.That(retrievedSeries.SeriesName, Is.EqualTo(testEpisode.Series.SeriesName));
                Assert.That(retrievedSeries.Episodes, Is.Not.Null);
                Assert.That(retrievedSeries.Episodes.Count, Is.GreaterThan(0));
                Assert.That(retrievedSeries.Episodes.First().Season, Is.EqualTo(testEpisode.Season));
                Assert.That(retrievedSeries.Episodes.First().EpisodeNumber, Is.EqualTo(testEpisode.EpisodeNumber));

                var newEpisodeId = testEpisode.EpisodeId;
                var retrievedEpisode = dbContext.Episodes.FirstOrDefault(x => x.EpisodeId == newEpisodeId);

                Assert.That(retrievedEpisode, Is.Not.Null, "retrievedEpisode is null");
                Assert.That(retrievedEpisode.Series.SeriesName, Is.EqualTo(testEpisode.Series.SeriesName));
                Assert.That(retrievedEpisode.Season, Is.EqualTo(testEpisode.Season));
                Assert.That(retrievedEpisode.EpisodeNumber, Is.EqualTo(testEpisode.EpisodeNumber));

                dbContext.Episodes.Remove(retrievedEpisode);
                dbContext.SaveChanges();
                var empltyEpisodes = dbContext.Episodes.Where(x => x.EpisodeId == newEpisodeId);

                Assert.That(empltyEpisodes.Count(), Is.EqualTo(0));

                dbContext.Series.Remove(retrievedSeries);
                dbContext.SaveChanges();
            }
        }

        [Test]
        public void ShouldLoadSeriesIfOneExists()
        {
            using (var dbContext = new MediaModelDBContext())
            {
                const string testDirectory = TestContstants.TestDir + @"\ShouldLoadSeriesIfOneExists";
                var episodeFactory = new EpisodeFactory(dbContext);
                var randomSeriesName = Guid.NewGuid().ToString();
                var testEpisodeA = episodeFactory.CreateNewEpisode(testDirectory + @"\" + randomSeriesName + @"S01E01.avi");
                var testEpisodeB = episodeFactory.CreateNewEpisode(testDirectory + @"\" + randomSeriesName + @"S02E03.avi");


                dbContext.Episodes.Add(testEpisodeA);
                dbContext.Episodes.Add(testEpisodeB);
                dbContext.SaveChanges();

                Assert.That(dbContext.Series.Count(s => s.SeriesName == randomSeriesName), Is.EqualTo(1));

                var testSeries = testEpisodeA.Series;
                dbContext.Episodes.Remove(testEpisodeA);
                dbContext.Episodes.Remove(testEpisodeB);
                dbContext.Series.Remove(testSeries);
                dbContext.SaveChanges();
            }
        }
    }
}
