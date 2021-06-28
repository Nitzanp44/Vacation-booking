using BE;
using DS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Net;


namespace DAL
{
    public class Dal_XML_imp : Idal
    {
        List<HostingUnit> listOfHostingUnits = new List<HostingUnit>(DataSource.hostingUnits);
        List<GuestRequest> listOfGuestRequest = new List<GuestRequest>(DataSource.guestRequests);
        List<Order> listOfOrders = new List<Order>(DataSource.orders);
        List<BankAccount> bankAccounts = new List<BankAccount>();
      
        
        XElement configurationXML;
        XElement orderXML;
        XElement bankXML;


        string GuestRequestPath = @"GuestRequestXML.xml";
        string HostingUnitPath = @"HostingUnitXML.xml";
        string OrderPath = @"OrderXML.xml";
        string configurationPath = @"configurationXML.xml";
        string BankPath = @"BankXML.xml";
        string HostPath = @"HostXML.xml";


        public Dal_XML_imp()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += Background_DoWork;
            backgroundWorker.RunWorkerCompleted += Background_RunWorkerCompleted;

            if (!File.Exists(configurationPath))
                CreateFilesconfigurationXML();
            else
                LoadDataconfigurationXML();

            if (!File.Exists(OrderPath))
            {
                CreateFilesorderXML();
                foreach (var item in listOfOrders)
                {
                    orderXML.Add(orderToXElement(item));
                    orderXML.Save(OrderPath);
                }
            }

            else
                LoadDataorderXML();

            if (!File.Exists(GuestRequestPath))
            {
                FileStream fileGuestRequest = new FileStream(GuestRequestPath, FileMode.Create);
                fileGuestRequest.Close();
                saveListToXML<GuestRequest>(listOfGuestRequest, GuestRequestPath);
            }
            else
                listOfGuestRequest = (loadListFromXML<GuestRequest>(GuestRequestPath));

            if (!File.Exists(HostingUnitPath))
            {
                FileStream fileHostingUnit = new FileStream(HostingUnitPath, FileMode.Create);
                FileStream fileHost = new FileStream(HostPath, FileMode.Create);
                List<Host> hosts = new List<Host>();
                foreach (var item in listOfHostingUnits)
                {
                    if (!(hosts.Contains(item.Owner)))
                        hosts.Add(item.Owner);
                }

                fileHost.Close();
                fileHostingUnit.Close();
                saveListToXML<HostingUnit>(listOfHostingUnits, HostingUnitPath);
                saveListToXML<Host>(hosts, HostPath);
            }
            else
                listOfHostingUnits = (loadListFromXML<HostingUnit>(HostingUnitPath));

            if (!File.Exists(BankPath))
            {
                CreateFilesBanksXML();
                if (backgroundWorker.IsBusy != true)
                    backgroundWorker.RunWorkerAsync();
            }
            else
            {
                LoadDatabanksXML();
            }

        }

        public static void saveListToXML<T>(List<T> list, string Path)
        {
            FileStream file = new FileStream(Path, FileMode.Create);
            XmlSerializer x = new XmlSerializer(list.GetType());
            x.Serialize(file, list);
            file.Close();
        }

        public static List<T> loadListFromXML<T>(string path)
        {
            if (File.Exists(path))
            {
                List<T> list;
                XmlSerializer x = new XmlSerializer(typeof(List<T>));
                FileStream file = new FileStream(path, FileMode.Open);
                list = (List<T>)x.Deserialize(file);
                file.Close();
                return list;
            }
            else return new List<T>();
        }

        private void CreateFilesconfigurationXML()
        {
            XElement GuestRequestKey = new XElement("GuestRequestKey", "100");
            XElement HostingUnitKey = new XElement("HostingUnitKey", "10000000");
            XElement HostKey = new XElement("HostKey", "10000000");
            XElement OrderKey = new XElement("OrderKey", "100");
            XElement fee = new XElement("fee", "10");
            XElement DayForRequest = new XElement("DayForRequest", "60");
            configurationXML = new XElement("configurationXML", GuestRequestKey, HostingUnitKey, HostKey, OrderKey, fee, DayForRequest);
            configurationXML.Save(configurationPath);
        }

        private void LoadDataconfigurationXML()
        {
            try
            {
                configurationXML = XElement.Load(configurationPath);
            }
            catch
            {
                throw new FileLoadException("הקובץ לא עלה כראוי");
            }
        }

        private void CreateFilesorderXML()
        {
            orderXML = new XElement("orders");
            orderXML.Save(OrderPath);
        }

        private void LoadDataorderXML()
        {
            try
            {
                orderXML = XElement.Load(OrderPath);
            }
            catch
            {
                throw new FileLoadException("הקובץ לא עלה כראוי");
            }
        }

