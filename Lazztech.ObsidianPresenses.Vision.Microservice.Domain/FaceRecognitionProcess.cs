using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class FaceRecognitionProcess : Iface_recognition
    {
        public List<string> FaceRecognition()
        {
            var results = new List<string>();

            var x = Environment.ProcessorCount;

            var procInfo = new ProcessStartInfo($"face_recognition")
            { 
                RedirectStandardOutput = true,
                Arguments = $"{FacialRecognitionManager.knownPath} {FacialRecognitionManager.unknownPath}"
            };
            var proc = new Process { StartInfo = procInfo };

            proc.Start();
            while (proc.StandardOutput.EndOfStream == false)
            {
                var line = proc.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    results.Add(line);
                Console.WriteLine(line);
            }

            return results;
        }
    }
}