using System;

namespace Lazztech.ObsidianPresences.Vision.Microservice.Domain
{
    public interface IFileServices
    {
         string[] GetAllImageDirs(string path);
         string GetFileNameFromDir(string dir);
         DateTime GetCreationDateTime(string filePath); 
    }
}