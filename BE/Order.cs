using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Order
    {
        public int numOrderKey { get; set; }
        public int numHostingUnitKey { get; set; }
        public int numGuestRequest { get; set; }
        public Enums.StatusOrder Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }

       
        public override string ToString()
        {
            Console.WriteLine("");
            return ("Order details:\nOrder key: " + numOrderKey + "\nStatus: " + Status);
        }
    }
}
