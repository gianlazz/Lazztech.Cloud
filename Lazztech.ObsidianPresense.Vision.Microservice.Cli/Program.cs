using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazztech.ObsidianPresense.Vision.Microservice.GoogleCloudVision;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Filters;
using SixLabors.ImageSharp.Processing.Transforms;

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
            var inputPath = Console.ReadLine().Trim();

            var imageBytes = File.ReadAllBytes(inputPath);
            var base64 = Convert.ToBase64String(imageBytes);

            var gcv = new GoogleCloudVisionParser();

            var snapshot = gcv.Process(base64);

            var json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
            Console.WriteLine(json);

            Console.WriteLine("Drawing bounding boxes.");

            // Image.Load(string path) is a shortcut for our default type. Other pixel formats use Image.Load<TPixel>(string path))
            using (Image<Rgba32> image = Image.Load(inputPath))
            {
                //var context = new 
                //SixLabors.ImageSharp.Processing.Drawing.DrawLineExtensions.Dr
                image.Mutate(x => x
                     .Resize(image.Width / 2, image.Height / 2)
                     .Grayscale());
                image.Save(inputPath.TrimEnd(".jpg".ToCharArray()) + "_bar.jpg"); // automatic encoder selected based on extension.
            }

            Console.ReadKey();
        }
    }
}
