using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Internal.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;

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
