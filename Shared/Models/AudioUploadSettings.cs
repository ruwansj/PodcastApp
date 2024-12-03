using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class AudioUploadSettings
    {
        public string StorageConnectionString { get; set; }
        public string ContainerName { get; set; }
        public long MaxFileSize { get; set; }
        public string[] AllowedFileTypes { get; set; }
    }
}
