using System;
using System.Collections.Generic;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using Xunit;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Tests
{
    public class FacialRecognitionManagerTests
    {
        #region test data
        public static string knownPath = @"/face/known/";
        public static string unknownPath = @"/face/unknown/";
        public static string knownUnkownPath = @"/face/known_unknown/";
        public static string noPersonsFoundPath = @"/face/no_persons_found/";

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

        public static string face_detectionLinesTestData = @"/face/unknown/0.jpeg,29,133,101,61
/face/unknown/webcam.jpg,185,400,400,185
/face/unknown/images.jpeg,54,181,158,77
/face/unknown/2892.png,136,394,394,136
/face/unknown/Chad Peterson.jpg,113,328,328,113
/face/unknown/unnamed.jpg,156,610,527,238";
        #endregion

        [Fact]
        public void Test1_BasicSmokeTest()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(new FaceRecognitionProcessMock(), new FaceDetectionProcessMock(), new FileServicesMock());

            //Act
            var results = recognition.Process();

            //Assert
            Assert.NotNull(results);
        }
    }

    #region interface mocks
    class FaceRecognitionProcessMock : Iface_recognition
    {
        public List<string> FaceRecognition()
        {
            string[] lines = FacialRecognitionManagerTests.face_recognitionLinesTestData.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);
            return new List<string>(lines);
        }
    }

    class FaceDetectionProcessMock : Iface_detection
    {
        public List<string> FaceDetection()
        {
            string[] lines = FacialRecognitionManagerTests.face_detectionLinesTestData.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);
            return new List<string>(lines);
        }
    }

    class FileServicesMock : IFileServices
    {
        public string[] GetAllImageDirs(string path)
        {
            if (path.Contains("/known/"))
            {
                return new string[] {};
            } 
            else if (path.Contains("/unknown/"))
            {
                return new string[] {};
            }
            else if (path.Contains("/known_unknown/"))
            {
                return new string[] {};
            }
            else if (path.Contains("/"))
            {
                return new string[] {};
            }
            else
            {
                throw new Exception("No valid path mock detected.");
                return new string[] {};
            }
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
    #endregion
}
