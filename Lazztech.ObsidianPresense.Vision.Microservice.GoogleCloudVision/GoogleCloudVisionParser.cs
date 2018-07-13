using Google.Cloud.Vision.V1;
using System;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Lazztech.ObsidianPresense.Vision.Microservice.Domain.Models;

namespace Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision
{
    public class GoogleCloudVisionParser
    {
        public Snapshot Process(string base64)
        {
            var snap = new Snapshot();

            var bytes = Convert.FromBase64String(base64);

            // Instantiates a client
            var client = ImageAnnotatorClient.Create();

            var image = Image.FromBytes(bytes);
            var response = client.DetectFaces(image);


            var json = response.ToString();

            return snap;
        }
    }
}