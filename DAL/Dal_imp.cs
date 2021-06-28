using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BE;
using DS;

namespace DAL
{
    public class Dal_imp:Idal
    {
        List<HostingUnit> listOfHostingUnits = new List<HostingUnit>(DataSource.hostingUnits);
        List<GuestRequest> listOfGuestRequest = new List<GuestRequest>(DataSource.guestRequests);
        List<Order> listOfOrders = new List<Order>(DataSource.orders);

        #region request
        public int addRequest(GuestRequest guestRequest)
        {
            guestRequest.numGuestRequest = Configuration.GuestRequestKey++;
            guestRequest.Status = Enums.StatusClient.פתוחה;
            guestRequest.RegistrationDate = DateTime.Now;
            listOfGuestRequest.Add(guestRequest);
            return guestRequest.numGuestRequest;
        }

        public void UpdateRequest(int Key, Enums.StatusClient status)
        {
            List<GuestRequest> guestRequests = listOfGuestRequest;
            if (guestRequests != null)
            {
                var lst = from item in guestRequests
                          where item.numGuestRequest == Key
                          select item;
                if (lst != null)
                {
                    lst.FirstOrDefault(s => s.Status == status);
                }
                else
                    throw new KeyNotFoundException("The request is not exist");
            }
            else
                throw new KeyNotFoundException("The request is not exist");
        }

        public GuestRequest GetRequest (int key)
        {
            GuestRequest h= listOfGuestRequest.FirstOrDefault(ge => ge.numGuestRequest == key);
            if (h != null)
                return h;
            else
                throw new KeyNotFoundException("לא קיים במערכת מארח עם המספר שנשלח");
        }

        #endregion

        #region hostUnit
        public void addHostUnit(HostingUnit hostingUnit)
        {
            HostingUnit hosti = GetHostingUnit(hostingUnit.numHostingUnitKey);
            if (hosti == null)
            {
                hostingUnit.numHostingUnitKey = Configuration.HostingUnitKey++;
                listOfHostingUnits.Add(hostingUnit);
                hostingUnit.Owner.numOfUnit++;
            }
            else 
                throw new DuplicateNameException ("The hostingUnit is already exist");
        }
        public void removeHostUnit(HostingUnit hostingUnit)
        {
            List<HostingUnit> hostingUnits = listOfHostingUnits;
            var lst = from item in hostingUnits
                      where item.numHostingUnitKey == hostingUnit.numHostingUnitKey
                      select item;
            if (lst != null)
            {
                hostingUnit.Owner.numOfUnit--;
                listOfHostingUnits.Remove(hostingUnit);
            }
            else
                throw new KeyNotFoundException("The hostingUnit is not exist");
        }
        public void updateHostUnit(HostingUnit hostingUnit)
        {
            List<HostingUnit> hostingUnits = listOfHostingUnits;
            if (hostingUnit != null)
            {
                var lst = from item in hostingUnits
                          where item.numHostingUnitKey == hostingUnit.numHostingUnitKey
                          select item;
                if (lst != null)
                {
                    listOfHostingUnits.Remove(lst.First());
                    listOfHostingUnits.Add(hostingUnit);
                }
                else
                    throw new KeyNotFoundException("The hostingUnit is not exist");
            }
            else
                throw new KeyNotFoundException("The hostingUnit is not exist");
        }
        public HostingUnit GetHostingUnit(int key)
        {
            HostingUnit h= listOfHostingUnits.FirstOrDefault(ho => ho.numHostingUnitKey == key);
            if (h==null)
                throw new KeyNotFoundException("The hostingUnit is not exist");
            return h;

        }
        public HostingUnit GetHostingUnit(string name)
        {
            HostingUnit h = listOfHostingUnits.FirstOrDefault(ho => ho.HostingUnitName == name);
            if (h == null)
                throw new KeyNotFoundException("The hostingUnit is not exist");
            return h;

        }
        #endregion

        #region Order
        public int addOrder(Order order)
        {
            order.numOrderKey = Configuration.OrderKey++;
            order.Status = Enums.StatusOrder.טרם_טופל;
            order.CreateDate = DateTime.Now;

            listOfOrders.Add(order);
            return order.numOrderKey;
        }
        public void updateOrder(int Key, Enums.StatusOrder status)
        {
            List<Order> orders = listOfOrders;
            if (orders != null)
            {
                var lst = from item in orders
                          where item.numOrderKey == Key
                          select item;
                if (lst != null)
                {
                    lst.FirstOrDefault(s => s.Status == status);
                    if (status== Enums.StatusOrder.נשלח_מייל)
                    {
                        lst.FirstOrDefault(s => s.OrderDate == DateTime.Now);
                    }
                }
                else
                    throw new KeyNotFoundException("The order is not exist");
            }
            else
                throw new KeyNotFoundException("The order is not exist");
        }

