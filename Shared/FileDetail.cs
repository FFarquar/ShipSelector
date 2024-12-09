using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Shared
{
    //A class to handle the details of a file to be stored in the database.
    //This can be used by any class. 
    //ClassOfRelatedEntity will store the class name of the related entity, Will do some trickery
    //to find the actual class name later
    public class FileDetail
    {
        public int id { get; set; }
        public string OriginalFileName { get; set; }
        public int RuleSetId { get; set; }      
        public int CountryId { get; set; }
        //public string StoredFileName { get; set; }
        //public string ServerPath { get; set; }
       // public string MimeType { get; set; }
        //public string ClassOfRelatedEntity { get; set; }        //Any class could store a file
        //public int IdOfRelatedEntity{ get; set; }        
    }
}
