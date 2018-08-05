using System;
using System.Collections.Generic;
using System.Linq;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models;
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
            "/face/known/Gian Lazzarini.jpeg",
            "/face/known/Scott Hanselman.png"};
        public static string[] unknownDirs = {
            "/face/unknown/0.jpeg",
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

        #region test cases
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

        [Fact]
        public void SnapshotWithStatusOfno_persons_found_ShouldHaveNoPeople() 
        {
            //Arrange
            var recognition = new FacialRecognitionManager(new FaceRecognitionProcessMock(), new FaceDetectionProcessMock(), new FileServicesMock());

            //Act
            var results = recognition.Process();

            //Assert
            Assert.False(
                results.Where(x => x.Status == Snapshot.SnapshotStatus.unknown_person)
                .Where(y => y.People.Count > 0)
                .Any()
                );
        }

        [Fact]
        public void SnapshotGuidIdShouldNotBeEmpty()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(new FaceRecognitionProcessMock(), new FaceDetectionProcessMock(), new FileServicesMock());

            //Act
            var results = recognition.Process();

            //Assert
            Assert.False(results.Where(snapshot => snapshot.GuidId == Guid.Empty).Any());
        }

        [Fact]
        public void SnapshotsWithPeopleFoundShouldHaveValidBoundingBoxForThem()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(new FaceRecognitionProcessMock(), new FaceDetectionProcessMock(), new FileServicesMock());
            //54,181,158,77
            var boundingBox = new FaceBoundingBox() 
            {
                LeftTopCoordinate = new PixelCoordinateVertex() { x = 54, y = 181 },
                RightBottomCoordinate = new PixelCoordinateVertex() { x = 158, y = 77 }
            };

            //Act
            var results = recognition.Process();
            var result = results.Where(snapshot => snapshot.ImageDir == "/face/unknown/images.jpeg").FirstOrDefault();
            var person2 = result.People.Where(p => p.Name == "Gian Lazzarini\r").FirstOrDefault();
            var person = result.People.Where(p => p.Name == "Gian Lazzarini").FirstOrDefault();
            // var personsBoundingBox = result.People.Where(p => p.Name == "Gian Lazzarini").FirstOrDefault().FaceBoundingBox;

            //Assert
            //Assert.True(personsBoundingBox == boundingBox);
            // Assert.True(
            //     results.Where(snapshot => snapshot.ImageDir == "/face/unknown/images.jpeg").FirstOrDefault()
            //     .People.Where(p => p.Name == "Gian Lazzarini").FirstOrDefault()
            //     .FaceBoundingBox == boundingBox);
        }
        #endregion
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
                return FacialRecognitionManagerTests.knownDirs;
            } 
            else if (path.Contains("/unknown/"))
            {
                return FacialRecognitionManagerTests.unknownDirs;
            }
            else if (path.Contains("/known_unknown/"))
            {
                //throw new Exception("No valid mock prepared yet.");
                return new string[] {};
            }
            else if (path.Contains("/no_persons_found/"))
            {
                throw new Exception("No valid mock prepared yet.");
            }
            else
            {
                throw new Exception("No valid path mock detected.");
            }
        }

        public DateTime GetCreationDateTime(string filePath)
        {
            //MAY NEED TO CHANGE IF I START INCORPERATING LOGIC FROM THIS
            return DateTime.Now;
        }

        public string GetFileNameFromDir(string dir)
        {
            return dir.Substring(dir.LastIndexOf('/') + 1);
        }
    }
    #endregion
}
