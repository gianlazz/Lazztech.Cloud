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

        public static string knownDirs = @"/face/known/Gian Lazzarini.jpeg
/face/known/Scott Hanselman.png";
        public static string unknownDirs = @"/face/unknown/0.jpeg
/face/unknown/webcam.jpg
/face/unknown/images.jpeg
/face/unknown/2892.png
/face/unknown/Chad Peterson.jpg
/face/unknown/unnamed.jpg";
        public static string knownUnknownDirs = @"";

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

        public static string multipersonKnownDirs = @"/face/known/Prince Harry.jpg
/face/known/Gian Lazzarini.jpeg
/face/known/Meghan Markle.jpeg
/face/known/Scott Hanselman.png";

        public static string multiplepersonunknownDirs = @"/face/unknown/webcam.jpg
/face/unknown/Chad Peterson.jpg
/face/unknown/unnamed.jpg
/face/unknown/harry-meghan-15.jpg
/face/unknown/0.jpeg
/face/unknown/images.jpeg
/face/unknown/2892.png";

        #endregion

        #region test cases
        [Fact]
        public void Test1_BasicSmokeTest()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(knownDirs, unknownDirs, knownUnknownDirs));

            //Act
            var results = recognition.Process();

            //Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void SnapshotWithStatusOfno_persons_found_ShouldHaveNoPeople() 
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(knownDirs, unknownDirs, knownUnknownDirs));

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
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(knownDirs, unknownDirs, knownUnknownDirs));

            //Act
            var results = recognition.Process();

            //Assert
            Assert.False(results.Where(snapshot => snapshot.GuidId == Guid.Empty).Any());
        }

        [Fact]
        public void PersonNameShouldNotHaveReturnCarriage()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(knownDirs, unknownDirs, knownUnknownDirs));

            //Act
            var results = recognition.Process();
            var people = new List<Person>();
            foreach (var result in results)
            {
                people.AddRange(result.People);
            }
            var a = people.Where(x => x.Name.Contains("\r")).ToList();
            var b = people.Where(x => x.Name.Contains("\n")).ToList();

            //Assert
            Assert.False(
                people.Where(x => x.Name.Contains("\r")).ToList().Any()
                ||
                people.Where(x => x.Name.Contains("\n")).ToList().Any());
        }

        [Fact]
        public void SnapshotsWithPeopleFoundShouldHaveValidBoundingBoxForThem()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(knownDirs, unknownDirs, knownUnknownDirs));         

            //54,181,158,77
            var arrangedBoundingBox = new FaceBoundingBox() 
            {
                LeftTopCoordinate = new PixelCoordinateVertex() { x = 54, y = 181 },
                RightBottomCoordinate = new PixelCoordinateVertex() { x = 158, y = 77 }
            };

            //Act
            var results = recognition.Process();
            var snapshot = results.Where(snap => snap.ImageDir == "/face/unknown/images.jpeg").FirstOrDefault();
            var gian = snapshot.People.Where(p => p.Name == "Gian Lazzarini").FirstOrDefault();
            var giansFaceBoundingBox = gian.FaceBoundingBox;

            //Assert
            Assert.Equal(gian.FaceBoundingBox, arrangedBoundingBox);
        }

        [Fact]
        public void SnapshotWithTwoKnownPeople()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData), 
                new FaceDetectionProcessMock(face_detectionLinesTestData), 
                new FileServicesMock(multipersonKnownDirs, multiplepersonunknownDirs, knownUnknownDirs));
            
            //Act
            var results = recognition.Process();
            var resultsFromDualPersonImage = results.Where(x => x.ImageName == "harry-meghan-15.jpg").ToList();

            //Assert
            Assert.Equal(1, resultsFromDualPersonImage.Count());
            Assert.Equal(2, resultsFromDualPersonImage.First().People.Count);
        }
        #endregion
    }

    #region interface mocks
    class FaceRecognitionProcessMock : Iface_recognition
    {
        private List<string> output;

        public FaceRecognitionProcessMock(string lines)
        {
            output = SplitStdoutLines(lines);
        }
        public List<string> FaceRecognition()
        {
            return output;
        }

        private List<string> SplitStdoutLines(string stdout)
        {
            var lines = stdout.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }

    class FaceDetectionProcessMock : Iface_detection
    {
        private List<string> output;

        public FaceDetectionProcessMock(string lines)
        {
            output = SplitStdoutLines(lines);
        }

        public List<string> FaceDetection()
        {
            return output;
        }

        private List<string> SplitStdoutLines(string stdout)
        {
            var lines = stdout.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }

    class FileServicesMock : IFileServices
    {
        private string known;
        private string unknown;
        private string knownUnknown;

        public FileServicesMock(string knownPaths, string unknownPaths, string knownUnknownPaths)
        {
            known = knownPaths;
            unknown = unknownPaths;
            knownUnknown = knownUnknownPaths;
        }

        public string[] GetAllImageDirs(string path)
        {
            if (path.Contains("/known/"))
            {
                return SplitStdoutLines(known).ToArray();
                // return FacialRecognitionManagerTests.knownDirs;
            } 
            else if (path.Contains("/unknown/"))
            {
                return SplitStdoutLines(unknown).ToArray();
                // return FacialRecognitionManagerTests.unknownDirs;
            }
            else if (path.Contains("/known_unknown/"))
            {
                //throw new Exception("No valid mock prepared yet.");
                return SplitStdoutLines(knownUnknown).ToArray();
                // return new string[] {};
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

        private List<string> SplitStdoutLines(string stdout)
        {
            var lines = stdout.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }
    #endregion
}