        public Order GetOrder(int key)
        {
            Order h= listOfOrders.FirstOrDefault(or => or.numOrderKey == key);
            if (h != null)
                return h;
            else
                throw new KeyNotFoundException("לא קיימת במערכת הזמנה עם המספר שנשלח");
        }

        #endregion

        public int addHost(Host host)
        { 
            return 0;
        }
        public int GetHostKey()
        {
            return 0;
        }

        #region getLists
        public List<HostingUnit> allHostingUnits()
        {
            return getList(listOfHostingUnits);
        }
        public List<GuestRequest> allGuestRequest()
        {
            return getList(listOfGuestRequest);
        }
        public List<Order> allOrders()
        {
            return getList(listOfOrders);
        }
        public List<BankAccount> allBanks()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>()
            {
               new BankAccount()
                    {
                        BankNumber=11,
                        BankName="דיסקונט",
                        BranchNumber=123,
                        BranchAddress="כנפי נשרים 3",
                        BranchCity="קצרין",

                    },
               new BankAccount()
                    {
                        BankNumber=12,
                        BankName="בנק הפועלים",
                        BranchNumber=234,
                        BranchAddress="שדרות רגר 3",
                        BranchCity="באר שבע",

                    },
               new BankAccount()
                    {
                        BankNumber=13,
                        BankName="בנק לאומי",
                        BranchNumber=456,
                        BranchAddress="שדרות הרצל 6",
                        BranchCity="תל אביב",

                    },
               new BankAccount()
                    {
                        BankNumber=14,
                        BankName="בנק יהב",
                        BranchNumber=135,
                        BranchAddress="יפו 50",
                        BranchCity="ירושלים",

                    },
               new BankAccount()
                    {
                        BankNumber=15,
                        BankName="מזרחי טפחות",
                        BranchNumber=364,
                        BranchAddress="רוטשילד 8",
                        BranchCity="חיפה",

                    }
            };
            return getList(bankAccounts);
        }
        public List<Host> allHosts()
        {
            List<HostingUnit> lst = allHostingUnits();
            List<Host> result = new List<Host>();
            foreach (var item in lst)
            {
                if (!(result.Contains(item.Owner)))
                    result.Add(item.Owner);
            }

            return getList(result);
        }
        public static List<T> getList<T>(List<T> ts )
        {
            T[] arrey = new T[ts.Count];
            ts.CopyTo(arrey);
            return arrey.ToList();
        }

        #endregion

