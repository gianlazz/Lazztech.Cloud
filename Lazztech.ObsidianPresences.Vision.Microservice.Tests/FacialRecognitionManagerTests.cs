using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Tests
{
    public class FacialRecognitionManagerTests
    {
        #region test data

        public static string knownPath = @"/face/known/";
        public static string unknownPath = @"/face/unknown/";
        public static string noPersonsFoundPath = @"/face/no_persons_found/";

        public static string knownDirs = @"/face/known/Gian Lazzarini.jpeg
/face/known/Scott Hanselman.png";

        public static string unknownDirs = @"/face/unknown/0.jpeg
/face/unknown/webcam.jpg
/face/unknown/images.jpeg
/face/unknown/2892.png
/face/unknown/Chad Peterson.jpg
/face/unknown/unnamed.jpg";

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

        public static string multipersonface_recognitionLinesTestData = @"/face/unknown/0.jpeg,Gian Lazzarini
/face/unknown/webcam.jpg,no_persons_found
/face/unknown/images.jpeg,Gian Lazzarini
/face/unknown/2892.png,Scott Hanselman
/face/unknown/Chad Peterson.jpg,unknown_person
/face/unknown/unnamed.jpg,Gian Lazzarini
/face/unknown/harry-meghan-15.jpg,Meghan Markle
/face/unknown/harry-meghan-15.jpg,Prince Harry";

        public static string multipersonface_detectionLinesTestData = @"/face/unknown/0.jpeg,29,133,101,61
/face/unknown/webcam.jpg,185,400,400,185
/face/unknown/images.jpeg,54,181,158,77
/face/unknown/2892.png,136,394,394,136
/face/unknown/Chad Peterson.jpg,113,328,328,113
/face/unknown/unnamed.jpg,156,610,527,238
/face/unknown/harry-meghan-15.jpg,294,792,443,642
/face/unknown/harry-meghan-15.jpg,154,652,333,473";

        #endregion test data

        #region test cases

        [Fact]
        public void Test1_BasicSmokeTest()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData),
                new FaceDetectionProcessMock(face_detectionLinesTestData),
                new FileServicesMock(knownDirs, unknownDirs));

            //Act
            var results = recognition.Process();

            //Assert
            Assert.NotNull(results);
        }

        [Fact]
        public void SnapshotWithStatusOfno_persons_found_PersonShouldStillHaveBB()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData),
                new FaceDetectionProcessMock(face_detectionLinesTestData),
                new FileServicesMock(knownDirs, unknownDirs));

            var arrangedBb = new FaceBoundingBox()
            {
                LeftTopCoordinate = new PixelCoordinateVertex()
                {
                    x = 0,
                    y = 0
                },
                RightBottomCoordinate = new PixelCoordinateVertex()
                {
                    x = 0,
                    y = 0
                }
            };

            //Act
            var results = recognition.Process();
            var unknownPersons = results.Where(x => x.Status == Snapshot.SnapshotStatus.unknown_person).ToList();
            var unknownPerson = unknownPersons.Where(y => y.People.Count > 0).First().People.Where(x => x.Name == "unknown_person").First();
            var bb = unknownPerson.FaceBoundingBox;

            //Assert
            Assert.True(unknownPerson != null);
            Assert.NotEqual(arrangedBb, bb);
            // Assert.True()
        }

        [Fact]
        public void SnapshotGuidIdShouldNotBeEmpty()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(face_recognitionLinesTestData),
                new FaceDetectionProcessMock(face_detectionLinesTestData),
                new FileServicesMock(knownDirs, unknownDirs));

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
                new FileServicesMock(knownDirs, unknownDirs));

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
                new FileServicesMock(knownDirs, unknownDirs));

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
                new FaceRecognitionProcessMock(multipersonface_recognitionLinesTestData),
                new FaceDetectionProcessMock(multipersonface_detectionLinesTestData),
                new FileServicesMock(multipersonKnownDirs, multiplepersonunknownDirs));

            //Act
            var results = recognition.Process();
            var resultsFromDualPersonImage = results.Where(x => x.ImageName == "harry-meghan-15.jpg").ToList();

            //Assert
            Assert.Equal(1, resultsFromDualPersonImage.Count());
            Assert.Equal(2, resultsFromDualPersonImage.First().People.Count);
        }

        [Fact]
        public void SnapshotWithTwoKnownPeopleShouldHaveCorrectlyAssignedBBForBoth()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(multipersonface_recognitionLinesTestData),
                new FaceDetectionProcessMock(multipersonface_detectionLinesTestData),
                new FileServicesMock(multipersonKnownDirs, multiplepersonunknownDirs));

            //294,792,443,642
            var meganArrangedBoundingBox = new FaceBoundingBox()
            {
                LeftTopCoordinate = new PixelCoordinateVertex() { x = 294, y = 792 },
                RightBottomCoordinate = new PixelCoordinateVertex() { x = 443, y = 642 }
            };
            //154,652,333,473
            var harryArrangedBoundingBox = new FaceBoundingBox()
            {
                LeftTopCoordinate = new PixelCoordinateVertex() { x = 154, y = 652 },
                RightBottomCoordinate = new PixelCoordinateVertex() { x = 333, y = 473 }
            };

            //Act
            var results = recognition.Process();
            var resultsFromDualPersonImage = results.Where(x => x.ImageName == "harry-meghan-15.jpg").ToList();
            var megan = resultsFromDualPersonImage.First().People.Where(x => x.Name == "Meghan Markle").FirstOrDefault();
            var harry = resultsFromDualPersonImage.First().People.Where(x => x.Name == "Prince Harry").FirstOrDefault();

            //Assert
            Assert.Equal(meganArrangedBoundingBox, megan.FaceBoundingBox);
            Assert.Equal(harryArrangedBoundingBox, harry.FaceBoundingBox);
        }

        [Fact]
        public void _3PersonSnap1UnkownShouldStillHavePeopleForeachWithBB()
        {
            //Arrange
            var recogLines = @"/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,unknown_person
/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,Meghan Markle
/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,Prince Harry";
            var detectLines = @"/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,78,559,228,410
/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,54,413,234,234
/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg,95,228,244,78";
            var knownDirs = @"/face/known/Prince Harry.jpg
/face/known/Meghan Markle.jpeg";
            var unknownDirs = @"/face/unknown/Prince-Harry_Thomas-Markle_Meghan-Markle.jpg";

            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(recogLines),
                new FaceDetectionProcessMock(detectLines),
                new FileServicesMock(knownDirs, unknownDirs));

            //Act
            var snapshots = recognition.Process();
            var people = snapshots.Where(x => x.ImageName == "Prince-Harry_Thomas-Markle_Meghan-Markle.jpg").First().People;

            //Assert
            //Assert.Equal(1, snapshots.Count());
            Assert.True(people.Where(x => x.Name == "Prince Harry").ToList().Any());
            Assert.True(people.Where(x => x.Name == "Meghan Markle").ToList().Any());
            Assert.Equal(3, people.Count());
        }

        [Fact]
        public void KnownSnapshotsShouldHavePersonToo()
        {
            //Arrange
            var recognition = new FacialRecognitionManager(
                new FaceRecognitionProcessMock(string.Empty),
                //PUT THE FACE DETECTION LINES HERE FROM THE KNOWN
                new FaceDetectionProcessMock(string.Empty),
                new FileServicesMock(knownDirs, unknownDirs));

            //Act
            var snapshots = recognition.Process();
            var giansSnapshot = snapshots.Where(x => x.ImageName == "Gian Lazzarini.jpeg").FirstOrDefault();
            var scottsSnapshot = snapshots.Where(x => x.ImageName == "Scott Hanselman.png").FirstOrDefault();

            //Assert
            Assert.True(giansSnapshot.People.Count == 1);
            Assert.True(scottsSnapshot.People.Count == 1);
        }

        //[Fact]
        //public void KnownSnapshotsPeopleShouldHaveProperlySetBoundingBox()
        //{
        //    //Arrange
        //    var recognition = new FacialRecognitionManager(
        //        new FaceRecognitionProcessMock(string.Empty),
        //        //PUT THE FACE DETECTION LINES HERE FROM THE KNOWN
        //        new FaceDetectionProcessMock(string.Empty),
        //        new FileServicesMock(knownDirs, unknownDirs));

        //    var arrangedEmptyBoundingBox = new FaceBoundingBox()
        //    {
        //        LeftTopCoordinate = new PixelCoordinateVertex() { x = 0, y = 0 },
        //        RightBottomCoordinate = new PixelCoordinateVertex() { x = 0, y = 0 }
        //    };

        //    //Act
        //    var snapshots = recognition.Process();
        //    var giansSnapshot = snapshots.Where(x => x.ImageName == "Gian Lazzarini.jpeg").FirstOrDefault();
        //    var scottsSnapshot = snapshots.Where(x => x.ImageName == "Scott Hanselman.png").FirstOrDefault();

        //    //Assert
        //    Assert.False(giansSnapshot.People.FirstOrDefault().FaceBoundingBox.Equals(arrangedEmptyBoundingBox));
        //    Assert.False(scottsSnapshot.People.FirstOrDefault().FaceBoundingBox.Equals(arrangedEmptyBoundingBox));
        //}

        #endregion test cases
    }

    #region interface mocks

    internal class FaceRecognitionProcessMock : Iface_recognition
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
                StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }

    internal class FaceDetectionProcessMock : Iface_detection
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
                StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }

    internal class FileServicesMock : IFileServices
    {
        private string known;
        private string unknown;

        public FileServicesMock(string knownPaths, string unknownPaths)
        {
            known = knownPaths;
            unknown = unknownPaths;
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
                StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd('\r', '\n');
            }

            return new List<string>(lines);
        }
    }

    #endregion interface mocks
}