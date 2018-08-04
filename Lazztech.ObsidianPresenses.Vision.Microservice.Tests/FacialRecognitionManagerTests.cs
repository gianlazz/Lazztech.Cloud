using System;
using System.Collections.Generic;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using Xunit;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Tests
{
    public class FacialRecognitionManagerTests
    {
        public static string[] knownDirs = {
            "/face/known/Gian Lazzarini.jpg",
            "/face/known/Scott Hanselman.jpeg"};
        public static string[] unknownDirs = {
            "/face/unknown/webcam.jpg",
            "/face/unknown/images.jpeg",
            "/face/unknown/2892.png",
            "/face/unknown/Chad Peterson.jpg",
            "/face/unknown/unnamed.jpg"};
        public static string[] knownUnknownDirs = {};

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
            var rec = new FacialRecognitionManager(new FacialIdendtityHandlerMock(), new FaceDetectionProcess(), new FileServicesMock());

            //Act
            rec.Process();
            var results = rec.Results;

            //Assert
            Assert.NotNull(results);
        }
    }

    class FileServicesMock : IFileServices
    {
        public string[] GetAllImageDirs(string path)
        {
            return new string[] {};
        }

        public DateTime GetCreationDateTime(string filePath)
        {
            return DateTime.Now;
        }

        public string GetFileNameFromDir(string dir)
        {
            return null;
        }
    }

    class FacialIdendtityHandlerMock : Iface_recognition
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
