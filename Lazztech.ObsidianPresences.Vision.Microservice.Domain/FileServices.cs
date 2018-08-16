using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain
{
    public class FileServices : IFileServices
    {
        public FileServices()
        {
        }
        
        public string[] GetAllImageDirs(string path)
        {
            var result = new List<string>();
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".jpg")));
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".jpeg")));
            result.AddRange(Directory.GetFiles(path).Where(x => x.EndsWith(".png")));
            return result.ToArray();
        }

        public DateTime GetCreationDateTime(string filePath)
        {
            return File.GetCreationTime(filePath);
        }

        public string GetFileNameFromDir(string dir)
        {
            return dir.Substring(dir.LastIndexOf('/') + 1);
        }
    }
}