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

        //SHOULD THESE BE LIST OF PEOPLE OBJECTS?

        public Face_RecognitionManager()
        {
            bool result;
            result = Directory.Exists(_knownPath);
            result = Directory.Exists(_unknownPath);
            result = Directory.Exists(_knownUnkownPath);
        }

        public void Process()
        {
            Process(_knownPath, _unknownPath);
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
        
        public void Process(string known, string unknown)
        {
            if (CheckAllAssetsValid() == false)
                throw new Exception("Input not valid");

            var lines = new List<string>();

            //try
            //{
            //    var procStartInfo = new ProcessStartInfo()
            //    {
            //        RedirectStandardOutput = true,
            //        Arguments = $"face_recognition {known} {unknown}",
            //    };

            //    var proc = new Process { StartInfo = procStartInfo };
            //    proc.Start();
            //    while (proc.StandardOutput.EndOfStream == false)
            //    {
            //        var line = proc.StandardOutput.ReadLine();
            //        if (string.IsNullOrEmpty(line) == false)
            //            lines.Add(line);
            //    }
            //    proc.WaitForExit();
            //    //return lines;
            //}
            //catch { throw; }

            var procInfo = new ProcessStartInfo($"face_recognition")
            { 
                RedirectStandardOutput = true,
                WorkingDirectory = "/",
                Arguments = "face/known/ face/unknown"
            };
            var proc = new Process { StartInfo = procInfo };
            proc.Start();
            while (proc.StandardOutput.EndOfStream == false)
            {
                var line = proc.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    lines.Add(line);
            }
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}
