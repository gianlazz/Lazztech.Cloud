using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain.Models;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class Face_RecognitionManager
    {
        string binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        string _knownPath = @"/face/known/";
        string _unknownPath = @"/face/unknown/";
        string _knownUnkownPath = @"/face/known_unknown/";

        public List<Snapshot> Known = new List<Snapshot>();
        public List<Snapshot> Unknown = new List<Snapshot>();
        public List<Snapshot> KnownUnknown = new List<Snapshot>();

        public List<Snapshot> Results = new List<Snapshot>();
        public List<string> face_recognitionLines = new List<string>();
        public List<string> face_coordinatesLines = new List<string>();

        public Face_RecognitionManager()
        {
            if (Directory.Exists(_knownPath))
                Directory.CreateDirectory(_knownPath);
            if (Directory.Exists(_unknownPath))
                Directory.CreateDirectory(_unknownPath);
            if (Directory.Exists(_knownUnkownPath))
                Directory.CreateDirectory(_knownUnkownPath);
        }
        
        public void Process()
        {
            if (CheckAllAssetsValid() == false)
                throw new Exception("Input not valid");

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
                    face_recognitionLines.Add(line);
                Console.WriteLine(line);
            }
         }

        private bool CheckAllAssetsValid()
        {
            //var filePaths = new List<string>(Directory.GetFiles(resultsPath));
            //var jsonFiles = filePaths.Where(x => x.EndsWith(".json"));
            //var imageFiles = filePaths.Where(x => x.EndsWith(".jpg"));
            var knownFiles = Directory.GetFiles(_knownPath);
            var unkownFiles = Directory.GetFiles(_unknownPath);
            var knownUnknownFiles = Directory.GetFiles(_knownUnkownPath);
            if (knownFiles.Length == 0)
            {
                Console.WriteLine("No known files found.");
                return false;
            }
            Console.WriteLine($"{knownFiles.Length} known images.");
            if (unkownFiles.Length == 0)
            {
                Console.WriteLine("No unknown files found.");
                return false;
            }
            Console.WriteLine($"{unkownFiles.Length} unknown images.");
            if (knownUnknownFiles.Length == 0)
            {
                Console.WriteLine("No known_unknown files found.");
            }

            return true;
        }
    }
}
