using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShipSelector.Models
{
    [Serializable()]
    public class UnitForGameSystemDTO
    {
        public int Id{ get; set; }
        public string? Name_ClassName{ get; set; }
        //public string SubUnitType { get; set; } //A string representation of the subunitypeid
        public SubUnitType? subUnitTypeObj { get; set; }
        //public string Country { get; set; }         //A string representation of the country
        public Country? Countryobj { get; set; }         
        public string Alliance { get; set; }
        public int NumberinClass_shipSub { get; set; }      //a value for ships and subs only. This is the maximum number that can be selected

        public int Cost { get; set; }
        public string? ImagePath{ get; set; }
        public string? ShipsSubsInClass { get; set; }       //The names of the ships and subs I have in this class

        public int NumberSelected { get; set; }                 //This is the number of instances of this unit selected the user selects. If showing ships from my collection, this can not exceed 1
        public bool AllowUnlimitedSelection { get; set; }       //Intended for aircraft and airfields. The NumberClass_ShipSub should be set to something high, like 1000
        public bool AddButtonDisabled { get; set; }       //A flag for the ui to determine what buttons should be disabled
        public bool RemoveButtonDisabled { get; set; }
        public string styleForAddButton { get; set; }       //This is used in the UI to keep track of syle applied to add button
        public string styleForRemoveButton { get; set; }       //This is used in the UI to keep track of syle applied to add button
        public bool FilterVisible{ get; set; }                         //This is used in the UI to kep track of what is visible (filtered)
        //[NonSerialized()]
        public Stream? imageStream = null;

}
}
