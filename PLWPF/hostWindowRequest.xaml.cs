using System;
using System.Collections.Generic;
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
using System.Data;
using System.Net.Mail;
using System.ComponentModel;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for hostWindowRequest.xaml
    /// </summary>
    public partial class hostWindowRequest : Window
    {
        IBL bl = FactoryBL.GetBl();
        GuestRequest guestRequest = new GuestRequest();
        Host host = new Host();
        BackgroundWorker backgroundWorker;
        int selectedClientToSendMail;
        int orderSendMail;

        public int hostKey { get; set; }
        public hostWindowRequest()
        {
            InitializeComponent();
            backgroundWorker = new BackgroundWorker();
            List<string> option = new List<string>() { "הכל", "לפי סטטוס", "לפי אזור", "אפשרויות נוספות" };
            groupGuestRequest.ItemsSource = option;
            this.guestRequestDataGrid.ItemsSource = bl.allGuestRequest();
            backgroundWorker.DoWork += Background_DoWork;
            backgroundWorker.RunWorkerCompleted += Background_RunWorkerCompleted;
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            host = bl.getBLHost(hostKey);
            Hello.Text = "שלום " + host.PrivateName + "!";
            List<HostingUnit> hostingUnits = bl.allHostingUnitsOfHost(hostKey);
            List<string> names = new List<string>();
            foreach (var item in hostingUnits)
            {
                names.Add(item.HostingUnitName);
            }
            myHosting.ItemsSource = names;
            orderDataGrid.ItemsSource = bl.filterOrdersOfHost(hostKey);

            System.Windows.Data.CollectionViewSource orderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("orderViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // orderViewSource.Source = [generic data source]

        }

        #region Guest Request
        private void groupGuestRequest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (groupGuestRequest.SelectedIndex == 0)//הכל
            {
                groupGuestRequestByParameter.Visibility = Visibility.Hidden;
            }

            if (groupGuestRequest.SelectedIndex == 1)//מיון לפי סטטוס
            {
                // groupGuestRequestByParameter.Items.Clear();

                groupGuestRequestByParameter.Visibility = Visibility.Visible;
                groupGuestRequestByParameter.ItemsSource = Enum.GetValues(typeof(Enums.StatusClient));
            }

            if (groupGuestRequest.SelectedIndex == 2)//מיון לפי אזור
            {
                groupGuestRequestByParameter.Visibility = Visibility.Hidden;
            }

            if (groupGuestRequest.SelectedIndex == 3)//מיון לפי פרמטר
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

        #region order
        private void sendOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order order = new Order();
                order.numGuestRequest = Convert.ToInt32(numRequest.Text);
                selectedClientToSendMail= Convert.ToInt32(numRequest.Text);
                
                order.numHostingUnitKey = bl.getBLHostingUnit(Convert.ToString(myHosting.SelectedItem)).numHostingUnitKey;
                int num = bl.addOrder(order);
                orderSendMail = num;
                if (backgroundWorker.IsBusy != true)
                    backgroundWorker.RunWorkerAsync(num);
                
            }
            catch (ConstraintException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(KeyNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Background_DoWork(object sender, DoWorkEventArgs e)
        {

            string mailAdressclient = bl.getBLGuestRequest(Convert.ToInt32(selectedClientToSendMail)).MailAddress;
            //MailMessage יצירת אובייקט
            MailMessage mail = new MailMessage();
            // כתובת הנמען)ניתן להוסיף יותר מאחד(
            mail.To.Add(mailAdressclient);
            // הכתובת ממנה נשלח המייל
            mail.From = new MailAddress("hofesh.com@gmail.com");
            // נושא ההודע ה
            mail.Subject = "יש לנו הצעה לחופשה שלך!";
            //( HTML תוכן ההודעה )נניח שתוכן ההודעה בפורמט
            mail.Body = "עובד?"/*"נשמח להציע לך חופשה חלומית ב " + bl.getBLHostingUnit(Convert.ToString(myHosting.SelectedItem)).HostingUnitName + " בכתובת " + bl.getBLHostingUnit(Convert.ToString(myHosting.SelectedItem)).Address + " נשמח אם תיצור איתנו קשר :)"*/;
            // HTML הגדרה שתוכן ההודעה בפורמט
            mail.IsBodyHtml = true;
            // Smtp יצירת עצם מסוג
            SmtpClient smtp = new SmtpClient();
            // gmail הגדרת השרת של
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            // gmail הגדרת פרטי הכניסה )שם משתמש וסיסמה( לחשבון ה
            smtp.Credentials = new System.Net.NetworkCredential("hofesh.com@gmail.com", "rn123456");
            // SSL ע"פ דרישת השר, חובה לאפשר במקרה זה
            smtp.EnableSsl = true;
            try
            {
                // שליחת ההודע ה
                smtp.Send(mail);
            }
            catch (ArgumentNullException ex)
            {
                // טיפול בשגיאות ותפיסת חריגות
                MessageBox.Show(ex.Message);
            }
            catch (SmtpException ex)
            {
                // טיפול בשגיאות ותפיסת חריגות
                MessageBox.Show(ex.Message);
            }
            //catch (SmtpFailedRecipientsException ex)
            //{
            //    // טיפול בשגיאות ותפיסת חריגות
            //    MessageBox.Show(ex.Message);
            //}
            catch (InvalidOperationException ex)
            {
                // טיפול בשגיאות ותפיסת חריגות
                MessageBox.Show(ex.Message);
            }
            e.Result = e;

        }


        private void Background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bl.updateOrder(orderSendMail, Enums.StatusOrder.נשלח_מייל);
        }

        private void closeOrder_Click(object sender, RoutedEventArgs e)
        {
            bl.updateOrder(Convert.ToInt32(numOrder.Text), Enums.StatusOrder.נסגר_בהיענות_של_הלקוח);
            bl.UpdateRequest(bl.getBLGuestRequest(bl.getBLOrder(Convert.ToInt32(numOrder.Text)).numGuestRequest).numGuestRequest, Enums.StatusClient.נסגרה_עסקה_דרך_האתר);
        }
        #endregion
        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
