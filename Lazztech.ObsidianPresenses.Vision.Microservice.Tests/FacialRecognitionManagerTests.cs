using System;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using NUnit.Framework;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Tests
{
    [TestFixture]
    public class FacialRecognitionManagerTests
    {
        [Test]
        public void AssertNotNull()
        {
            //Arrange
            var rec = new FacialRecognitionManager();

            //Act
            rec.Process();
            var results = rec.Results;

            //Assert
            Assert.NotNull(results);
        }
    }
}
