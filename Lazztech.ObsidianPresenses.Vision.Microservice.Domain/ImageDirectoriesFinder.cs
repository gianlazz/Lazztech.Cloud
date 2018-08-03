using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class ImageDirectoriesFinder : IImageDirectoriesFinder
    {
        public string KnownImageDir { get; set; }
        public string UnknownImageDir { get; set; }
        public string KnownUnknownImagDir { get; set; }
        public string NoPersonsFoundDir { get; set; }

        private static string knownPath = @"/face/known/";
        private static string unknownPath = @"/face/unknown/";
        private static string knownUnknownPath = @"/face/known_unknown/";
        private static string noPersonsFoundPath = @"/face/no_persons_found/";

        public ImageDirectoriesFinder()
        {
            KnownImageDir = knownPath;
            UnknownImageDir = unknownPath;
            KnownUnknownImagDir = knownUnknownPath;
            NoPersonsFoundDir = noPersonsFoundPath;
        }

        public string[] GetAllKnownImageDirs()
        {
            var result = new List<string>();
            result.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".jpg")));
            result.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".jpeg")));
            result.AddRange(Directory.GetFiles(knownPath).Where(x => x.EndsWith(".png")));
            return result.ToArray();
        }

        public string[] GetAllKnownUnknownImageDirs()
        {
            var result = new List<string>();
            result.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".jpg")));
            result.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".jpeg")));
            result.AddRange(Directory.GetFiles(unknownPath).Where(x => x.EndsWith(".png")));
            return result.ToArray();
        }

        public string[] GetAllUnknownImageDirs()
        {
            var result = new List<string>();
            result.AddRange(Directory.GetFiles(knownUnknownPath).Where(x => x.EndsWith(".jpg")));
            result.AddRange(Directory.GetFiles(knownUnknownPath).Where(x => x.EndsWith(".jpeg")));
            result.AddRange(Directory.GetFiles(knownUnknownPath).Where(x => x.EndsWith(".png")));
            return result.ToArray();
        }

        public string[] GetAllNoPersonsFoundImageDirs()
        {
            throw new System.NotImplementedException();
        }
    }
}