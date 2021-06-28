using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BE
{
    public class HostingUnit
    {
        public int numHostingUnitKey { get; set; }
        public Host Owner { get; set; }
        public string HostingUnitName { get; set; }

        [XmlIgnore]
        public bool[,] diary { get; set; }

        [XmlArray("diary")]
        public bool[] diaryArray
        {
            get{ return diary.flatten(); }
            set { diary = value.expand(12); }
        }
        public Enums.Area Area { get; set; }
        public Enums.SubArea SubArea { get; set; }
        public string Address { get; set; }
        public int price { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        public bool ChildrensAttractions { get; set; }
        public bool Breakfast { get; set; }
        public int Capacity { get; set; }
        public Enums.KindOfVication TypeOfHost { get; set; }



        public override string ToString()
        {
            Console.WriteLine("");
            return ("Host unit details:\nName: " + HostingUnitName + "\nOwner: " + Owner.PrivateName + " " + Owner.FamilyName);
        }
    }
}
