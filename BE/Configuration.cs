using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Configuration
    {
        public static int GuestRequestKey = 100;
        public static int HostingUnitKey = 100000;
        public static int HostKey = 10000;
        public static int OrderKey=100;
        public static int fee = 10; //עמלה למנהל האתר
        public static int DayForRequest = 60; //זמן מקסימלי שבקשה פתוחה
    }
}
