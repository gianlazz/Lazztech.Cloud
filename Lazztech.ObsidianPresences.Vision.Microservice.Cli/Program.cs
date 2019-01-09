using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.Cloud.Vision.Microservice.Domain;
using Lazztech.Cloud.Vision.Microservice.Domain.Models;
using Lazztech.Cloud.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;

namespace Lazztech.Cloud.Vision.Microservice.Cli
{
    public class Program
    {
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

            //WRITE OUT/PERSIST THE JSON RESULTS TO DISK
            foreach (var snapshot in Results)
            {
                var json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
                var date = snapshot.DateTimeWhenCaptured.ToString("dd-MM-yyyy-hh-mm-ss-tt");
                if (snapshot.ImageDir.Contains("/known"))
                    File.WriteAllText($"{FacialRecognitionManager.knownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json", json);
                if (snapshot.ImageDir.Contains("/unknown"))
                {
                    File.WriteAllText($"{FacialRecognitionManager.unknownJsonsPath}{date}_{snapshot.ImageName}_{snapshot.GetHashCode()}.json", json);
                    Console.WriteLine(json);
                }
            }

            Console.ReadLine();
        }
    }
}
