using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class GuestRequest
    {
        public int numGuestRequest { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public Enums.StatusClient Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Enums.Area Area { get; set; }
        public Enums.SubArea SubArea { get; set; }
        public Enums.KindOfVication Type { get; set; }
        public int Price { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public Enums.Option Pool { get; set; }
        public Enums.Option Jacuzzi { get; set; }
        public Enums.Option Garden { get; set; }
        public Enums.Option ChildrensAttractions { get; set; }
        public Enums.Option Breakfast { get; set; }


        public override string ToString()
        {
            Console.WriteLine("");
            return ("Client details:\nName: " + PrivateName +" "+ FamilyName + "\nDates: " + EntryDate + " - " + ReleaseDate + "\nStatus: " + Status);
        }
    }
}
