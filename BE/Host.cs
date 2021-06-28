using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Host
    {
        public int HostKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public int FhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public BankAccount BankAccuontHost { get; set; }
        public bool CollectionClearance { get; set; }//אישור גבייה
        public int numOfUnit { get; set; }

      
        public override string ToString()
        {
            Console.WriteLine("");
            return ("Host details:\nName: " + PrivateName + "-" + FamilyName + " - " + FhoneNumber);
        }
    }
}
