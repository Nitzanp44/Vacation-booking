//Nitzan Popovich 206584104
//Reut Levenberg 318731916

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BE;
using BL;
using DS;

namespace PL
{

    public class PL_class
    {

        public static void Main(string[] args)//מסך ראשי- בחירת משתמש(לקוח, מארח, בעל האתר
        {
            IBL bl = BL.FactoryBL.GetBl();
            int user;
            do
            {
                Console.WriteLine(@"Welcome to our site! your perfect vacation is in our hand.
Exit- press 0
Client-press 1
Host-press 2
Manager site- press 3");

                string user1 = Console.ReadLine();
                user = int.Parse(user1);
                switch (user)
                {
                    case 0:
                        Console.WriteLine("goodBye");
                        break;
                    case 1:
                        caseUserClient(bl);
                        break;
                    case 2:
                        caseUserHost(bl);
                        break;
                    case 3:
                        caseUserManager(bl);
                        break;
                    default:
                        break;
                }
            } while (user != 0);
        }

        public static void caseUserClient(IBL bl)//מסך לקוח-הוספת בקשה (הוספנו מהרשימות שיצרנו(
        {
            //IBL bl = BL.FactoryBL.GetBl();
            string choice1;
            do
            {
                Console.WriteLine(@"for add new request- prass 1
for exit-press 0");
                choice1 = Console.ReadLine();
                switch (choice1)
                {
                    case "1":

                        //GuestRequest guestRequest = new GuestRequest()
                        //{
                        //    //PrivateName = "Nitzan",
                        //    //FamilyName = "Peled",
                        //    //MailAddress = "Nitzanp447@gmail.com",
                        //    //EntryDate = new DateTime(2020, 3, 12),
                        //    //ReleaseDate = new DateTime(2020, 3, 16),
                        //    //Area = BE.Enums.Area.מרכז,
                        //    //SubArea = BE.Enums.SubArea.תל_אביב,
                        //    //Type = BE.Enums.KindOfVication.זוגי,
                        //    //Price=Enums.Price.יותר_מ_1000,
                        //    //Adults = 2,
                        //    //Children = 0,
                        //    //Pool = BE.Enums.Option.אפשרי,
                        //    //Jacuzzi = BE.Enums.Option.הכרחי,
                        //    //Garden = BE.Enums.Option.אפשרי,
                        //    //ChildrensAttractions = BE.Enums.Option.לא_מעוניין,
                        //    //Breakfast = BE.Enums.Option.הכרחי
                        //};
                        try
                        {
                            bl.addRequest(DataSource.guestRequests[0]);
                            bl.addRequest(DataSource.guestRequests[1]);
                        }
                        catch (FormatException s)
                        {
                            Console.WriteLine(s);
                        }

                        break;


                    case "0":
                        break;
                }
            } while (choice1 != "0");

        }

        public static void caseUserHost(IBL bl)//מסך מארח-כל האפשרויות שמארח ירצה לעשות באתר
        {

            //IBL bl = BL.FactoryBL.GetBl();
            bl.addHostUnit(DataSource.hostingUnits[0]);
            string choice1;
            Host host = null;
            Console.WriteLine("enter your key");
            choice1 = Console.ReadLine();
            int num;
            while (host == null)
            {
                try
                {
                    num = int.Parse(choice1);
                    host = bl.getBLHost(num);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("enter your key");
                    choice1 = Console.ReadLine();
                }
            }
            do
            {
                try
                {
                    Console.WriteLine(@"for add new host unit- prass 1
for delete host unit-press 2
for update host unit- press 3
for add new order- press 4
for update order- press 5
for get the guest requests- press 6
for get the guest requests by parameter- press 7
for get the guest requests by area- press 8
for get the close order- press 9
for get the open order- press 10
for exit- press 0
");
                    choice1 = Console.ReadLine();
                    switch (choice1)
                    {
                        case "1":
                            //HostingUnit hosting = bl.allHostingUnits().FirstOrDefault();

                            bl.addHostUnit(DataSource.hostingUnits[1]);
                            break;
                        case "2":
                            Console.WriteLine("enter the host unit key to delete");
                            choice1 = Console.ReadLine();
                            num = int.Parse(choice1);
                            HostingUnit hosting1 = bl.getBLHostingUnit(num);
                            bl.removeHostUnit(hosting1);
                            break;
                        case "3":
                            Console.WriteLine("enter the host unit key to update");
                            choice1 = Console.ReadLine();
                            num = int.Parse(choice1);
                            HostingUnit hosting2 = bl.getBLHostingUnit(num);
                            hosting2.ChildrensAttractions = false;
                            bl.updateHostUnit(hosting2);
                            break;
                        case "4":
                            //Order ordi = bl.allOrders().FirstOrDefault();
                            bl.addOrder(DataSource.orders[0]);
                            bl.addOrder(DataSource.orders[1]);
                            break;
                        case "5":
                            Console.WriteLine("enter the order to update");
                            choice1 = Console.ReadLine();
                            num = int.Parse(choice1);
                            Order order = bl.getBLOrder(num);
                            bl.updateOrder(order.numOrderKey, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                            break;
                        case "6":
                            List<GuestRequest> guestRequests = bl.allGuestRequest();
                            if (guestRequests != null)
                            {
                                foreach (GuestRequest item in guestRequests)
                                    Console.WriteLine(item.ToString());
                                Console.WriteLine("");
                            }
                            else
                                Console.WriteLine("אין בקשות אירוח.");
                            break;
                        case "7":
                            List<GuestRequest> lst = bl.filterGuestByParameter(x => x.Pool == Enums.Option.הכרחי);
                            if (lst != null)
                            {
                                foreach (GuestRequest item in lst)
                                    Console.WriteLine(item.ToString());
                                Console.WriteLine("");
                            }
                            else
                                Console.WriteLine("אין בקשות העונות על דרישה זו.");
                            break;
                        case "8":
                            var filterGuestByArea = bl.filterGuestByArea();
                            foreach (var item in filterGuestByArea)
                            {
                                Console.WriteLine(item.Key);
                                foreach (var temp in item)
                                    Console.WriteLine("הזמנות מספר: " + temp.numGuestRequest);
                            }
                            break;
                        case "9":
                            List<HostingUnit> units = bl.allHostingUnitsOfHost(host.HostKey);
                            List<Order> closeOrder = new List<Order>();

                            foreach (HostingUnit item in units)
                            {
                                foreach (Order temp in bl.filterStatusOrdersInUnit(item.numHostingUnitKey, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח))
                                    closeOrder.Add(temp);

                            }
                            if (closeOrder != null)
                            {
                                foreach (Order temp in closeOrder)
                                    Console.WriteLine(temp.ToString());
                            }
                            else
                                Console.WriteLine("אין הזמנות שנסגרו ביחידה זו.");
                            break;
                        case "10":
                            List<HostingUnit> units1 = bl.allHostingUnitsOfHost(host.HostKey);
                            List<Order> openOrder = new List<Order>();

                            foreach (HostingUnit item in units1)
                            {
                                foreach (Order temp in bl.filterStatusOrdersInUnit(item.numHostingUnitKey, Enums.StatusOrder.נשלח_מייל))
                                    openOrder.Add(temp);
                            }
                            if (openOrder != null)
                            {
                                foreach (Order temp in openOrder)
                                    Console.WriteLine(temp.ToString());
                            }
                            else
                                Console.WriteLine("אין הזמנות פתוחות ליחידה זו.");
                            break;
                        case "0":
                            break;

                    }
                }
                catch (ConstraintException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DuplicateNameException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ReadOnlyException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (choice1 != "0");
        }

        public static void caseUserManager(IBL bl)// מסך בעל האתר- כל האפשרויות שמעניינות את בעל האתר
        {
            //IBL bl = BL.FactoryBL.GetBl();
            string choice = "y";
            do
            {
                try
                {
                    Console.WriteLine(@"for get the guest requests- press 1
for get the hosts- press 2
for get the hosting unit- press 3
for get the orders- press 4
for get the banks- press 5
for get the host with collection clearance- press 6
for get the number of order that sent to clients- press 7
for get the guest requests by parameter- press 8
for get the guest requests by area- press 9
for get the close order- press 10
for get the hosting unit by their host- 11
for exit- press 0
");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            List<GuestRequest> guestRequests = bl.allGuestRequest();
                            if (guestRequests != null)
                                foreach (GuestRequest temp in guestRequests)
                                    Console.WriteLine(temp.ToString());
                            else
                                Console.WriteLine("אין בקשות אירוח");
                            break;
                        case "2":
                            List<Host> hosts = bl.allHosts();
                            if (hosts != null)
                                foreach (Host temp in hosts)
                                    Console.WriteLine(temp.ToString());
                            else
                                Console.WriteLine("אין מארחים");
                            break;
                        case "3":
                            List<HostingUnit> hostingUnits = bl.allHostingUnits();
                            if (hostingUnits != null)
                                foreach (HostingUnit temp in hostingUnits)
                                    Console.WriteLine(temp.ToString());
                            else
                                Console.WriteLine("אין יחידות אירוח");
                            break;
                        case "4":
                            List<Order> orders = bl.allOrders();
                            if (orders != null)
                                foreach (Order temp in orders)
                                    Console.WriteLine(temp.ToString());
                            else
                                Console.WriteLine("אין הזמנות");
                            break;
                        case "5":
                            List<BankAccount> bankAccounts = bl.allBanks();
                            if (bankAccounts != null)
                                foreach (BankAccount temp in bankAccounts)
                                    Console.WriteLine(temp.ToString());
                            else
                                Console.WriteLine("אין בנקים בשימוש");
                            break;
                        case "6":
                            List<Host> hosts1 = bl.filterByCollectionClearance();
                            if (hosts1 != null)
                            {
                                foreach (Host item in hosts1)
                                    Console.WriteLine(item.ToString());
                                Console.WriteLine("");
                            }
                            else
                                Console.WriteLine("אין מארחים עם אישור גבייה.");
                            break;
                        case "7":
                            List<GuestRequest> lst = bl.allGuestRequest();
                            if (lst != null)
                            {
                                foreach (GuestRequest item in lst)
                                    Console.WriteLine("for " + item.ToString() + " sent " + bl.numOfOrderToClient(item) + " orders.");
                                Console.WriteLine("");
                            }
                            else
                                Console.WriteLine("אין בקשות אירוח.");
                            break;
                        case "8":
                            List<GuestRequest> lst1 = bl.filterGuestByParameter(x => x.Breakfast == Enums.Option.הכרחי);
                            if (lst1 != null)
                            {
                                foreach (GuestRequest item in lst1)
                                    Console.WriteLine(item.ToString());
                                Console.WriteLine("");
                            }
                            else
                                Console.WriteLine("אין בקשות העונות על דרישה זו.");
                            break;
                        case "9":
                            var filterGuestByArea = bl.filterGuestByArea();
                            foreach (var item in filterGuestByArea)
                            {
                                Console.WriteLine(item.Key);
                                foreach (var temp in item)
                                    Console.WriteLine("הזמנות מספר: " + temp.numGuestRequest);
                            }

                            break;
                        case "10":

                            //List<GuestRequest> closeOrder = bl.filterApprovedRequest();
                            //if (closeOrder != null)
                            //{
                            //    foreach (GuestRequest item in closeOrder)
                            //    {
                            //        Console.WriteLine(item.ToString());
                            //    }
                            //}
                            //else
                            //    Console.WriteLine("אין הזמנות שנסגרו.");
                            //break;
                        case "11":
                            List<Host> host = bl.allHosts();
                            foreach (Host item in host)
                            {
                                Console.WriteLine(item.ToString());
                                List<HostingUnit> allHostingUnitsOfHost = bl.allHostingUnitsOfHost(item.HostKey);
                                foreach (HostingUnit temp in allHostingUnitsOfHost)
                                    Console.WriteLine(temp.ToString());
                            }
                            break;
                        case "0":
                            break;

                        default:
                            break;
                    }

                }
                catch (ConstraintException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DuplicateNameException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ReadOnlyException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (choice != "0");
        }
    }


}







