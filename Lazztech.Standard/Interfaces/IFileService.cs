using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Standard.Interfaces
{
    public interface IFileService
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);
        void DeleteFile(string path);
        void CreateDirectory(string path);
        void WriteAllBytes(string path, byte[] bytes);
        void WriteAllText(string path, string content);
        string GetExtension(string path);
    }
}
