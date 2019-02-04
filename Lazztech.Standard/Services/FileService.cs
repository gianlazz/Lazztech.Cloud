using Lazztech.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lazztech.Standard.Services
{
    public class FileService : IFileService
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            var dir = Path.GetDirectoryName(path);
            if (!DirectoryExists(dir))
                CreateDirectory(dir);
            File.WriteAllBytes(path, bytes);
        }

        public void WriteAllText(string path, string content)
        {
            var dir = Path.GetDirectoryName(path);
            if (!DirectoryExists(dir))
                CreateDirectory(dir);
            File.WriteAllText(path, content);
        }
    }
}
