using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Lazztech.ObsidianPresences.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Cli
{
    public class Program
    {
        private static List<string> SerializedResults = new List<string>();
        public static List<Snapshot> Results { get; set; }

        public static void Main(string[] args)
        {
            Results = new List<Snapshot>();

            var resultsPath = @"/face/";
            Console.WriteLine("Begining facial recognition processing.");
            //Console.ReadLine();
            Console.WriteLine("Checking shared volumn is valid.");
            var volumeIsValid = Directory.Exists(resultsPath);
            Console.WriteLine($"Shared volume exists: {volumeIsValid}.");
            if (volumeIsValid == false)
            {
                System.Threading.Thread.Sleep(1000);
                return;
            }
            //DESERIALIZE EXISTING SNAPSHOTS

            //PROCESS NEW ONES
            var facialRecognition = new FacialRecognitionManager(new FaceRecognitionProcess(), new FaceDetectionProcess(), new FileServices());
            Results = facialRecognition.Process();

            //SERIALIZE THE RESULTS
            foreach (var snapshot in facialRecognition.Results)
            {
                SerializedResults.Add(
                    JsonConvert.SerializeObject(snapshot, Formatting.Indented)
                );
            }

            //CONSOLE LOG THE JSON RESULTS
            SerializedResults.ForEach(json => Console.WriteLine(json));

            //WRITE OUT/PERSIST THE JSON RESULTS TO DISK
            if (!Directory.Exists($"{resultsPath}/results/"))
                Directory.CreateDirectory($"{resultsPath}/results/");
            foreach (var snapshot in Results)
            {
                var json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
                var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss");
                File.WriteAllText($"{resultsPath}/results/{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json",json);
            }

            Console.ReadLine();
        }
    }
}
