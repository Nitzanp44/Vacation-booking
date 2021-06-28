using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ManagWindow.xaml
    /// </summary>
    public partial class ManagWindow : Window
    {
        IBL bl = FactoryBL.GetBl();
        Host host = new Host();
        public ManagWindow()
        {
            InitializeComponent();
            //מיון מארח
            groupByHost.Items.Add("הכל");
            groupByHost.Items.Add("מארחים בעלי אישור גבייה");
            groupByHost.Items.Add("מספר יחידות האירוח");

            //מיון הזמנות
            groupByOrder.Items.Add("הכל");
            groupByOrder.Items.Add("נסגר דרך האתר");
            groupByOrder.Items.Add("נשלח מייל");
            groupByOrder.Items.Add("נסגר מחוסר היענות");
            groupByParameter.Items.Add("הכל");
            groupByParameter.Items.Add("לפי בקשת לקוח");
            groupByParameter.Items.Add("לפי יחידת אירוח");


            //מיון יחידות אירוח
           
            groupByArea.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            List<string> parameters = new List<string>() { "הכל", "בריכה", "ג'קוזי", "ארוחת בוקר", "גינה", "אטרקציות לילדים" };
            groupHostingByParameter.ItemsSource = parameters;
            keyNumHosting.Items.Add("הכל");
            foreach (var item in bl.allHosts())
            {
                keyNumHosting.Items.Add(item.HostKey);
            }


            //מיון דרישות לקוח
            List<string> option = new List<string>() { "הכל", "לפי סטטוס", "לפי אזור", "אפשרויות נוספות" };
            groupGuestRequest.ItemsSource = option;


            this.hostDataGrid.ItemsSource = bl.allHosts();
            this.orderDataGrid.ItemsSource = bl.allOrders();
            this.hostingUnitDataGrid.ItemsSource = bl.allHostingUnits();
            this.guestRequestDataGrid.ItemsSource = bl.allGuestRequest();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource guestRequestViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("guestRequestViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // guestRequestViewSource.Source = [generic data source]
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (Password.Password == "1234")
                {
                    Pass.Visibility = Visibility.Collapsed;
                    gridManag.Visibility = Visibility.Visible;
                }
                else
                {
                    //Password.Password = "";
                    worng.Visibility = Visibility.Visible;
                }
            }
        }

        #region Host
        private void groupByHost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)groupByHost.SelectedValue == "הכל")
                this.hostDataGrid.ItemsSource = bl.allHosts();

            if ((string)groupByHost.SelectedValue == "מארחים בעלי אישור גבייה")
                this.hostDataGrid.ItemsSource = bl.filterByCollectionClearance();

            if ((string)groupByHost.SelectedValue == "מספר יחידות האירוח")
            {
                List<IGrouping<int, Host>> hostingOfHost;
                List<Host> hostingOfHostList = new List<Host>();
                hostingOfHost = bl.filterHostByHostingunit();

                foreach (var item in hostingOfHost)
                {
                    foreach (var v in item)
                    {
                        hostingOfHostList.Add(v);
                    }
                }

                hostDataGrid.ItemsSource = hostingOfHostList;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(hostDataGrid.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("numOfUnit");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        #endregion

        #region back
        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            Pass.Visibility = Visibility.Visible;
            gridManag.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region order
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (groupByParameter.SelectedValue != null && groupByOrder.SelectedValue != null)
            {
                filter.IsEnabled = true;
            }
        }

        private void groupByParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ("לפי בקשת לקוח" == (string)groupByParameter.SelectedValue)
            {
                keyNum.Items.Clear();
                keyNum.Visibility = Visibility.Hidden;
                selectUnit.Visibility = Visibility.Hidden;
                selectRequest.Visibility = Visibility.Hidden;
                foreach (var item in bl.allGuestRequest())
                {
                    keyNum.Items.Add(item.numGuestRequest);
                }
                keyNum.Visibility = Visibility.Visible;
                selectRequest.Visibility = Visibility.Visible;
            }

            if ("לפי יחידת אירוח" == (string)groupByParameter.SelectedValue)
            {
                keyNum.Items.Clear();
                keyNum.Visibility = Visibility.Hidden;
                selectUnit.Visibility = Visibility.Hidden;
                selectRequest.Visibility = Visibility.Hidden;
                foreach (var item in bl.allHostingUnits())
                {
                    keyNum.Items.Add(item.numHostingUnitKey);
                }
                keyNum.Visibility = Visibility.Visible;
                selectUnit.Visibility = Visibility.Visible;
            }

            if ("הכל" == (string)groupByParameter.SelectedValue)
            {
                keyNum.Items.Clear();
                keyNum.Visibility = Visibility.Hidden;
                selectUnit.Visibility = Visibility.Hidden;
                selectRequest.Visibility = Visibility.Hidden;
            }

            if (groupByParameter.SelectedValue!=null && groupByOrder.SelectedValue!=null)
            {
                filter.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ("הכל" == (string)groupByParameter.SelectedValue && "הכל" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.allOrders();
                }

                if ("הכל" == (string)groupByParameter.SelectedValue && "נסגר דרך האתר" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterOrderByStatus(Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                }

                if ("הכל" == (string)groupByParameter.SelectedValue && "נשלח מייל" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterOrderByStatus(Enums.StatusOrder.נשלח_מייל);
                }

                if ("הכל" == (string)groupByParameter.SelectedValue && "נסגר מחוסר היענות" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterOrderByStatus(Enums.StatusOrder.נסגר_מחוסר_היענות);
                }



                if ("לפי בקשת לקוח" == (string)groupByParameter.SelectedValue && "הכל" == (string)groupByOrder.SelectedValue)
                {
                    List<Order> lst = bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                    lst.AddRange(bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_מחוסר_היענות));
                    lst.AddRange(bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נשלח_מייל));
                    this.orderDataGrid.ItemsSource = lst;
                }

                if ("לפי בקשת לקוח" == (string)groupByParameter.SelectedValue && "נסגר דרך האתר" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                }

                if ("לפי בקשת לקוח" == (string)groupByParameter.SelectedValue && "נשלח מייל" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נשלח_מייל);
                }

                if ("לפי בקשת לקוח" == (string)groupByParameter.SelectedValue && "נסגר מחוסר היענות" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersOfRequest((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_מחוסר_היענות);
                }


                if ("לפי יחידת אירוח" == (string)groupByParameter.SelectedValue && "הכל" == (string)groupByOrder.SelectedValue)
                {
                    List<Order> lst = bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                    lst.AddRange(bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_מחוסר_היענות));
                    lst.AddRange(bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נשלח_מייל));
                    this.orderDataGrid.ItemsSource = lst;
                }

                if ("לפי יחידת אירוח" == (string)groupByParameter.SelectedValue && "נסגר דרך האתר" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
                }

                if ("לפי יחידת אירוח" == (string)groupByParameter.SelectedValue && "נשלח מייל" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נשלח_מייל);
                }

                if ("לפי יחידת אירוח" == (string)groupByParameter.SelectedValue && "נסגר מחוסר היענות" == (string)groupByOrder.SelectedValue)
                {
                    this.orderDataGrid.ItemsSource = bl.filterStatusOrdersInUnit((int)keyNum.SelectedValue, Enums.StatusOrder.נסגר_מחוסר_היענות);
                }
            }

            catch
            {
                MessageBox.Show("לא הוזנו פרטים כראוי");
            }

        }
        #endregion

        #region hosting Unit
        private void filter1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (0 == groupHostingByParameter.SelectedIndex && 0 ==groupByArea.SelectedIndex && 0 == keyNumHosting.SelectedIndex)
                {
                    this.hostingUnitDataGrid.ItemsSource = bl.allHostingUnits();
                }

                if (0 == groupHostingByParameter.SelectedIndex && 0 == groupByArea.SelectedIndex && 0 != keyNumHosting.SelectedIndex)
                {
                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByHost(bl.allHostingUnits(), (int)keyNumHosting.SelectedValue);
                }

                if (0 == groupHostingByParameter.SelectedIndex && 0 != groupByArea.SelectedIndex && 0 != keyNumHosting.SelectedIndex)
                {
                   
                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByArea(bl.filterHostingByHost(bl.allHostingUnits(), (int)keyNumHosting.SelectedValue), (Enums.Area)groupByArea.SelectedValue);
                }

                if (0 == groupHostingByParameter.SelectedIndex && 0 != groupByArea.SelectedIndex && 0 == keyNumHosting.SelectedIndex)
                {
                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByArea(bl.allHostingUnits(), (Enums.Area)groupByArea.SelectedValue);
                }

                if (0 != groupHostingByParameter.SelectedIndex && 0 == groupByArea.SelectedIndex && 0 == keyNumHosting.SelectedIndex)
                {
                    if ((string)groupHostingByParameter.SelectedValue == "בריכה")
                        this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByParameter(x => x.Pool == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ג'קוזי")
                        this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByParameter(x => x.Jacuzzi == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ארוחת בוקר")
                        this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByParameter(x => x.Breakfast == true);
                    if ((string)groupHostingByParameter.SelectedValue == "גינה")
                        this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByParameter(x => x.Garden == true);
                    if ((string)groupHostingByParameter.SelectedValue == "אטרקציות לילדים")
                        this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByParameter(x => x.ChildrensAttractions == true);
                }

                if (0 != groupHostingByParameter.SelectedIndex && 0 == groupByArea.SelectedIndex && 0 != keyNumHosting.SelectedIndex)
                {
                    List<HostingUnit> lst = new List<HostingUnit>();
                    if ((string)groupHostingByParameter.SelectedValue == "בריכה")
                        lst = bl.filterHostingByParameter(x => x.Pool == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ג'קוזי")
                        lst = bl.filterHostingByParameter(x => x.Jacuzzi == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ארוחת בוקר")
                        lst = bl.filterHostingByParameter(x => x.Breakfast == true);
                    if ((string)groupHostingByParameter.SelectedValue == "גינה")
                        lst = bl.filterHostingByParameter(x => x.Garden == true);
                    if ((string)groupHostingByParameter.SelectedValue == "אטרקציות לילדים")
                        lst = bl.filterHostingByParameter(x => x.ChildrensAttractions == true);

                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByHost(lst, (int)keyNumHosting.SelectedValue);
                }

                if (0 != groupHostingByParameter.SelectedIndex && 0 != groupByArea.SelectedIndex && 0 == keyNumHosting.SelectedIndex)
                {
                    List<HostingUnit> lst = new List<HostingUnit>();
                    if ((string)groupHostingByParameter.SelectedValue == "בריכה")
                        lst = bl.filterHostingByParameter(x => x.Pool == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ג'קוזי")
                        lst = bl.filterHostingByParameter(x => x.Jacuzzi == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ארוחת בוקר")
                        lst = bl.filterHostingByParameter(x => x.Breakfast == true);
                    if ((string)groupHostingByParameter.SelectedValue == "גינה")
                        lst = bl.filterHostingByParameter(x => x.Garden == true);
                    if ((string)groupHostingByParameter.SelectedValue == "אטרקציות לילדים")
                        lst = bl.filterHostingByParameter(x => x.ChildrensAttractions == true);

                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByArea(lst, (Enums.Area)groupByArea.SelectedValue);
                }

                if (0 != groupHostingByParameter.SelectedIndex && 0 != groupByArea.SelectedIndex && 0 != keyNumHosting.SelectedIndex)
                {
                    List<HostingUnit> lst = new List<HostingUnit>();
                    if ((string)groupHostingByParameter.SelectedValue == "בריכה")
                        lst = bl.filterHostingByParameter(x => x.Pool == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ג'קוזי")
                        lst = bl.filterHostingByParameter(x => x.Jacuzzi == true);
                    if ((string)groupHostingByParameter.SelectedValue == "ארוחת בוקר")
                        lst = bl.filterHostingByParameter(x => x.Breakfast == true);
                    if ((string)groupHostingByParameter.SelectedValue == "גינה")
                        lst = bl.filterHostingByParameter(x => x.Garden == true);
                    if ((string)groupHostingByParameter.SelectedValue == "אטרקציות לילדים")
                        lst = bl.filterHostingByParameter(x => x.ChildrensAttractions == true);

                    this.hostingUnitDataGrid.ItemsSource = bl.filterHostingByArea(bl.filterHostingByHost(lst, (int)keyNumHosting.SelectedValue), (Enums.Area)groupByArea.SelectedValue);
                }

            }

            catch
            {
                MessageBox.Show("לא הוזנו פרטים כראוי");
            }
        }

        #endregion

        #region guest Request
        private void groupGuestRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (groupGuestRequest.SelectedIndex == 0)
            {
                groupGuestRequestByParameter.Visibility = Visibility.Hidden;
            }

            if (groupGuestRequest.SelectedIndex == 1)
            {
                groupGuestRequestByParameter.Items.Clear();
                
                groupGuestRequestByParameter.Visibility = Visibility.Visible;
                groupGuestRequestByParameter.ItemsSource = Enum.GetValues(typeof(Enums.StatusClient));
            }

            if (groupGuestRequest.SelectedIndex == 2)
            {
                groupGuestRequestByParameter.Visibility = Visibility.Hidden;
            }

            if (groupGuestRequest.SelectedIndex == 3)
            {
                //groupGuestRequestByParameter.Items.Clear();
                groupGuestRequestByParameter.Visibility = Visibility.Visible;
                List<string> parameters1 = new List<string>() { "הכל", "בריכה", "ג'קוזי", "ארוחת בוקר", "גינה", "אטרקציות לילדים" };
                groupGuestRequestByParameter.ItemsSource = parameters1;
                
            }
        }

        private void filter2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (groupGuestRequest.SelectedIndex == 0)
                    this.guestRequestDataGrid.ItemsSource = bl.allGuestRequest();

                if (groupGuestRequest.SelectedIndex == 1)
                {
                    if (groupGuestRequestByParameter.SelectedIndex == 0)
                        this.guestRequestDataGrid.ItemsSource = bl.filterStatusRequest(Enums.StatusClient.פתוחה);

                    if (groupGuestRequestByParameter.SelectedIndex == 1)
                        this.guestRequestDataGrid.ItemsSource = bl.filterStatusRequest(Enums.StatusClient.נסגרה_עסקה_דרך_האתר);

                    if (groupGuestRequestByParameter.SelectedIndex == 2)
                        this.guestRequestDataGrid.ItemsSource = bl.filterStatusRequest(Enums.StatusClient.נסגרה_כי_פג_תוקפה);

                }

                if (groupGuestRequest.SelectedIndex == 2)
                {

                    IEnumerable<IGrouping<Enums.Area, GuestRequest>> guestRequests;
                    List<GuestRequest> guestRequestsList = new List<GuestRequest>();
                    guestRequests = bl.filterGuestByArea();

                    foreach (var item in guestRequests)
                    {
                        foreach (var v in item)
                        {
                            guestRequestsList.Add(v);
                        }
                    }

                    guestRequestDataGrid.ItemsSource = guestRequestsList;

                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(guestRequestDataGrid.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Area");
                    view.GroupDescriptions.Add(groupDescription);
                }

                if (groupGuestRequest.SelectedIndex == 3)
                {
                    if ((string)groupGuestRequestByParameter.SelectedValue == "בריכה")
                        this.guestRequestDataGrid.ItemsSource = bl.filterGuestByParameter(x => x.Pool != Enums.Option.לא_מעוניין);
                    if ((string)groupGuestRequestByParameter.SelectedValue == "ג'קוזי")
                        this.guestRequestDataGrid.ItemsSource = bl.filterGuestByParameter(x => x.Jacuzzi != Enums.Option.לא_מעוניין);
                    if ((string)groupGuestRequestByParameter.SelectedValue == "ארוחת בוקר")
                        this.guestRequestDataGrid.ItemsSource = bl.filterGuestByParameter(x => x.Breakfast != Enums.Option.לא_מעוניין);
                    if ((string)groupGuestRequestByParameter.SelectedValue == "גינה")
                        this.guestRequestDataGrid.ItemsSource = bl.filterGuestByParameter(x => x.Garden != Enums.Option.לא_מעוניין);
                    if ((string)groupGuestRequestByParameter.SelectedValue == "אטרקציות לילדים")
                        this.guestRequestDataGrid.ItemsSource = bl.filterGuestByParameter(x => x.ChildrensAttractions != Enums.Option.לא_מעוניין);
                }

            }

            catch
            {
                MessageBox.Show("לא הוזנו פרטים כראוי");
            }
        }

        #endregion
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            worng.Visibility = Visibility.Collapsed;
        }
    }
}
