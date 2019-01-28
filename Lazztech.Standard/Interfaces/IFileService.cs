using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Standard.Interfaces
{
    public interface IFileService
    {
        bool Exists(string path);
        void CreateDirectory(string path);
        void WriteAllBytes(string path, byte[] bytes);
        void WriteAllText(string path, string content);
        string GetExtension(string path);
    }
}
