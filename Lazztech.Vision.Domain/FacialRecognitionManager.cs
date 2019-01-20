using Lazztech.Cloud.Vision.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Lazztech.Cloud.Vision.Domain.Models.Snapshot;

namespace Lazztech.Cloud.Vision.Domain
{
    public class FacialRecognitionManager
    {
        #region properties

        public static string knownPath = @"/face/known/";
        public static string unknownPath = @"/face/unknown/";
        public static string knownJsonsPath = @"/face/knownResults/";
        public static string unknownJsonsPath = @"/face/unknownResults/";

        public List<string> _knownImageDirs = new List<string>();
        public List<string> _unknownImageDirs = new List<string>();

        public List<Snapshot> Known = new List<Snapshot>();
        public List<Snapshot> Unknown = new List<Snapshot>();

        public List<Snapshot> Results { get; set; }
        public List<string> face_recognitionLines = new List<string>();
        public List<string> face_detectionLines = new List<string>();

        #endregion properties

        #region services fields

        private IFileServices _fileServices;
        private Iface_detection _face_detection;
        private Iface_recognition _face_recognition;

        #endregion services fields

        #region ctor

        public FacialRecognitionManager(
            Iface_recognition face_recognition,
            Iface_detection face_detection,
            IFileServices fileServices)
        {
            _face_detection = face_detection;
            _face_recognition = face_recognition;
            _fileServices = fileServices;
            Results = new List<Snapshot>();

            StandUpAllNeededDirectories();
        }

        #endregion ctor

        public List<Snapshot> Process()
        {
            CollectAllImageDirs();
            InstantiateSnapshotsFromDirs();
            //SetSnapshotsCreationDateTime();
            face_recognitionLines = _face_recognition.FaceRecognition();
            if (face_recognitionLines.Count == 0)
                throw new Exception("face_recognition returned no lines");
            face_detectionLines = _face_detection.FaceDetection();
            if (face_detectionLines.Count == 0)
                throw new Exception("face_detection returned no lines");

            Results.AddRange(Known);
            Results.AddRange(Unknown);

            HandleIdentities();
            HandleBoundingBoxes();

            return Results;
        }

        public Snapshot Process(string path)
        {
            //_knownImageDirs.Add(path);
            CollectAllImageDirs();
            InstantiateSnapshotsFromDirs();

            face_recognitionLines = _face_recognition.FaceRecognition();
            if (face_recognitionLines.Count == 0)
                throw new Exception("face_recognition returned no lines");
            face_detectionLines = _face_detection.FaceDetection();
            if (face_detectionLines.Count == 0)
                throw new Exception("face_detection returned no lines");

            Results.AddRange(Known);
            Results.AddRange(Unknown);

            HandleIdentities();
            HandleBoundingBoxes();

            var result = Results.Where(x => x.ImageDir == path).FirstOrDefault();

            if (result == null)
                throw new Exception();

            return result;
        }

        private void StandUpAllNeededDirectories()
        {
            if (!Directory.Exists(knownPath))
                Directory.CreateDirectory(knownPath);

            if (!Directory.Exists(unknownPath))
                Directory.CreateDirectory(unknownPath);

            if (!Directory.Exists(knownJsonsPath))
                Directory.CreateDirectory(knownJsonsPath);

            if (!Directory.Exists(unknownJsonsPath))
                Directory.CreateDirectory(unknownJsonsPath);
        }

        private void HandleBoundingBoxes()
        {
            var lines = face_detectionLines;

            var snaps = Results.Where(x => x.Status != SnapshotStatus.no_persons_found).ToList();
            var snapsWithPeople = snaps.Where(x => x.People.Any()).ToList();

            foreach (var line in lines)
            {
                var imageDirFromLine = line.Split(',')[0];
                var imageName = _fileServices.GetFileNameFromDir(imageDirFromLine);
                if (snapsWithPeople.Where(x => x.ImageName == imageName).ToList().Any())
                {
                    var matchingLines = lines.Where(x => x.Split(',')[0] == imageDirFromLine).ToList();
                    for (int i = 0; i < matchingLines.Count(); i++)
                    {
                        var snap = snapsWithPeople.Where(x => x.ImageName == imageName).First();
                        var bb = ExtractBoundingBox(matchingLines[i]);
                        snap.People[i].FaceBoundingBox = bb;
                    }
                }
                else { continue; }
            }
        }

