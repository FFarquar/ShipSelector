using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Shared
{
    public class UploadResult
    {
        public bool Uploaded { get; set; }
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? ServerPath { get; set; } //Dean added this to get the folder structure location
        public int ErrorCode { get; set; }
        public string MimeType { get; set; }
    }
}