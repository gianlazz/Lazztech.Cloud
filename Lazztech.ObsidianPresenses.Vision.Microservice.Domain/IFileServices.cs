using System;

namespace Lazztech.ObsidianPresenses.Vision.Microservice.Domain
{
    public interface IFileServices
    {
         string[] GetAllImageDirs(string path);
         string GetFileNameFromDir(string dir);
         DateTime GetCreationTime(string filePath); 
    }
}