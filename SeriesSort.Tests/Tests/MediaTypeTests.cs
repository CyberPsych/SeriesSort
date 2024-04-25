using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    class MediaTypeTests
    {
        [Test]
        public void ShouldReturnAListOfValidExtensions()
        {
            var mediaModelDbContext = new MediaModelDBContext();

            var mediaTypeHelper = new MediaTypeHelper(mediaModelDbContext);
            List<string> list = mediaTypeHelper.GetValidExtensions();
            Assert.That(list.Aggregate((a, x) => a + ", " + x), Is.EqualTo("avi, mpeg, mkv, mp4"));
        }
    }
}
