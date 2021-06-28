using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        int addRequest(GuestRequest guestRequest);
        void UpdateRequest(int Key, Enums.StatusClient status);
        void addHostUnit(HostingUnit hostingUnit);
        void removeHostUnit(HostingUnit hostingUnit);
        void updateHostUnit(HostingUnit hostingUnit);
        int addOrder(Order order);
        void updateOrder(int Key, Enums.StatusOrder status);
        int addHost(Host host);
        int GetHostKey();
        List<HostingUnit> allHostingUnits();
        List<GuestRequest> allGuestRequest();
        List<Order> allOrders();
        List<BankAccount> allBanks();
        List<Host> allHosts();
        Order getBLOrder(int key);
        HostingUnit getBLHostingUnit(int key);
        HostingUnit getBLHostingUnit(string name);
        GuestRequest getBLGuestRequest(int key);
        Host getBLHost(int key);
        List<GuestRequest> filterStatusRequest(Enums.StatusClient status);//מחזיר את הבקשות לפי סטטוס
        List<Order> filterStatusOrdersInUnit(int key, Enums.StatusOrder status);//מחזיר את כל ההזמנות לפי סטטוס ביחידה מסויימת
        List<Order> filterStatusOrdersInUnit(int key);//מחזיר את כל ההזמנות לפי יחידה מסויימת
        List<Order> filterStatusOrdersOfRequest(int key, Enums.StatusOrder status);//מחזיר את כל ההזמנות לפי סטטוס לבקשה מסויימת
        List<Order> filterOrdersOfHost(int key);//מחזיר את כל ההזמנות של מארח מסויים
        List<Host> filterByCollectionClearance();//מחזיר את כל המארחים שיש להם אישור לגביית עמלה
        List<HostingUnit> freeHostUnitInDates(DateTime date, int duration, List<HostingUnit> units);//פונקציה שמקבלת תאריך ומספר ימי נופש ומחזירה את רשימת כל יחידות האירוח הפנויות
        int daysBetween(DateTime start, DateTime end);//פונקציה שמקבלת תאריך אחד או שניים. הפונקציה מחזירה את מספר הימים שעברו מהתאריך הראשון ועד לשני
        int daysBetween(DateTime start);//   פונקציה שמקבלת תאריך אחד או שניים. הפונקציה מחזירה את מספר הימים שעברומהתאריך ראשון ועד היום
        List<Order> orderAfterTime(int days);//פונקציה שמקבלת מספר ימים, ומחזירה את כל ההזמנות שמשך הזמן שעבר מאז שנוצרו
        List<HostingUnit> filterHostingByParameter(Predicate<HostingUnit> match = null);//מסנן יחידות אירוח לפי פרמטר -יכול להשתנות
        List<GuestRequest> filterGuestByParameter(Predicate<GuestRequest> match);//מסנן בקשות אירוח לפי פרמטר -יכול להשתנות
        int numOfOrderToClient(GuestRequest guestRequest);//פונקציה שמקבלת דרישת לקוח, ומחזירה את מספר ההזמנות שנשלחו אליו.
        int nunOfOrderToHosting(HostingUnit hostingUnit);//פונקציה שמקבלת יחידת אירוח ומחזירה את מספר ההזמנות שנסגרו בהצלחה/נשלחו עבור יחידה זו דרך האתר
        IEnumerable<IGrouping<Enums.Area, GuestRequest>> filterGuestByArea();//רשימת דרישות לקוח מקובצת ע"פ אזור הנופש הנדרש
        List<IGrouping<int, GuestRequest>> filterGuestByPeople();//רשימת דרישות לקוח מקובצת ע"פ מספר הנופשים.
        List<IGrouping<int, Host>> filterHostByHostingunit();//רשימת מארחים מקובצת ע"פ מספר יחידות האירוח שהם מחזיקים
        IEnumerable<IGrouping<Enums.Area, HostingUnit>> filterHostingunitByArea(List<HostingUnit> lst);//רשימת יחידות אירוח מקובצת ע"פ אזור הנופש הנדרש.
        List<HostingUnit> allHostingUnitsOfHost(int hostKey);//מקבל מספר מארח ומחזיר את רשימת יחידות האירוח שלו
        List<Order> filterOrderByStatus(Enums.StatusOrder statusOrder);//מסנן הזמנות לפי סטטוס
        List<HostingUnit> filterHostingByHost(List<HostingUnit> lst, int key);//מסנן יחידות אירוח לפי מארח
        List<HostingUnit> filterHostingByArea(List<HostingUnit> lst, Enums.Area area);//מסנן יחידות אירוח לפי אזור
        int configurationData(string item);
    }
}
