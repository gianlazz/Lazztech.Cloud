using System;
using System.Collections.Generic;
using System.Text;

namespace Lazztech.Events.Dal.Dao
{
    public class AudioUpload
    {
        public int AudioUploadId { get; set; }
        public DateTime UploadDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public AudioUpload()
        {
            UploadDate = DateTime.Now;
        }
    }
}
