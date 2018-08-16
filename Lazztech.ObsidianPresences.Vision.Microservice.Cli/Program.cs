﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain;
using Lazztech.ObsidianPresences.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Cli
{
    public class Program
    {
        public static List<string> SerializedResults = new List<string>();

        public static void Main(string[] args)
        {
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

            var facialRecognition = new FacialRecognitionManager(new FaceRecognitionProcess(), new FaceDetectionProcess(), new FileServices());
            facialRecognition.Process();

            foreach (var snapshot in facialRecognition.Results)
            {
                SerializedResults.Add(
                    JsonConvert.SerializeObject(snapshot, Formatting.Indented)
                );
            }
            SerializedResults.ForEach(json => Console.WriteLine(json));

            Console.ReadLine();
        }
    }
}