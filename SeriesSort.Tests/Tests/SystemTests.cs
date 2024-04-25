using NUnit.Framework;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;
using System;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    public class SystemTests
    {
        [Test]
        public void ShouldHandleExceptions()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldReturnValidExtensions()
        {
            var mediaTypeHelper = new MediaTypeHelper(new MediaModelDBContext());
            var result = mediaTypeHelper.GetValidExtensions();
            Assert.That(result.Count, Is.GreaterThan(0));
        }

    }
}
