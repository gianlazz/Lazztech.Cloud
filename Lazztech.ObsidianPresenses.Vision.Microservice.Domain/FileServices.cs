using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public class FileServices : IFileServices
    {
        public FileServices()
        {
        }

        //THIS SHOULD REPLACE IImageDirectoriesFinder
        public string[] GetAllImageDirs(string path)
        {
            var result = new List<string>();
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".jpg")));
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".jpeg")));
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".png")));
            return result.ToArray();
        }
    }
}