using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Threading;
using BE;
using DAL;

namespace BL
{
    public class BL_Class:IBL
    {
        Idal dal = DAL.FactoryDAL.GetDal();


        public BL_Class()
        {
            Thread thread = new Thread(closeOrders);
            thread.Start();
        }
        public int addRequest(GuestRequest guestRequest)
        {
            try
            {
                if (guestRequest.EntryDate < guestRequest.ReleaseDate)

                {

                    int num=dal.addRequest(guestRequest);
                    return num;
                }
                else
                {
                    throw new FormatException("התאריך לא חוקי"+guestRequest.EntryDate+" gs "+guestRequest.ReleaseDate);
                }
            }
            catch (FormatException s)
            {
                throw s;
            }
        }
        public void UpdateRequest(int Key, Enums.StatusClient status)
        {
            dal.UpdateRequest(Key, status);
        }
        public void addHostUnit(HostingUnit hostingUnit)
        {
            if (hostingUnit.Capacity < 2)
                throw new ConstraintException("לא ניתן לפתוח יחידה שלא מתאימה לפחות לשני אנשים");
            try
            {
                dal.addHostUnit(hostingUnit);
            }
            catch (DuplicateNameException ex)
            {
                throw ex;
            }
        }
        public void removeHostUnit(HostingUnit hostingUnit)
        {
            List<Order> orders = dal.allOrders();
            var lst = from item in orders
                      let num=item.numHostingUnitKey
                      where num==hostingUnit.numHostingUnitKey && item.Status==Enums.StatusOrder.נשלח_מייל
                      select item;
            if (lst.Count()!=0)
                throw new FormatException("לא ניתן למחוק יחידת אירוח כשיש הזמנה פתוחה");
            else
                try
                {
                    dal.removeHostUnit(hostingUnit);
                }

                catch (KeyNotFoundException ex)
                {
                    throw ex;
                }
                catch (FormatException ex)
                {
                    throw ex;
                }

        }
        public void updateHostUnit(HostingUnit hostingUnit)
        {
            HostingUnit temp = dal.getBLHostingUnit(hostingUnit.numHostingUnitKey);
            List<Order> orders = dal.allOrders();
            var lst = from item in orders
                      let num = item.numHostingUnitKey
                      where num == hostingUnit.numHostingUnitKey && item.Status == Enums.StatusOrder.נשלח_מייל
                      select item;
            if (lst != null)
                if (temp.Owner.CollectionClearance == true && hostingUnit.Owner.CollectionClearance == false)
                    throw new ReadOnlyException("לא ניתן לשנות הרשאה בזמן שיש הזמנה פתוחה");
            try
            {
                dal.updateHostUnit(hostingUnit);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        public int addOrder(Order order)
        {
            HostingUnit hostingUnit = dal.getBLHostingUnit(order.numHostingUnitKey);
            GuestRequest guestRequest = dal.getBLGuestRequest(order.numGuestRequest);
            if (!ApproveRequest(hostingUnit.diary, guestRequest.EntryDate, guestRequest.ReleaseDate))
                throw new DuplicateWaitObjectException("התאריכים המבוקשים אינם פנויים");
            if (hostingUnit.Owner.CollectionClearance == false)
                throw new ConstraintException("לא ניתן לפתוח הזמנה ללא הרשאת חיוב");
            if (hostingUnit.Area != guestRequest.Area && hostingUnit.SubArea != guestRequest.SubArea)
                throw new ConstraintException("לא ניתן לפתוח הזמנה שלא באזורך");
            if ((hostingUnit.price > 100 && guestRequest.Price < 100) || (hostingUnit.price > 500 && guestRequest.Price < 500) || (hostingUnit.price > 1000 && guestRequest.Price < 1000))
                throw new ConstraintException("לא ניתן לפתוח הזמנה שלא עומדת בתקציב הלקוח");
            if (hostingUnit.Breakfast == false && guestRequest.Breakfast == Enums.Option.הכרחי || hostingUnit.Pool == false && guestRequest.Breakfast == Enums.Option.הכרחי || hostingUnit.Breakfast == false && guestRequest.Jacuzzi == Enums.Option.הכרחי || hostingUnit.Breakfast == false && guestRequest.ChildrensAttractions == Enums.Option.הכרחי || hostingUnit.Breakfast == false && guestRequest.Garden == Enums.Option.הכרחי || hostingUnit.TypeOfHost != guestRequest.Type)
                throw new ConstraintException("לא ניתן לפתוח הזמנה שלא עומדת בתנאי הלקוח");
            if (hostingUnit.Capacity < (guestRequest.Adults + guestRequest.Children))
                throw new ConstraintException("אין מספיק מקום ביחידת האירוח");

            int num = dal.addOrder(order);
            return num;
        }
        public void updateOrder(int Key, Enums.StatusOrder status)
        {
            Order ordi = dal.getBLOrder(Key);
            if (ordi.Status == Enums.StatusOrder.נסגר_בהיענות_של_הלקוח)
            {
                throw new ReadOnlyException("לא ניתן לשנות סטטוס הזמנה שנסגרה");
            }
            else
            {
                try
                {
                    dal.updateOrder(ordi.numOrderKey, ordi.Status);
                    if (status == Enums.StatusOrder.נסגר_בהיענות_של_הלקוח)
                    {
                        HostingUnit temp = dal.getBLHostingUnit(ordi.numHostingUnitKey);
                        GuestRequest help = dal.getBLGuestRequest(ordi.numGuestRequest);
                        if (!ApproveRequest(temp.diary, help.EntryDate, help.ReleaseDate))
                            throw new DuplicateWaitObjectException("התאריכים המבוקשים אינם פנויים");
                        else
                        {
                            temp.diary = fillData(temp.diary, help.EntryDate, help.ReleaseDate);
                            dal.updateHostUnit(temp);
                            dal.UpdateRequest(ordi.numGuestRequest, Enums.StatusClient.נסגרה_עסקה_דרך_האתר);
                            List<Order> orders = dal.allOrders();
                            var lst = from item in orders
                                      where item.numGuestRequest == ordi.numGuestRequest
                                      select item;
                            foreach (var items in lst)
                                items.Status = Enums.StatusOrder.נסגר_בהיענות_של_הלקוח;
                        }
                    }
                    if (status == Enums.StatusOrder.נשלח_מייל)
                    {

                        Console.WriteLine("נשלח מייל ללקוח");
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    throw ex;
                }
                catch (DuplicateWaitObjectException ex1)
                {
                    throw ex1;
                }
            }
        }
        public int addHost(Host host)
        {
            return dal.addHost(host);
        }

        public int GetHostKey()
        {
            try
            {
                return dal.GetHostKey();
            }
            catch (FileLoadException ex)
            {
                throw ex;
            }
        }
            public List<HostingUnit> allHostingUnits()
        {
            return dal.allHostingUnits();
        }
        public List<GuestRequest> allGuestRequest()
        {
            return dal.allGuestRequest();
        }
        public List<Order> allOrders()
        {
            return dal.allOrders();
        }
        public List<BankAccount> allBanks()
        {
            return dal.allBanks();
        }
        public List<Host> allHosts()
        {
            return dal.allHosts();
        }
        public Order getBLOrder(int key)
        {
            return dal.getBLOrder(key);
        }
        public HostingUnit getBLHostingUnit(int key)
        {
            try
            {
                return dal.getBLHostingUnit(key);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }

        }
        public HostingUnit getBLHostingUnit(string name)
        {
            try
            {
                return dal.getBLHostingUnit(name);
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }

        }
        public GuestRequest getBLGuestRequest(int key)
        {
            return dal.getBLGuestRequest(key);
        }
        public Host getBLHost(int key)
        {
            try
            {
               Host h= dal.getBLHost(key);
                return h;
            }
            catch(KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        public List<GuestRequest> filterStatusRequest(Enums.StatusClient status)
        {
            var lst = from item in allGuestRequest()
                      where item.Status == status
                      select item;
            return lst.ToList();
        }
        public List<Order> filterStatusOrdersInUnit(int key, Enums.StatusOrder status)
        {
            var lst = from item in allOrders()
                      let num = item.numHostingUnitKey
                      where num == key && item.Status == status
                      select item;
            return lst.ToList();
        }
        public List<Order> filterStatusOrdersInUnit(int key)
        {
            var lst = from item in allOrders()          
                      where item.numHostingUnitKey == key
                      select item;
            return lst.ToList();
        }
        public List<Order> filterStatusOrdersOfRequest(int key, Enums.StatusOrder status)
        {
            var lst = from item in allOrders()
                      let num = item.numGuestRequest
                      where num == key && item.Status == status
                      select item;
            return lst.ToList();
        }
        public List<Order> filterOrdersOfHost(int key)
        {
            List<Order> orders = new List<Order>();
            foreach (var temp in allHostingUnitsOfHost(key))
            {
                var lst = from item in allOrders()
                          where item.numHostingUnitKey == temp.numHostingUnitKey
                          select item;
                orders.AddRange(lst.ToList());
            }
            return orders;
        }
        public List<Host> filterByCollectionClearance()
        {
            var lst = from item in allHostingUnits()
                      let ok = item.Owner.CollectionClearance
                      where ok == true
                      select item.Owner;
            return lst.ToList();
        }
        public List<HostingUnit> freeHostUnitInDates(DateTime date, int duration, List<HostingUnit> units)//פונקציה שמקבלת תאריך ומספר ימי נופש ומחזירה את רשימת כל יחידות האירוח הפנויות
        {
            DateTime dateEnd = new DateTime(date.Year, date.Month, date.Day);
            dateEnd.AddDays(duration);
            List<HostingUnit> lst = units.FindAll(host => ApproveRequest(host.diary, date, dateEnd) == true);
            return lst;
        }
        public int daysBetween(DateTime start, DateTime end)//פונקציה שמקבלת תאריך אחד או שניים. הפונקציה מחזירה את מספר הימים שעברו מהתאריך הראשון ועד לשני
        {
            TimeSpan date = end - start;
            return date.Days;
        }
        public int daysBetween(DateTime start)//   פונקציה שמקבלת תאריך אחד או שניים. הפונקציה מחזירה את מספר הימים שעברומהתאריך ראשון ועד היום
        {
            TimeSpan time = DateTime.Now - start;
            return time.Days;
        }
        public List<Order> orderAfterTime(int days)//פונקציה שמקבלת מספר ימים, ומחזירה את כל ההזמנות שמשך הזמן שעבר מאז שנוצרו גדול או שווה למספר שהתקבל 
        {
            List<Order> orders = allOrders();
            var result = from itam in orders
                         where ((DateTime.Now - itam.CreateDate).Days > days && itam.Status == Enums.StatusOrder.טרם_טופל) || ((DateTime.Now - itam.OrderDate).Days > days && itam.Status == Enums.StatusOrder.נשלח_מייל)
                         select itam;
            return result.ToList();

        }
        public List<GuestRequest> filterGuestByParameter(Predicate<GuestRequest> match = null)//מסנן בקשות אירוח לפי פרמטר -יכול להשתנות
        {
            List<GuestRequest> lst = allGuestRequest();
            List<GuestRequest> result = new List<GuestRequest>();
            result = lst.FindAll(match);
            return result;
        }
        public List<HostingUnit> filterHostingByParameter(Predicate<HostingUnit> match = null)//מסנן יחידות אירוח לפי פרמטר -יכול להשתנות
        {
            List<HostingUnit> lst = allHostingUnits();
            List<HostingUnit> result = new List<HostingUnit>();
            result = lst.FindAll(match);
            return result;
        }
        public int numOfOrderToClient(GuestRequest guestRequest)//פונקציה שמקבלת דרישת לקוח, ומחזירה את מספר ההזמנות שנשלחו אליו.
        {
            List<Order> lst = allOrders().FindAll(x => x.numGuestRequest == guestRequest.numGuestRequest);
            return lst.Count();
        }
        public int nunOfOrderToHosting(HostingUnit hostingUnit)//פונקציה שמקבלת יחידת אירוח ומחזירה את מספר ההזמנות שנסגרו בהצלחה/נשלחו עבור יחידה זו דרך האתר
        {
            int num = filterStatusOrdersInUnit(hostingUnit.numHostingUnitKey, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח).Count() + filterStatusOrdersInUnit(hostingUnit.numHostingUnitKey, Enums.StatusOrder.נשלח_מייל).Count();
            return num;
        }
        public IEnumerable<IGrouping<Enums.Area, GuestRequest>> filterGuestByArea()//רשימת דרישות לקוח מקובצת ע"פ אזור הנופש הנדרש
        {
            List<GuestRequest> lst = allGuestRequest();
            IEnumerable<IGrouping<Enums.Area, GuestRequest>> result = from item in lst
                                                                      group item by item.Area;
            return result;
        }
        public List<IGrouping<int, GuestRequest>> filterGuestByPeople()//רשימת דרישות לקוח מקובצת ע"פ מספר הנופשים.
        {
            List<GuestRequest> lst = allGuestRequest();
            IEnumerable<IGrouping<int, GuestRequest>> result = from item in lst
                                                               group item by item.Adults + item.Children;
            return result.ToList();
        }
        public List<IGrouping<int, Host>> filterHostByHostingunit()//רשימת מארחים מקובצת ע"פ מספר יחידות האירוח שהם מחזיקים
        {
            List<Host> lst = allHosts();
            IEnumerable<IGrouping<int, Host>> fianal = from item2 in lst
                                                       group item2 by item2.numOfUnit;
            return fianal.ToList();
        }
        public IEnumerable<IGrouping<Enums.Area, HostingUnit>> filterHostingunitByArea(List<HostingUnit> lst)//רשימת יחידות אירוח מקובצת ע"פ אזור הנופש הנדרש
        {
            IEnumerable<IGrouping<Enums.Area, HostingUnit>> result = from item in lst
                                                                      group item by item.Area;
            return result;
        }
        public List<HostingUnit> allHostingUnitsOfHost(int hostKey)//מקבל מספר מארח ומחזיר את רשימת יחידות האירוח שלו
        {
            List<HostingUnit> lst = allHostingUnits();
            var result = from item in lst
                         where item.Owner.HostKey == hostKey
                         select item;
            return result.ToList();
        }
        public List<Order> filterOrderByStatus(Enums.StatusOrder statusOrder)//מסנן הזמנות לפי סטטוס
        {
            List<Order> orders = allOrders();
            var result = from item in orders
                         where item.Status == statusOrder
                         select item;
            return result.ToList();
        }
        public List<HostingUnit> filterHostingByArea (List<HostingUnit> lst, Enums.Area area)//מסנן יחידות אירוח לפי אזור
        {
            var result = from item in lst
                         where item.Area == area
                         select item;
            return result.ToList();
        }
        public List<HostingUnit> filterHostingByHost(List<HostingUnit> lst, int key)//מסנן יחידות אירוח לפי מארח
        {
            var result = from item in lst
                         where item.Owner.HostKey == key
                         select item;
            return result.ToList();
        }
        public bool ApproveRequest(bool[,] Diary, DateTime EntryDate1, DateTime ReleaseDate1)//הוספת חופשה ללוח שנה אם התאריכים פנויים
        {
            int day = EntryDate1.Day;
            int month = EntryDate1.Month;
            TimeSpan t = ReleaseDate1 - EntryDate1;
            int duration = t.Days;
            int tempDay = day - 1;
            int tempMonth = month - 1;
            for (int i = 0; i < duration - 1; i++)                                            //בודק אם התאריכים שבין הסיום וההתחלה פנויים
            {
                if (tempDay > 30)
                {
                    tempMonth++;
                    tempDay = 1;
                }
                if (tempMonth > 11)
                {
                    tempMonth = 1;
                    tempDay = 1;
                }
                if (Diary[tempMonth, tempDay++])
                {
                    return false;
                }
            }
            return true;
        }
        public bool[,] fillData(bool[,] Diary, DateTime EntryDate1, DateTime ReleaseDate1)
        {
            int day = EntryDate1.Day;
            int month = EntryDate1.Month;
            TimeSpan t = ReleaseDate1 - EntryDate1;
            int duration = t.Days;
            int tempDay = day - 1;
            int tempMonth = month - 1;
            for (int j = 0; j < duration - 1; j++)                                              //מוסיף ללוח שנה
            {
                if (tempDay > 30)
                {
                    tempMonth++;
                    tempDay = 1;
                }
                if (tempMonth > 11)
                {
                    tempMonth = 1;
                    tempDay = 1;
                }
                Diary[tempMonth, tempDay++] = true;
            }
            return Diary;
        }

        public int configurationData(string item)
        {
            return dal.configurationData(item);
        }

        private void closeOrders()
        {
            while (true)
            {
                foreach (var item in orderAfterTime(configurationData("DayForRequest")))
                {
                    updateOrder(item.numOrderKey, Enums.StatusOrder.נסגר_מחוסר_היענות);
                    UpdateRequest(item.numGuestRequest, Enums.StatusClient.נסגרה_כי_פג_תוקפה);
                }
                Thread.Sleep(86400000);
            }
        }


        //public string[] allHostNameAndPhone()
        //{
        //    List<Host> lst = allHosts();
        //    var result=from item in lst
        //               select new { Name = item.PrivateName/*, Phone=item.FhoneNumber*/ };
        //    return result;
        //}

        //select new { Name = item.Owner.PrivateName };
    }
}
