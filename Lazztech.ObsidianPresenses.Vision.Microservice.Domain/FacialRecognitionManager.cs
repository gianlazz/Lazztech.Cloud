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
        string _knownPath = @"/face/known/";
        string _unknownPath = @"/face/unknown/";
        string _knownUnkownPath = @"/face/known_unknown/";
        string _noPersonsFoundPath = @"/face/no_persons_found/";

        public List<string> _knownImageDirs = new List<string>();
        public List<string> _unknownImageDirs = new List<string>();
        public List<string> _knownUnknownImageDirs = new List<string>();

        public List<Snapshot> Known = new List<Snapshot>();
        public List<Snapshot> Unknown = new List<Snapshot>();
        public List<Snapshot> KnownUnknown = new List<Snapshot>();

        public List<Snapshot> Results = new List<Snapshot>();
        public List<string> face_recognitionLines = new List<string>();
        public List<string> face_coordinatesLines = new List<string>();

        #region ctor
        public FacialRecognitionManager()
        {
            if (!Directory.Exists(_knownPath))
                Directory.CreateDirectory(_knownPath);
            if (!Directory.Exists(_unknownPath))
                Directory.CreateDirectory(_unknownPath);
            if (!Directory.Exists(_knownUnkownPath))
                Directory.CreateDirectory(_knownUnkownPath);
            if (!Directory.Exists(_noPersonsFoundPath))
                Directory.CreateDirectory(_noPersonsFoundPath);
        }
        #endregion
        
        public void Process()
        {
            CheckAllAssetsValid();
            FaceRecognition();
            FaceDetection();

            Results.AddRange(Known);
            Results.AddRange(Unknown);
            Results.AddRange(KnownUnknown);

            //return Results;
         }

         private void FaceRecognition()
         {
            var procInfo = new ProcessStartInfo($"face_recognition")
            { 
                RedirectStandardOutput = true,
                //WorkingDirectory = "/",
                //Arguments = $"{_knownPath} {_unknownPath}"
                Arguments = $"{_knownPath} {_unknownPath}"
            };
            var proc = new Process { StartInfo = procInfo };

            proc.Start();
            while (proc.StandardOutput.EndOfStream == false)
            {
                var line = proc.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    face_recognitionLines.Add(line);
                Console.WriteLine(line);
            }
         }

         private void FaceDetection()
         {
            var procInfo = new ProcessStartInfo($"face_detection")
            { 
                RedirectStandardOutput = true,
                //WorkingDirectory = "/",
                //Arguments = $"{_knownPath} {_unknownPath}"
                Arguments = $"{_unknownPath}"
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

        private void CheckAllAssetsValid()
        {
            //var filePaths = new List<string>(Directory.GetFiles(resultsPath));
            _knownImageDirs.AddRange(Directory.GetFiles(_knownPath).Where(x => x.EndsWith(".jpg")));
            _knownImageDirs.AddRange(Directory.GetFiles(_knownPath).Where(x => x.EndsWith(".jpeg")));
            _knownImageDirs.AddRange(Directory.GetFiles(_knownPath).Where(x => x.EndsWith(".png")));
            _unknownImageDirs.AddRange(Directory.GetFiles(_unknownPath).Where(x => x.EndsWith(".jpg")));
            _unknownImageDirs.AddRange(Directory.GetFiles(_unknownPath).Where(x => x.EndsWith(".jpeg")));
            _unknownImageDirs.AddRange(Directory.GetFiles(_unknownPath).Where(x => x.EndsWith(".png")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(_knownUnkownPath).Where(x => x.EndsWith(".jpg")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(_knownUnkownPath).Where(x => x.EndsWith(".jpeg")));
            _knownUnknownImageDirs.AddRange(Directory.GetFiles(_knownUnkownPath).Where(x => x.EndsWith(".png")));
            if (_knownImageDirs.Count == 0)
            {
                Console.WriteLine("No known files found.");
            }
            Console.WriteLine($"{_knownImageDirs.Count} known images.");
            foreach (var imageDir in _knownImageDirs)
            {
                DateTime creation = File.GetCreationTime(imageDir);
                DateTime modification = File.GetLastWriteTime(imageDir);
                Known.Add(new Snapshot(){
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString()
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
                Unknown.Add(new Snapshot(){
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString()
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
                KnownUnknown.Add(new Snapshot(){
                    ImageDir = imageDir,
                    DateTimeWhenCaptured = creation.ToString()
                });
            }
        }
    }
}