        private void CreateFilesBanksXML()
        {
            bankXML = new XElement("banks");
            bankXML.Save(BankPath);
        }

        private void LoadDatabanksXML()
        {
            try
            {
                bankXML = XElement.Load(BankPath);
            }
            catch
            {
                throw new FileLoadException("הקובץ לא עלה כראוי");
            }
        }
        Order XElementToOrder(XElement toConvert)
        {
            Order order = new Order();

            order.numOrderKey = int.Parse(toConvert.Element("numOrderKey").Value);
            order.numHostingUnitKey = int.Parse(toConvert.Element("numHostingUnitKey").Value);
            order.numGuestRequest = int.Parse(toConvert.Element("numGuestRequest").Value);
            order.CreateDate = new DateTime(int.Parse(toConvert.Element("CreateDate").Element("creatYear").Value),
                                                       int.Parse(toConvert.Element("CreateDate").Element("creatMonth").Value),
                                                       int.Parse(toConvert.Element("CreateDate").Element("creatDay").Value));
            order.OrderDate = new DateTime(int.Parse(toConvert.Element("OrderDate").Element("orderYear").Value),
                                                   int.Parse(toConvert.Element("OrderDate").Element("orderMonth").Value),
                                                   int.Parse(toConvert.Element("OrderDate").Element("orderDay").Value));
            order.Status = (Enums.StatusOrder)Enum.Parse(typeof(Enums.StatusOrder), toConvert.Element("Status").Value);
            
            return order;

        }

        XElement orderToXElement(Order orderToConvert)
        {
            XElement numOrderKey = new XElement("numOrderKey", orderToConvert.numOrderKey);
            XElement numHostingUnitKey = new XElement("numHostingUnitKey", orderToConvert.numHostingUnitKey);
            XElement numGuestRequest = new XElement("numGuestRequest", orderToConvert.numGuestRequest);
            XElement creatYear = new XElement("creatYear", orderToConvert.CreateDate.Year);
            XElement creatMonth = new XElement("creatMonth", orderToConvert.CreateDate.Month);
            XElement creatDay = new XElement("creatDay", orderToConvert.CreateDate.Day);
            XElement CreateDate = new XElement("CreateDate", creatYear, creatMonth, creatDay);
            XElement orderYear = new XElement("orderYear", orderToConvert.OrderDate.Year);
            XElement orderMonth = new XElement("orderMonth", orderToConvert.OrderDate.Month);
            XElement orderDay = new XElement("orderDay", orderToConvert.OrderDate.Day);
            XElement OrderDate = new XElement("OrderDate", orderYear, orderMonth, orderDay);
            XElement Status = new XElement("Status", orderToConvert.Status);

            XElement order = new XElement("orders", numOrderKey, numHostingUnitKey, numGuestRequest, CreateDate, OrderDate, Status);
            return order;
        }
        BankAccount XElementToBankAccount(XElement toConvert)
        {
            BankAccount bank = new BankAccount();

            bank.BankNumber = int.Parse(toConvert.Element("Bank_Code").Value);
            bank.BankName = toConvert.Element("Bank_Name").Value;
            bank.BranchNumber = int.Parse(toConvert.Element("Branch_Code").Value);
            bank.BranchAddress = toConvert.Element("Branch_Address").Value;
            bank.BranchCity = toConvert.Element("City").Value;
           return bank;
        }
        #region request
        public int addRequest(GuestRequest guestRequest)
        {
            try
            {
                LoadDataconfigurationXML();
                int code = int.Parse(configurationXML.Element("GuestRequestKey").Value);
                guestRequest.numGuestRequest = code++;
                configurationXML.Element("GuestRequestKey").Value = code.ToString();
                configurationXML.Save(configurationPath);
                guestRequest.Status = Enums.StatusClient.פתוחה;
                guestRequest.RegistrationDate = DateTime.Now;
                listOfGuestRequest.Add(guestRequest);
                saveListToXML(listOfGuestRequest, GuestRequestPath);
                return guestRequest.numGuestRequest;
            }
            catch (FileLoadException ex)
            {
                throw ex;
            }
        }

        public void UpdateRequest(int Key, Enums.StatusClient status)
        {
            listOfGuestRequest = loadListFromXML<GuestRequest>(GuestRequestPath);
            if (listOfGuestRequest != null)
            {
                var lst = from item in listOfGuestRequest
                          where item.numGuestRequest == Key
                          select item;
                if (lst != null)
                {
                    lst.FirstOrDefault(s => s.Status == status);
                    saveListToXML<GuestRequest>(listOfGuestRequest, GuestRequestPath);
                }
                else
                    throw new KeyNotFoundException("The request is not exist");
            }
            else
                throw new KeyNotFoundException("The request is not exist");
        }

