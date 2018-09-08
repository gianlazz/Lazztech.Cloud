using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain
{
    public class FaceRecognitionProcess : Iface_recognition
    {
        public Process process { get; private set; }
        public List<string> results { get; private set; }

        public List<string> FaceRecognition()
        {
            results = new List<string>();

            //var x = Environment.ProcessorCount;

            var procInfo = new ProcessStartInfo($"face_recognition")
            {
                RedirectStandardOutput = true,
                //RedirectStandardError = true,
                //UseShellExecute = false,
                Arguments = $"{FacialRecognitionManager.knownPath} {FacialRecognitionManager.unknownPath}"
            };
            process = new Process { StartInfo = procInfo };
            process.Start();

            //process.OutputDataReceived += new DataReceivedEventHandler(OnProcOutputDataRecieved);
            //process.ErrorDataReceived += new DataReceivedEventHandler(OnProcErrorDataRecieved);

            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
            while (process.StandardOutput.EndOfStream == false)
            {
                var line = process.StandardOutput.ReadLine();
                if (string.IsNullOrEmpty(line) == false)
                    results.Add(line);
                Console.WriteLine(line);
            }

            return results;
        }

        private void OnProcOutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            var line = e.Data;
            if (string.IsNullOrEmpty(line) == false)
                results.Add(line);
            Console.WriteLine(line);
        }

        private void OnProcErrorDataRecieved(object sender, DataReceivedEventArgs e)
        {
            throw new Exception(e.Data);
        }
    }
}