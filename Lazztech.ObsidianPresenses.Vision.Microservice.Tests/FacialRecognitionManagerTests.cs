using System;
using System.Collections.Generic;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using Xunit;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Tests
{
    public class FacialRecognitionManagerTests
    {
        public static string[] knownDirs = {"", ""};
        public static string[] unknownDirs = {"", ""};
        public static string[] knownUnknownDirs = {"", ""};

        public static string face_recognitionLinesTestData = @"/face/unknown/0.jpeg,Gian Lazzarini
/face/unknown/webcam.jpg,no_persons_found
/face/unknown/images.jpeg,Gian Lazzarini
/face/unknown/2892.png,Scott Hanselman
/face/unknown/Chad Peterson.jpg,unknown_person
/face/unknown/unnamed.jpg,Gian Lazzarini";

        public static string face_coordinatesLinesTestData = @"/face/unknown/0.jpeg,29,133,101,61
/face/unknown/webcam.jpg,185,400,400,185
/face/unknown/images.jpeg,54,181,158,77
/face/unknown/2892.png,136,394,394,136
/face/unknown/Chad Peterson.jpg,113,328,328,113
/face/unknown/unnamed.jpg,156,610,527,238";

        [Fact]
        public void Test1_BasicSmokeTest()
        {
            //Arrange
            var rec = new FacialRecognitionManager(new FacialIdendtityHandlerMock(), new ImageDirectoriesFinderMock());

            //Act
            rec.Process();
            var results = rec.Results;

            //Assert
            Assert.NotNull(results);
        }
    }

    class ImageDirectoriesFinderMock : IImageDirectoriesFinder
    {
        public string KnownImageDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UnknownImageDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string KnownUnknownImagDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NoPersonsFoundDir { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string[] GetAllKnownImageDirs()
        {
            return FacialRecognitionManagerTests.knownDirs;
        }

        public string[] GetAllKnownUnknownImageDirs()
        {
            return FacialRecognitionManagerTests.knownUnknownDirs;
        }

        public string[] GetAllNoPersonsFoundImageDirs()
        {
            throw new NotImplementedException();
        }

        public string[] GetAllUnknownImageDirs()
        {
            return FacialRecognitionManagerTests.unknownDirs;
        }
    }

    class FacialIdendtityHandlerMock : IFacialIdentityHandler
    {
        public List<string> FaceRecognition()
        {
            string[] lines = FacialRecognitionManagerTests.face_recognitionLinesTestData.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);
            return new List<string>(lines);
        }
    }
}
