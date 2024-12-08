using System.Diagnostics.Contracts;

namespace ShipSelector.Models
{
    public class BroadsideSDS
    {
        private int hullhits;
        public int HullHits {
            get
            {
                return hullhits;
            }
            set
            {
                hullhits = value;
                setSummaryText();
            }
        }

        private int floodingHits;

        public int FloodingHits
        {
            get { return floodingHits; }
            set { 
                floodingHits = value;
                setSummaryText();
            }
        }

        private string shipName;

        public string ShipName
        {
            get { return shipName; }
            set { 
                shipName = value;
                setSummaryText() ;
            }
        }

        private int currentSpeed;

        public int CurrentSpeed
        {
            get { return currentSpeed; }
            set { 
                currentSpeed = value;
                setSummaryText();
            }
        }

        //private List<DamageCard> damageCards= new List<DamageCard>();

        //public List<DamageCard> DamageCards
        //{
        //    get { return damageCards; }
        //    set { 
        //        damageCards = value;
        //        setSummaryText();
        //    }
        //}

        private string directedGunTag;

        public string DirectedGunTag
        {
            get { return directedGunTag; }
            set { 
                directedGunTag = value;
                setSummaryText();
            }
        }

        //public string ShipName { get; set; }
        public string ImagePath { get; set; }
        //public int CurrentSpeed { get; set; } = 0;

        //public int FloodingHits { get; set; } = 0;
        public List<DamageCard> DamageCards { get; set; } = new List<DamageCard>();
        public int selectedOrderCard { get; set; } = 0;
        public string selectedOrderImagePath { get; set; } = string.Empty;
        public bool orderIsActive { get; set; } = false;
        public bool torpsFired { get; set; } = false;
        public bool starTorpsFired { get; set; } = false;
        public bool portTorpsFired { get; set; } = false;
        public bool renameHidden { get; set; } = true;
        public string summaryText { get; set; } = string.Empty;

        public void setSummaryText()
        {
            summaryText = "S:" + CurrentSpeed.ToString() + " HH:" + HullHits.ToString() +" F:" + FloodingHits.ToString() + " DC:" + DamageCards.Count.ToString() + " ID:" + DirectedGunTag;     
        }
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
