using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresense.Vision.Microservice.Cli
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var binaryPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var resultsPath = binaryPath + "/Results";
            //var filePaths = new List<string>(Directory.GetFiles(resultsPath));
            //var jsonFiles = filePaths.Where(x => x.EndsWith(".json"));
            //var imageFiles = filePaths.Where(x => x.EndsWith(".jpg"));

            // Load the image file into memory
            var inputPath = Console.ReadLine();

            var imageBytes = File.ReadAllBytes(inputPath);
            var base64 = Convert.ToBase64String(imageBytes);

            var gcv = new GoogleCloudVisionParser();

            var snapshot = gcv.Process(base64);

            var json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}
