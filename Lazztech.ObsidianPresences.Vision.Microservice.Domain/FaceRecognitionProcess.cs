using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lazztech.Cloud.Vision.Microservice.Domain
{
    public class FaceRecognitionProcess : Iface_recognition
    {
        public Process process { get; private set; }
        public List<string> StdoutResults { get; private set; }
        public List<string> StderrResults { get; private set; }

        public List<string> FaceRecognition()
        {
            StdoutResults = new List<string>();
            StderrResults = new List<string>();

            var procInfo = new ProcessStartInfo()
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = "face_recognition",
                Arguments = $"{FacialRecognitionManager.knownPath} {FacialRecognitionManager.unknownPath}"
            };
            process = new Process { StartInfo = procInfo };
            process.Start();

            process.OutputDataReceived += new DataReceivedEventHandler(OnProcOutputDataRecieved);
            process.ErrorDataReceived += new DataReceivedEventHandler(OnProcErrorDataRecieved);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            if (StderrResults.Count > 0)
                throw new Exception("face_recognition process threw an error.");

            return StdoutResults;
        }

        private void OnProcOutputDataRecieved(object sender, DataReceivedEventArgs e)
        {
            var line = e.Data;
            if (string.IsNullOrEmpty(line) == false)
                StdoutResults.Add(line);
            Console.WriteLine(line);
        }

        private void OnProcErrorDataRecieved(object sender, DataReceivedEventArgs e)
        {
            var line = e.Data;
            if (string.IsNullOrEmpty(line) == false)
                StderrResults.Add(line);
            Console.WriteLine(line);
        }
    }
}