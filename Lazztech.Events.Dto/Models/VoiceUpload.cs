﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dto.Models
{
    public class VoiceUpload
    {
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public VoiceUpload()
        {
            Id = Guid.NewGuid();
            UploadDate = DateTime.Now;
        }
    }
}