        public GuestRequest GetRequest(int key)
        {

            GuestRequest h = loadListFromXML<GuestRequest>(GuestRequestPath).FirstOrDefault(ge => ge.numGuestRequest == key);
            if (h != null)
                return h;
            else
                throw new KeyNotFoundException("לא קיים במערכת בקשה עם המספר שנשלח");
        }

        #endregion

        #region hostUnit
        public void addHostUnit(HostingUnit hostingUnit)
        {
            List<Host> hosts = loadListFromXML<Host>(HostPath);
            HostingUnit hosti = GetHostingUnit(hostingUnit.numHostingUnitKey);
            if (hosti == null)
            {
                try
                {
                    LoadDataconfigurationXML();
                    int code = int.Parse(configurationXML.Element("HostingUnitKey").Value);
                    hostingUnit.numHostingUnitKey = code++;
                    configurationXML.Element("HostingUnitKey").Value = code.ToString();
                    configurationXML.Save(configurationPath);
                    listOfHostingUnits.Add(hostingUnit);
                    //++hostingUnit.Owner.numOfUnit;
                    if (hostingUnit.Owner.numOfUnit == 1)
                    {
                        hosts.Add(hostingUnit.Owner);
                        saveListToXML(hosts, HostPath);
                    }
                    else
                    {
                        if (hosts != null)
                        {
                            foreach (var item in hosts)
                            {
                                if (item.HostKey == hostingUnit.Owner.HostKey)
                                {
                                    item.numOfUnit++;
                                }
                            }
                        }
                        else
                            throw new KeyNotFoundException("The hostingUnit is not exist");
                    }
                    saveListToXML<HostingUnit>(listOfHostingUnits, HostingUnitPath);
                    saveListToXML<Host>(hosts, HostPath);
                }
                catch (FileLoadException ex)
                {
                    throw ex;
                }
            }
            else
                throw new DuplicateNameException("The hostingUnit is already exist");
        }
        public void removeHostUnit(HostingUnit hostingUnit)
        {
            List<Host> hosts = loadListFromXML<Host>(HostPath);
            listOfHostingUnits = loadListFromXML<HostingUnit>(HostingUnitPath);
            var lst = from item in listOfHostingUnits
                      where item.numHostingUnitKey == hostingUnit.numHostingUnitKey
                      select item;
            if (lst != null)
            {
                //hostingUnit.Owner.numOfUnit--;
                foreach (var item in hosts)
                {
                    if (item.HostKey == hostingUnit.Owner.HostKey)
                    {
                        item.numOfUnit--;
                    }
                }

                saveListToXML(hosts, HostPath);
                //if (hostingUnit.Owner.numOfUnit == 0)
                //{
                //    hosts.Remove(hostingUnit.Owner);
                //    saveListToXML(hosts, HostPath);
                //}
                //else
                //{
                //    if (hosts != null)
                //    {
                //        foreach (var item in hosts)
                //        {
                //            if (item.HostKey == hostingUnit.Owner.HostKey)
                //            {
                //                item.numOfUnit++;
                //            }
                //        }
                //    }
                //    else
                //        throw new KeyNotFoundException("The hostingUnit is not exist");
                //}
                listOfHostingUnits.Remove(hostingUnit);
                saveListToXML<HostingUnit>(listOfHostingUnits, HostingUnitPath);
            }
            else
                throw new KeyNotFoundException("The hostingUnit is not exist");
        }
        public void updateHostUnit(HostingUnit hostingUnit)
        {
            listOfHostingUnits = loadListFromXML<HostingUnit>(HostingUnitPath);
            if (listOfHostingUnits != null)
            {
                var lst = from item in listOfHostingUnits
                          where item.numHostingUnitKey == hostingUnit.numHostingUnitKey
                          select item;
                if (lst != null)
                {
                    listOfHostingUnits.Remove(lst.First());
                    listOfHostingUnits.Add(hostingUnit);
                    saveListToXML<HostingUnit>(listOfHostingUnits, HostingUnitPath);
                }
                else
                    throw new KeyNotFoundException("The hostingUnit is not exist");
            }
            else
                throw new KeyNotFoundException("The hostingUnit is not exist");
        }
        public HostingUnit GetHostingUnit(int key)
        {
            HostingUnit h = loadListFromXML<HostingUnit>(HostingUnitPath).FirstOrDefault(ho => ho.numHostingUnitKey == key);
            if (h == null)
                throw new KeyNotFoundException("The hostingUnit is not exist");
            return h;

        }
        public HostingUnit GetHostingUnit(string name)
        {
            HostingUnit h = loadListFromXML<HostingUnit>(HostingUnitPath).FirstOrDefault(ho => ho.HostingUnitName == name);
            if (h == null)
                throw new KeyNotFoundException("The hostingUnit is not exist");
            return h;

        }
        #endregion

