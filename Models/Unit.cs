using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSelector.Models
{
    public class Unit
    {
        public int Id { get; set; }
        //public UnitType UnitType{ get; set; }
        //public int UnitTypeId { get; set; }
        public SubUnitType? SubUnitType { get; set; }
        public int SubUnitTypeId{ get; set; }
        public Country? Country { get; set; }
        public int CountryId { get; set; }
        //public AxisAndAlliesDetails AxisAndAlliesDetails { get; set; }
        //public NimitzDetails NimitzDetails { get; set; }
        public int NumberinClass_shipSub { get; set; }      //The max number of ships subs that can be selected. Number produced. For aircraft, this is the number of models I have
        public string? Name_ClassName { get; set; }      //Name of plane, or class name for ship or sub
        public string? ShipsSubsInClass{ get; set; }      //Names of members of this class in my collection
    }

    public class GameSystemUnitSpecificDetail
    {
        public int Id { get; set; }
        public Unit? Unit { get; set; }
        public int UnitId { get; set; }
        public int Cost { get; set; }
        public string? ImagePath { get; set; }     //The path to the related SDC image.
        public RuleSet? Ruleset { get; set; }
        public int RulesetId { get; set; }
    }

    public class RuleSet
    {
        public int Id { get; set; }
        public string RulesetName { get; set; }
        public bool AllowUnlimitedNumberOfAirCraftSelections { get; set; }      //Nimitz is not limited to selection of planes based on the number in my colleciton
    }
    public class UnitWithGameSystemDetails
    {
        public int Id { get; set; }
        public Unit Unit { get; set; }
        public List<GameSystemUnitSpecificDetail> GameSpecificDetailList { get; set; }
        public List<String> RuleSetName{ get; set; }
    }
}
