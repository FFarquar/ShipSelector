using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Models
{
    //A class for holding sub unit types. Ship sub types are CV, CVE, CVL etc
    //Submarines will just be SS
    //Aircraft will be Fighters, Dive Bomber etc
    public class SubUnitType
    {
        public int Id { get; set; }
        public UnitType UnitType { get; set; }     //The UnitType the is realted too
        public int UnitTypeId { get; set; }             //The ID of the unit type (Ship, Plane, Sub)
        public string SubUnitName{ get; set; }      //the name  given, CVE, CV etc
        public Color PrintColour { get; set; }          //The colour to use to print the name on the print page  
    }

    public class SubUnitTypeDTO
    {
        public int Id { get; set; }
        public string UnitTypeName { get; set; }     //The UnitType the is realted too
        public int UnitTypeId { get; set; }             //The ID of the unit type (Ship, Plane, Sub)
        public string SubUnitName { get; set; }      //the name  given, CVE, CV etc
        public RGBDetails RGBDetails { get; set; }          //The R colour Name property from the Colour Object
    }

    public class RGBDetails
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
