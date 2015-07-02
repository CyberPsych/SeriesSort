using NUnit.Framework;
using System;
using SeriesSort.Model;
using SeriesSort.Model.Helpers;

namespace SeriesSort.Tests.Tests
{
    [TestFixture]
    public class SystemTests
    {
        [Test, Ignore]
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
