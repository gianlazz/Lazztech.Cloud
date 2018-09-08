using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain
{
    public class FaceDetectionProcess : Iface_detection
    {
        public Process process { get; private set; }
        public List<string> result { get; private set; }

        public List<string> FaceDetection()
        {
            result = new List<string>();

            var procInfo = new ProcessStartInfo($"face_detection")
            {
                RedirectStandardOutput = true,
                Arguments = $"{FacialRecognitionManager.unknownPath}"
            };
            process = new Process { StartInfo = procInfo };
            process.OutputDataReceived += new DataReceivedEventHandler(OnProcOutputDataRecieved);
            process.ErrorDataReceived += new DataReceivedEventHandler(OnProcErrorDataRecieved);
            process.Start();
            //while (process.StandardOutput.EndOfStream == false)
            //{
            //    var line = process.StandardOutput.ReadLine();
            //    if (string.IsNullOrEmpty(line) == false)
            //        result.Add(line);
            //    Console.WriteLine(line);
            //}

            return result;
        }

        private void OnProcOutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            var line = e.Data;
            if (string.IsNullOrEmpty(line) == false)
                result.Add(line);
            Console.WriteLine(line);
        }

        private void OnProcErrorDataRecieved(object sender, DataReceivedEventArgs e)
        {
            throw new Exception($"{e.Data}");
        }
    }
}