        #region deepCopyToBL
        public Order getBLOrder(int key)
        {
            Order ordi = GetOrder(key);
            Order temp = new Order()
            {
                numGuestRequest = ordi.numGuestRequest,
                numHostingUnitKey = ordi.numHostingUnitKey,
                numOrderKey = ordi.numOrderKey,
                Status = ordi.Status,
                CreateDate = ordi.CreateDate,
                OrderDate = ordi.OrderDate
            };
            return temp;
        }
        public HostingUnit getBLHostingUnit(int key)
        {
            try
            {
                HostingUnit hosting = GetHostingUnit(key);
                HostingUnit temp = new HostingUnit()
                {
                    numHostingUnitKey = hosting.numHostingUnitKey,
                    Owner = new BE.Host()
                    {
                        HostKey = hosting.Owner.HostKey,
                        PrivateName = hosting.Owner.PrivateName,
                        FamilyName = hosting.Owner.FamilyName,
                        FhoneNumber = hosting.Owner.FhoneNumber,
                        MailAddress = hosting.Owner.MailAddress,
                        BankAccuontHost = new BE.BankAccount()
                        {
                            BankNumber = hosting.Owner.BankAccuontHost.BankNumber,
                            BankName = hosting.Owner.BankAccuontHost.BankName,
                            BranchNumber = hosting.Owner.BankAccuontHost.BranchNumber,
                            BranchAddress = hosting.Owner.BankAccuontHost.BranchAddress,
                            BranchCity = hosting.Owner.BankAccuontHost.BranchCity,

                        },
                        CollectionClearance = hosting.Owner.CollectionClearance,
                        numOfUnit = hosting.Owner.numOfUnit
                    },
                    HostingUnitName = hosting.HostingUnitName,
                    diary = setDiary(hosting.diary),
                    Area = hosting.Area,
                    SubArea = hosting.SubArea,
                    Address = hosting.Address,
                    price = hosting.price,
                    Pool = hosting.Pool,
                    Jacuzzi = hosting.Jacuzzi,
                    Garden = hosting.Garden,
                    ChildrensAttractions = hosting.ChildrensAttractions,
                    Breakfast = hosting.Breakfast,
                    Capacity = hosting.Capacity,
                    TypeOfHost = hosting.TypeOfHost
                };
                return temp;
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
                HostingUnit hosting = GetHostingUnit(name);
                HostingUnit temp = new HostingUnit()
                {
                    numHostingUnitKey = hosting.numHostingUnitKey,
                    Owner = new BE.Host()
                    {
                        HostKey = hosting.Owner.HostKey,
                        PrivateName = hosting.Owner.PrivateName,
                        FamilyName = hosting.Owner.FamilyName,
                        FhoneNumber = hosting.Owner.FhoneNumber,
                        MailAddress = hosting.Owner.MailAddress,
                        BankAccuontHost = new BE.BankAccount()
                        {
                            BankNumber = hosting.Owner.BankAccuontHost.BankNumber,
                            BankName = hosting.Owner.BankAccuontHost.BankName,
                            BranchNumber = hosting.Owner.BankAccuontHost.BranchNumber,
                            BranchAddress = hosting.Owner.BankAccuontHost.BranchAddress,
                            BranchCity = hosting.Owner.BankAccuontHost.BranchCity,

                        },
                        CollectionClearance = hosting.Owner.CollectionClearance,
                        numOfUnit = hosting.Owner.numOfUnit
                    },
                    HostingUnitName = hosting.HostingUnitName,
                    diary = setDiary(hosting.diary),
                    Area = hosting.Area,
                    SubArea = hosting.SubArea,
                    Address = hosting.Address,
                    price = hosting.price,
                    Pool = hosting.Pool,
                    Jacuzzi = hosting.Jacuzzi,
                    Garden = hosting.Garden,
                    ChildrensAttractions = hosting.ChildrensAttractions,
                    Breakfast = hosting.Breakfast,
                    Capacity = hosting.Capacity,
                    TypeOfHost = hosting.TypeOfHost
                };

                return temp;
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        public bool[,] setDiary(bool[,] host)
        {
            bool[,] diary = new bool[12, 31];
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 31; j++)
                    diary[i, j] = host[i, j];
            return diary;
        }
        public GuestRequest getBLGuestRequest(int key)
        {
            GuestRequest gust = GetRequest(key);
            GuestRequest temp = new GuestRequest()
            {
                numGuestRequest = gust.numGuestRequest,
                PrivateName = gust.PrivateName,
                FamilyName = gust.FamilyName,
                MailAddress = gust.MailAddress,
                Status = gust.Status,
                RegistrationDate = new DateTime(gust.RegistrationDate.Year, gust.RegistrationDate.Month, gust.RegistrationDate.Day),
                EntryDate = new DateTime(gust.EntryDate.Year, gust.EntryDate.Month, gust.EntryDate.Day),
                ReleaseDate = new DateTime(gust.ReleaseDate.Year, gust.ReleaseDate.Month, gust.ReleaseDate.Day),
                Area = gust.Area,
                SubArea=gust.SubArea,
                Type = gust.Type,
                Adults = gust.Adults,
                Price=gust.Price,
                Children = gust.Children,
                Pool = gust.Pool,
                Jacuzzi = gust.Jacuzzi,
                Garden = gust.Garden,
                ChildrensAttractions = gust.ChildrensAttractions,
                Breakfast = gust.Breakfast
            };
            return temp;
        }//
        public Host GetHost(int key)
        {
            try
            {
                Host h = allHosts().FirstOrDefault(or => or.HostKey == key);
                if (h != null)
                    return h;
                else
                    throw new KeyNotFoundException("לא קיים במערכת מארח עם המספר שנשלח");
            }
            catch(KeyNotFoundException ex)
            {
                throw ex;
            }
        }//
        public Host getBLHost(int key)
        {
            try
            {
                Host host = GetHost(key);
                Host temp = new Host()
                {
                    HostKey = host.HostKey,
                    PrivateName = host.PrivateName,
                    FamilyName = host.FamilyName,
                    FhoneNumber = host.FhoneNumber,
                    MailAddress = host.MailAddress,
                    BankAccuontHost = new BE.BankAccount()
                    {
                        BankNumber = host.BankAccuontHost.BankNumber,
                        BankName = host.BankAccuontHost.BankName,
                        BranchNumber = host.BankAccuontHost.BranchNumber,
                        BranchAddress = host.BankAccuontHost.BranchAddress,
                        BranchCity = host.BankAccuontHost.BranchCity,

                    },
                    CollectionClearance = host.CollectionClearance,
                    numOfUnit = host.numOfUnit
                };
                return temp;
            }
            catch(KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        #endregion

        public int configurationData(string item)
        {
            return Configuration.fee;
        }
    }
}


