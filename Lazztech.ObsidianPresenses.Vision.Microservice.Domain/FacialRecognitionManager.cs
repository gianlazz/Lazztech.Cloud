using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class FacialRecognitionManager : IFacialRecognitionManager
    {
        string binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
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
        private IFacialIdentityHandler _facialIdentityHandler;

        public List<Snapshot> Results { get; set; }    
        public List<string> face_recognitionLines = new List<string>();
        public List<string> face_coordinatesLines = new List<string>();

        #region ctor
        public FacialRecognitionManager(IFacialIdentityHandler facialIdentityHandler)
        {
            _facialIdentityHandler = facialIdentityHandler;
            Results = new List<Snapshot>();
        }
        #endregion
        
        public void Process()
        {
            CollectAllImageDirs();
            InstantiateSnapshotsFromDirs();
            face_recognitionLines = _facialIdentityHandler.FaceRecognition();
            FaceDetection();

            Results.AddRange(Known);
            Results.AddRange(Unknown);
            Results.AddRange(KnownUnknown);

            HandleIdentities();
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
            return line.Split(',').Last();
        }

         private void FaceDetection()
         {
            var procInfo = new ProcessStartInfo($"face_detection")
            { 
                RedirectStandardOutput = true,
                Arguments = $"{unknownPath}"
            };
            var proc = new Process { StartInfo = procInfo };

            proc.Start();
            while (proc.StandardOutput.EndOfStream == false)
            {
                var line = proc.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    face_coordinatesLines.Add(line);
                Console.WriteLine(line);
            }
         }

         private string GetFileNameFromDir(string dir)
         {
             return dir.Substring(dir.LastIndexOf('/') + 1);
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
                DateTime creation = File.GetCreationTime(imageDir);
                DateTime modification = File.GetLastWriteTime(imageDir);
                Known.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString(),
                    ImageName = GetFileNameFromDir(imageDir)
                });
            }

            if (_unknownImageDirs.Count == 0)
            {
                Console.WriteLine("No unknown files found.");
            }
            Console.WriteLine($"{_unknownImageDirs.Count} unknown images.");
            foreach (var imageDir in _unknownImageDirs)
            {
                DateTime creation = File.GetCreationTime(imageDir);
                DateTime modification = File.GetLastWriteTime(imageDir);
                Unknown.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString(),
                    ImageName = GetFileNameFromDir(imageDir)
                });
            }

            if (_knownUnknownImageDirs.Count == 0)
            {
                Console.WriteLine("No known_unknown files found.");
            }
            Console.WriteLine($"{_knownUnknownImageDirs.Count} known_unknown files found.");
            foreach (var imageDir in _knownUnknownImageDirs)
            {
                DateTime creation = File.GetCreationTime(imageDir);
                DateTime modification = File.GetLastWriteTime(imageDir);
                KnownUnknown.Add(new Snapshot()
                {
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString(),
                    ImageName = GetFileNameFromDir(imageDir)
                });
            }
        }

        private void CollectAllImageDirs()
        {
            _knownImageDirs.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".jpg")));
            _knownImageDirs.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".jpeg")));
            _knownImageDirs.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".png")));
            _unknownImageDirs.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".jpg")));
            _unknownImageDirs.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".jpeg")));
            _unknownImageDirs.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".png")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(knownUnkownPath).Where(x => x.EndsWith(".jpg")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(knownUnkownPath).Where(x => x.EndsWith(".jpeg")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(knownUnkownPath).Where(x => x.EndsWith(".png")));
        }
    }
}
