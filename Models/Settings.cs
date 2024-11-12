using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShipSelector.Models
{
    //a class that exposes settings across the client and server
    public static class Settings
    {
        public const  string styleForButtonAvailable = "btn btn-primary";
        public const string styleForButonNotAvailable = "btn btn-secondary";
        public const long tokenLengthDays = 7;
        public const int maxFilesToUpload = 1;
        public const long maxFileSize = 4000000;
        public const string version = "1.6";
    }
}