        private FaceBoundingBox ExtractBoundingBox(string line)
        {
            var csvSplit = line.Split(',').ToList();
            csvSplit.RemoveAt(0);
            var leftTopCoordinate = new PixelCoordinateVertex()
            {
                x = int.Parse(csvSplit[0]),
                y = int.Parse(csvSplit[1])
            };
            var rightBottomCoordinate = new PixelCoordinateVertex()
            {
                x = int.Parse(csvSplit[2]),
                y = int.Parse(csvSplit[3])
            };
            return new FaceBoundingBox()
            {
                LeftTopCoordinate = leftTopCoordinate,
                RightBottomCoordinate = rightBottomCoordinate
            };
        }

        private void HandleIdentities()
        {
            foreach (var line in face_recognitionLines)
            {
                var imageDir = GetImageDir(line);
                var snapshot = Results.Where(x => x.ImageDir == imageDir).FirstOrDefault();
                if (snapshot == null)
                    throw new Exception("No snapshot found by that image directory.");
                var status = SetIdentityOutcome(line);
                snapshot.Status = status;
                if (snapshot.Status == Snapshot.SnapshotStatus.known || snapshot.Status == Snapshot.SnapshotStatus.unknown_person)
                {
                    AssignIdentifiedPersons(snapshot, line);
                }
            }
        }

        private void AssignIdentifiedPersons(Snapshot snapshot, string line)
        {
            snapshot.People.Add(new Person()
            {
                Name = GetIdentifiedName(line)
            });
        }

        private string GetImageDir(string line)
        {
            return line.Split(',')[0];
        }

        private Snapshot.SnapshotStatus SetIdentityOutcome(string line)
        {
            var statusLine = line.Substring(line.LastIndexOf(',') + 1);
            if (statusLine == "no_persons_found")
            {
                return Snapshot.SnapshotStatus.no_persons_found;
            }
            else if (statusLine == "unknown_person")
            {
                return Snapshot.SnapshotStatus.unknown_person;
            }
            else
            {
                return Snapshot.SnapshotStatus.known;
            }
        }

        private string GetIdentifiedName(string line)
        {
            var name = line.Split(',').Last();
            return name;
        }

        private void InstantiateSnapshotsFromDirs()
        {
            if (_knownImageDirs.Count == 0)
            {
                Console.WriteLine("No known files found.");
            }
            Console.WriteLine($"{_knownImageDirs.Count} known images.");
            foreach (var imageDir in _knownImageDirs)
            {
                DateTime creation = _fileServices.GetCreationDateTime(imageDir);
                var person = new Person()
                {
                    Name = _fileServices.GetFileNameFromDir(imageDir).Split('.').First()
                };
                var people = new List<Person>();
                people.Add(person);
                Known.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = _fileServices.GetCreationDateTime(imageDir),
                    ImageName = _fileServices.GetFileNameFromDir(imageDir),
                    People = people
                });
            }

            if (_unknownImageDirs.Count == 0)
            {
                Console.WriteLine("No unknown files found.");
            }
            Console.WriteLine($"{_unknownImageDirs.Count} unknown images.");
            foreach (var imageDir in _unknownImageDirs)
            {
                Unknown.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = _fileServices.GetCreationDateTime(imageDir),
                    ImageName = _fileServices.GetFileNameFromDir(imageDir)
                });
            }
        }

        private void CollectAllImageDirs()
        {
            _knownImageDirs = new List<string>(_fileServices.GetAllImageDirs(knownPath));
            System.Console.WriteLine("_knownImageDirs");
            _knownImageDirs.ForEach(x => System.Console.WriteLine(x));

            _unknownImageDirs = new List<string>(_fileServices.GetAllImageDirs(unknownPath));
            System.Console.WriteLine("_unknownImageDirs");
            _unknownImageDirs.ForEach(x => System.Console.WriteLine(x));
        }
    }
}