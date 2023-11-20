using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Models
{
    public class Country
    {
        public int Id{ get; set; }
        public string CountryName{ get; set; }
        //public Alliance? Alliance { get; set; }
        public int AllianceId { get; set; }
    }

    public class Alliance
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
