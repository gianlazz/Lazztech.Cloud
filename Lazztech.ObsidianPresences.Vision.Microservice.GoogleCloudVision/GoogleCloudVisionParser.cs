using Google.Cloud.Vision.V1;
using System;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Lazztech.Cloud.Vision.Microservice.Domain.Models;

namespace Lazztech.Cloud.Vision.Microservice.GoogleCloudVision
{
    public class GoogleCloudVisionParser
    {
        public Snapshot Process(string base64)
        {
            var snap = new Snapshot();
            snap.Location = "Seattle";
            snap.People = new List<Person>();

            var bytes = Convert.FromBase64String(base64);

            // Instantiates a client
            var client = ImageAnnotatorClient.Create();

            var image = Image.FromBytes(bytes);

            IReadOnlyList<FaceAnnotation> result = client.DetectFaces(image);
            var response = client.DetectFaces(image);

            foreach (var face in response)
            {
                var person = new Person();
                person.Name = "Unknown";

                // var faceBox = new FaceBoundingBox();
                snap.People.Add(person);

                var boundingPolyVertices = face.BoundingPoly.Vertices;

                Console.WriteLine($"Confidence: {(int)(face.DetectionConfidence * 100)}" +
                                  $"%; BoundingPoly: {face.BoundingPoly}");
            }

            return snap;
        }
    }
}