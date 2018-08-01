using System;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using NUnit.Framework;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Tests
{
    [TestFixture]
    public class FacialRecognitionManagerTests
    {
        private string face_recognitionLinesTestData = @"/face/unknown/0.jpeg,Gian Lazzarini
/face/unknown/webcam.jpg,no_persons_found
/face/unknown/images.jpeg,Gian Lazzarini
/face/unknown/2892.png,Scott Hanselman
/face/unknown/Chad Peterson.jpg,unknown_person
/face/unknown/unnamed.jpg,Gian Lazzarini";

        private string face_coordinatesLinesTestData = @"/face/unknown/0.jpeg,29,133,101,61
/face/unknown/webcam.jpg,185,400,400,185
/face/unknown/images.jpeg,54,181,158,77
/face/unknown/2892.png,136,394,394,136
/face/unknown/Chad Peterson.jpg,113,328,328,113
/face/unknown/unnamed.jpg,156,610,527,238";

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
