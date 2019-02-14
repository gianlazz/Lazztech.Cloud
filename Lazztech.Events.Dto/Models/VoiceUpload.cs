using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Models
{
    public class VoiceUpload
    {
        public int Id { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public VoiceUpload()
        {
            UploadDate = DateTime.Now;
        }
    }
}
