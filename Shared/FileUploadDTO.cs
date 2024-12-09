using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Shared
{
    public class FileUploadDTO
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public bool FileExistsInFileSystem { get; set; } = false;     //Freshly added files will have a value of false and will need to be written to the server. Existing files will be true, and not require rewriting
                                                                      //This value only used when adding attachments to existing records (stops the same file being uploaded multiple times)
        /// <summary>
        /// Contains a reference to the fileDetail Object. Used when editing records with existing attachments. CAn be null
        /// </summary>
        public FileDetail? fileDetail { get; set; } = null;


    }
}
