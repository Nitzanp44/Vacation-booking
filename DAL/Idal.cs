using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface Idal
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
        #region deep copy toBL
        Order getBLOrder(int key);
        HostingUnit getBLHostingUnit(int key);
        HostingUnit getBLHostingUnit(string name);
        GuestRequest getBLGuestRequest(int key);
        Host getBLHost(int key);
        #endregion
        int configurationData(string item);
    }
}
