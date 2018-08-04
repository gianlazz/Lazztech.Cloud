using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class FacialRecognitionManager
    {
        string binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        #region properties
        public static string knownPath = @"/face/known/";
        public static string unknownPath = @"/face/unknown/";
        public static string knownUnkownPath = @"/face/known_unknown/";
        public static string noPersonsFoundPath = @"/face/no_persons_found/";

        public List<string> _knownImageDirs = new List<string>();
        public List<string> _unknownImageDirs = new List<string>();
        public List<string> _knownUnknownImageDirs = new List<string>();

        public List<Snapshot> Known = new List<Snapshot>();
        public List<Snapshot> Unknown = new List<Snapshot>();
        public List<Snapshot> KnownUnknown = new List<Snapshot>();

        public List<Snapshot> Results { get; set; }    
        public List<string> face_recognitionLines = new List<string>();
        public List<string> face_detectionLines = new List<string>();
        #endregion

        #region services fields
        private IFileServices _fileServices;
        private Iface_detection _face_detection;
        private Iface_recognition _face_recognition;
        #endregion

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
        }
        #endregion
        
        public List<Snapshot> Process()
        {
            CollectAllImageDirs();
            InstantiateSnapshotsFromDirs();
            //SetSnapshotsCreationDateTime();
            face_recognitionLines = _face_recognition.FaceRecognition();
            face_detectionLines = _face_detection.FaceDetection();

            Results.AddRange(Known);
            Results.AddRange(Unknown);
            Results.AddRange(KnownUnknown);

            HandleIdentities();

            return Results;
         }

        private void HandleIdentities()
        {
            foreach (var line in face_recognitionLines)
            {
                var imageDir = GetImageDir(line);
                var snapshot = Results.Where(x => x.ImageDir == imageDir).FirstOrDefault();
                // if (snapshot == null)
                //    throw new Exception("No snapshot found by that image directory.");
                var status = SetIdentityOutcome(line);
                snapshot.Status = status;
                if (snapshot.Status == Snapshot.SnapshotStatus.known)
                {
                    AssignIdentifiedPersons(snapshot, line);
                }
            }
        }

        private void AssignIdentifiedPersons(Snapshot snapshot, string line)
        {
            //CONDITION FOR SINGLE OR MULTIPLE PEOPLE
            snapshot.People.Add(new Person(){
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
            else {
                return Snapshot.SnapshotStatus.known;
            }
        }

        private string GetIdentifiedName(string line)
        {
            //THIS COULD BE MORE COMPLEX IF THERE'S MORE THAN ONE NAME
            return line.Split(',').Last();
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
                Known.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = _fileServices.GetCreationDateTime(imageDir).ToString(),
                    ImageName = _fileServices.GetFileNameFromDir(imageDir)
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
                    DateTimeWhenCaptured = _fileServices.GetCreationDateTime(imageDir).ToString(),
                    ImageName = _fileServices.GetFileNameFromDir(imageDir)
                });
            }

            if (_knownUnknownImageDirs.Count == 0)
            {
                Console.WriteLine("No known_unknown files found.");
            }
            Console.WriteLine($"{_knownUnknownImageDirs.Count} known_unknown files found.");
            foreach (var imageDir in _knownUnknownImageDirs)
            {
                KnownUnknown.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = _fileServices.GetCreationDateTime(imageDir).ToString(),
                    ImageName = _fileServices.GetFileNameFromDir(imageDir)
                });
            }
        }

        private void CollectAllImageDirs()
        {
            _knownImageDirs = new List<string>(_fileServices.GetAllImageDirs(knownPath));
            _unknownImageDirs = new List<string>(_fileServices.GetAllImageDirs(unknownPath));
            _knownUnknownImageDirs = new List<string>(_fileServices.GetAllImageDirs(knownUnkownPath));
        }
    }
}
