using System.Diagnostics.Contracts;

namespace ShipSelector.Models
{
    public class BroadsideSDS
    {

        public string ShipName { get; set; }
        public string ImagePath { get; set; }
        public int CurrentSpeed { get; set; } = 0;
        public int HullHits { get; set; } = 0;
        public int FloodingHits { get; set; } = 0;
        public List<DamageCard> DamageCards { get; set; } = new List<DamageCard>();
        public int selectedOrderCard { get; set; } = 0;
        public string selectedOrderImagePath { get; set; } = string.Empty;
        public bool orderIsActive { get; set; } = false;
        public bool torpsFired { get; set; } = false;
        public bool starTorpsFired { get; set; } = false;
        public bool portTorpsFired { get; set; } = false;
        public bool renameHidden { get; set; } = true;
    }

    public class DamageCardData
    {
        //This is the source card from the JSON. 
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int id { get; set; }
        public int number_cards { get; set; }
    }

    public class DamageCard
    {
        //This is the card object that goes into a deck
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int id { get; set; }
    }

    public class OrderCard
    {
        //can only 1 Order card
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int id { get; set; }
    }


}
