using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain
{
    public class FaceDetectionProcess : Iface_detection
    {
        public List<string> FaceDetection()
        {
            var result = new List<string>();

            var procInfo = new ProcessStartInfo($"face_detection")
            {
                RedirectStandardOutput = true,
                Arguments = $"{FacialRecognitionManager.unknownPath}"
            };
            var proc = new Process { StartInfo = procInfo };

            proc.Start();
            while (proc.StandardOutput.EndOfStream == false)
            {
                var line = proc.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    result.Add(line);
                Console.WriteLine(line);
            }

            return result;
        }
    }
}