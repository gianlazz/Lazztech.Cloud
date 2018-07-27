using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresenses.Vision.Microservice.Domain;
using Lazztech.ObsidianPresenses.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Cli
{
    public class Program
    {
        public static List<string> SerializedResults = new List<string>();

        public static void Main(string[] args)
        {
            var resultsPath = @"/face/";
            Console.WriteLine("Press any key to begin facial recognition processing.");
            //Console.ReadLine();
            Console.WriteLine("Checking shared volumn is valid.");
            var volumeIsValid = Directory.Exists(resultsPath);
            Console.WriteLine($"Shared volume exists: {volumeIsValid}.");
            if (volumeIsValid == false)
            {
                System.Threading.Thread.Sleep(1000);
                return;
            }

            var face_recognition = new Face_RecognitionManager();
            face_recognition.Process();

            foreach (var snapshot in face_recognition.Results)
            {
                SerializedResults.Add(
                    JsonConvert.SerializeObject(snapshot, Formatting.Indented)
                );
            }
            SerializedResults.ForEach(json => Console.WriteLine(json));

            Console.ReadLine();
        }

        private static void Gcv()
        {
            //var binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            //var resultsPath = binaryPath + "/Results";

            //var imageBytes = File.ReadAllBytes(inputPath);
            //var base64 = Convert.ToBase64String(imageBytes);

            //var gcv = new GoogleCloudVisionParser();
            //var snapshot = gcv.Process(base64);
        }
    }
}
