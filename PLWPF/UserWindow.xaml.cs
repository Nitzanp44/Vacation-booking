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
using System.Text.RegularExpressions;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        IBL bl = FactoryBL.GetBl();
        GuestRequest guestRequest;
        public UserWindow()
        {
            InitializeComponent();
            guestRequest = new GuestRequest();
            this.GuestRequestGrid.DataContext = guestRequest;
            List<Enums.Area> areas = new List<Enums.Area>() { Enums.Area.דרום, Enums.Area.ירושלים, Enums.Area.מרכז, Enums.Area.צפון };
            Area.ItemsSource = areas;
            Type.ItemsSource = Enum.GetValues(typeof(Enums.KindOfVication));
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region vacation details
        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (guestRequest.Area == Enums.Area.דרום)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaSouth));
            }
            if (guestRequest.Area == Enums.Area.ירושלים)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaJerus));
            }
            if (guestRequest.Area == Enums.Area.מרכז)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaCenter));
            }
            if (guestRequest.Area == Enums.Area.צפון)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaNorth));
            }

        }

        private void subArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (guestRequest.Area == Enums.Area.דרום)
            {
                if (subArea.SelectedIndex == 0)
                {
                    guestRequest.SubArea = Enums.SubArea.אילת;
                }
                if (subArea.SelectedIndex == 1)
                {
                    guestRequest.SubArea = Enums.SubArea.מצפה_רמון;
                }
                if (subArea.SelectedIndex == 2)
                {
                    guestRequest.SubArea = Enums.SubArea.ערבה;
                }
                if (subArea.SelectedIndex == 3)
                {
                    guestRequest.SubArea = Enums.SubArea.באר_שבע;
                }
            }
            if (guestRequest.Area == Enums.Area.ירושלים)
            {
                if (subArea.SelectedIndex == 0)
                {
                    guestRequest.SubArea = Enums.SubArea.בקעה;
                }
                if (subArea.SelectedIndex == 1)
                {
                    guestRequest.SubArea = Enums.SubArea.שפלה;
                }
                if (subArea.SelectedIndex == 2)
                {
                    guestRequest.SubArea = Enums.SubArea.ירושלים;
                }
                if (subArea.SelectedIndex == 3)
                {
                    guestRequest.SubArea = Enums.SubArea.בנימין;
                }
                if (subArea.SelectedIndex == 4)
                {
                    guestRequest.SubArea = Enums.SubArea.חברון;
                }
                if (subArea.SelectedIndex == 5)
                {
                    guestRequest.SubArea = Enums.SubArea.ים_המלח;
                }
            }
            if (guestRequest.Area == Enums.Area.מרכז)
            {
                if (subArea.SelectedIndex == 0)
                {
                    guestRequest.SubArea = Enums.SubArea.תל_אביב;
                }
                if (subArea.SelectedIndex == 1)
                {
                    guestRequest.SubArea = Enums.SubArea.נתניה;
                }
                if (subArea.SelectedIndex == 2)
                {
                    guestRequest.SubArea = Enums.SubArea.שומרון;
                }
                if (subArea.SelectedIndex == 3)
                {
                    guestRequest.SubArea = Enums.SubArea.אשדוד;
                }
            }
            if (guestRequest.Area == Enums.Area.צפון)
            {
                if (subArea.SelectedIndex == 0)
                {
                    guestRequest.SubArea = Enums.SubArea.גליל;
                }
                if (subArea.SelectedIndex == 1)
                {
                    guestRequest.SubArea = Enums.SubArea.גולן;
                }
                if (subArea.SelectedIndex == 2)
                {
                    guestRequest.SubArea = Enums.SubArea.חיפה;
                }
            }
        }
        private void releaseDateDatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            guestRequest.ReleaseDate = (DateTime)releaseDateDatePicker.SelectedDate;
        }

        private void entryDateDatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            guestRequest.EntryDate = (DateTime)entryDateDatePicker.SelectedDate;
        }
        private void Price_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Price.Value = (int)e.NewValue;
        }

        private void JacuzziY_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Jacuzzi = Enums.Option.הכרחי;
        }

        private void JacuzziN_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Jacuzzi = Enums.Option.לא_מעוניין;
        }

        private void JacuzziM_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Jacuzzi = Enums.Option.אפשרי;
        }

        private void PoolY_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Pool = Enums.Option.הכרחי;
        }

        private void PoolN_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Pool = Enums.Option.לא_מעוניין;
        }

        private void PoolM_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Pool = Enums.Option.אפשרי;
        }

        private void BreakY_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Breakfast = Enums.Option.הכרחי;
        }

        private void BreakN_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Breakfast = Enums.Option.לא_מעוניין;
        }

        private void BreakM_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Breakfast = Enums.Option.אפשרי;
        }

        private void GardenY_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Garden = Enums.Option.הכרחי;
        }

        private void GardenN_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Garden = Enums.Option.לא_מעוניין;
        }

        private void GardenM_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.Garden = Enums.Option.אפשרי;
        }

        private void AttractionY_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.ChildrensAttractions = Enums.Option.הכרחי;
        }

        private void AttractionN_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.ChildrensAttractions = Enums.Option.לא_מעוניין;
        }

        private void AttractionM_Checked(object sender, RoutedEventArgs e)
        {
            guestRequest.ChildrensAttractions = Enums.Option.אפשרי;
        }

        private void Adulds_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void privateNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }
        #endregion
       
        #region input validity

        private void familyNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            familyNameTextBox.BorderBrush = Brushes.Black;
            ErrorFamily.Text = "";
        }

        private void familyNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            guestRequest.FamilyName = familyNameTextBox.Text;

            if (!(checkWord(guestRequest.FamilyName)))
            {
                familyNameTextBox.BorderBrush = Brushes.Red;
                ErrorFamily.Text = "*אותיות בלבד";
                guestRequest.FamilyName = null;
            }
            else
            {
                if (guestRequest.FamilyName.Length < 2)
                {
                    familyNameTextBox.BorderBrush = Brushes.Red;
                    ErrorFamily.Text = "*שם חייב להיות לפחות 2 אותיות";
                    guestRequest.FamilyName = null;
                }
            }

            if (guestRequest.PrivateName != null && guestRequest.FamilyName != null && guestRequest.MailAddress != null)
            {
                Send.IsEnabled = true;
            }
            else
                Send.IsEnabled = false;

        }

        
    private void privateNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            privateNameTextBox.BorderBrush = Brushes.Black;
            ErrorName.Text = "";
        }

        private void privateNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            guestRequest.PrivateName = privateNameTextBox.Text;

            if (!(checkWord(guestRequest.PrivateName)))
            {
                privateNameTextBox.BorderBrush = Brushes.Red;
                ErrorName.Text = "*אותיות בלבד";
                guestRequest.PrivateName = null;
            }
            else
            {
                if (guestRequest.PrivateName.Length < 2)
                {
                    privateNameTextBox.BorderBrush = Brushes.Red;
                    ErrorName.Text = "שם חייב להיות לפחות 2 אותיות*";
                    guestRequest.PrivateName = null;
                }
            }
            if (guestRequest.PrivateName != null && guestRequest.FamilyName != null && guestRequest.MailAddress != null)
            {
                Send.IsEnabled = true;
            }
            else
                Send.IsEnabled = false;
        }

        private void mailAddressTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            mailAddressTextBox.BorderBrush = Brushes.Black;
            ErrorMail.Text = "";
        }

        private void mailAddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            guestRequest.MailAddress = mailAddressTextBox.Text;

            if (!(IsValidEmail(guestRequest.MailAddress)))
            {
                mailAddressTextBox.BorderBrush = Brushes.Red;
                ErrorMail.Text = "כתובת מייל לא חוקית*";
                guestRequest.MailAddress = null;
            }

            if (guestRequest.PrivateName != null && guestRequest.FamilyName != null && guestRequest.MailAddress != null)
            {
                Send.IsEnabled = true;
            }
            else
                Send.IsEnabled = false;
        }
        private bool checkWord(string str)
        {
            Regex regex = new Regex("[^א-ת]+");
            if (regex.IsMatch(str) && str != "\r")
            {
                return false;
            }
            return true;
        }
        bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (privateNameTextBox.Text != null && familyNameTextBox.Text != null && mailAddressTextBox.Text != null)
                {
                    Send.IsEnabled = true;
                    guestRequest.Adults = Convert.ToInt32(Adults.txtNum.Content);
                    if (guestRequest.Adults == 0)
                    {
                        throw new FormatException("לא ניתן להזמין חופשה ללא מבוגרים");
                    }
                    guestRequest.Children = Convert.ToInt32(Child.txtNum.Content);
                    guestRequest.Price = (int)Price.Value;
                    int num = bl.addRequest(guestRequest);
                    numRequest.Text = Convert.ToString(num);
                    GuestRequestGrid.Visibility = Visibility.Collapsed;
                    Approved.Visibility = Visibility.Visible;

                    guestRequest = new GuestRequest();
                    this.GuestRequestGrid.DataContext = guestRequest;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

    }
}