        #region Order
        public int addOrder(Order order)
        {
            try
            {
                LoadDataconfigurationXML();
                int code = int.Parse(configurationXML.Element("OrderKey").Value);
                order.numOrderKey = code++;
                configurationXML.Element("OrderKey").Value = code.ToString();
                configurationXML.Save(configurationPath);
                order.Status = Enums.StatusOrder.טרם_טופל;
                order.CreateDate = DateTime.Now;

                listOfOrders.Add(order);
                orderXML.Add(orderToXElement(order));
                orderXML.Save(OrderPath);
                return order.numOrderKey;
            }
            catch (FileLoadException ex)
            {
                throw ex;
            }
        }
        public void updateOrder(int Key, Enums.StatusOrder status)
        {
            XElement orderElement = (from item in orderXML.Elements()
                                     where Convert.ToInt32(item.Element("numOrderKey").Value) == Key
                                     select item).FirstOrDefault();
            if (orderElement != null)
            {
                orderElement.Element("Status").Value = status.ToString();
                if (status == Enums.StatusOrder.נשלח_מייל)
                {
                    orderElement.Element("OrderDate").Value = DateTime.Now.ToString();
                }
                orderXML.Save(OrderPath);
            }

            else
                throw new KeyNotFoundException("The order is not exist");
        }
        public Order GetOrder(int key)
        {
            XElement orderElement = (from item in orderXML.Elements()
                                     where Convert.ToInt32(item.Element("numOrderKey").Value) == key
                                     select item).FirstOrDefault();
            if (orderElement != null)
            {
                return XElementToOrder(orderElement);
            }
            else
                throw new KeyNotFoundException("לא קיימת במערכת הזמנה עם המספר שנשלח");
        }

        #endregion

        public int addHost(Host host)
        {
            List<Host> hosts = loadListFromXML<Host>(HostPath);
            LoadDataconfigurationXML();
            int code = int.Parse(configurationXML.Element("HostKey").Value);
            code++;
            configurationXML.Element("HostKey").Value = code.ToString();
            configurationXML.Save(configurationPath);
            host.HostKey = code;
            host.numOfUnit = 0;
            hosts.Add(host);
            saveListToXML(hosts, HostPath);
            return host.HostKey;
        }

        public int GetHostKey()
        {
            try
            {
                LoadDataconfigurationXML();
                int code = int.Parse(configurationXML.Element("HostKey").Value);
                code++;
                configurationXML.Element("HostKey").Value = code.ToString();
                configurationXML.Save(configurationPath);
                return code;
            }
            catch (FileLoadException ex)
            {
                throw ex;
            }
            
        }

        #region getLists
        public List<HostingUnit> allHostingUnits()
        {
            listOfHostingUnits = loadListFromXML<HostingUnit>(HostingUnitPath);
            return getList(listOfHostingUnits);
        }
        public List<GuestRequest> allGuestRequest()
        {
            listOfGuestRequest = loadListFromXML<GuestRequest>(GuestRequestPath);
            return getList(listOfGuestRequest);
        }
        public List<Order> allOrders()
        {
            List<Order> orders = new List<Order>();
            foreach (var item in orderXML.Elements())
            {
                orders.Add(XElementToOrder(item));
            }
            return getList(orders);
        }
        public List<BankAccount> allBanks()
        {
            List<BankAccount> banks = new List<BankAccount>();
            foreach (var item in bankXML.Elements())
            {
                banks.Add(XElementToBankAccount(item));
            }
            return getList(banks);
        }
        public List<Host> allHosts()
        {
            List<Host> hosts = loadListFromXML<Host>(HostPath);
           
            return getList(hosts);
        }
        public static List<T> getList<T>(List<T> ts)
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
                SubArea = gust.SubArea,
                Type = gust.Type,
                Adults = gust.Adults,
                Price = gust.Price,
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
            catch (KeyNotFoundException ex)
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
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
        }
        #endregion

        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath = @"https://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_dnld_he.xml";
                wc.DownloadFile(xmlServerPath, BankPath);
            }
            catch /*(WebException ex)*/
            {
                throw new WebException("האתר לא עלה");
                //string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                //wc.DownloadFile(xmlServerPath, BankPath);
            }
            finally
            {
                wc.Dispose();
            }
        }

        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BankAccount bankAccount = new BankAccount();
            foreach (var item in bankXML.Elements())
            {
                bankAccount = XElementToBankAccount(item);
                bankAccounts.Add(bankAccount);
            }
        }

        public int configurationData(string item)
        {
            LoadDataconfigurationXML();
            int code = int.Parse(configurationXML.Element(item).Value);
            return code;
        }
    }
}





