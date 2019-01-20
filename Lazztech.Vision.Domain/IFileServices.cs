using System;

namespace Lazztech.Cloud.Vision.Domain
{
    public interface IFileServices
    {
        string[] GetAllImageDirs(string path);

        string GetFileNameFromDir(string dir);

        DateTime GetCreationDateTime(string filePath);
    }
}