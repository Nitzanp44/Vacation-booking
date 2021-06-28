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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for hostWindowHosting.xaml
    /// </summary>
    public partial class hostWindowHosting : Window
    {
        IBL bl = FactoryBL.GetBl();
        HostingUnit hostingUnit;
        Host host;
public int hostKey { get; set; }

        public hostWindowHosting()
        {
            InitializeComponent();
            hostingUnit = new HostingUnit();
            
            this.addHostingGrid.DataContext = hostingUnit;
            List<Enums.Area> areas = new List<Enums.Area>() { Enums.Area.דרום, Enums.Area.ירושלים, Enums.Area.מרכז, Enums.Area.צפון };
            Area.ItemsSource = areas;
            Type.ItemsSource = Enum.GetValues(typeof(Enums.KindOfVication));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostingUnitViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostingUnitViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostingUnitViewSource.Source = [generic data source]
        }

        #region addHosting

        private void addHosting_Click(object sender, RoutedEventArgs e)
        {
            host = bl.getBLHost(hostKey);
            Hello.Text = "שלום " + host.PrivateName + "!";
            addHostingGrid.Visibility = Visibility.Visible;
            hostingGrid.Visibility = Visibility.Collapsed;
        }
        private void PoolY_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Pool = true;
        }

        private void PoolN_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Pool = false;
        }

        private void JacuzziY_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Jacuzzi = true;
        }

        private void JacuzziN_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Jacuzzi = false;
        }

        private void BreakY_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Breakfast = true;
        }

        private void BreakN_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Breakfast = false;
        }

        private void GardenY_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Garden = true;
        }

        private void GardenN_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.Garden = false;
        }

        private void AttractionY_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.ChildrensAttractions = true;
        }

        private void AttractionN_Checked(object sender, RoutedEventArgs e)
        {
            hostingUnit.ChildrensAttractions = false;
        }

        private void Area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (hostingUnit.Area == Enums.Area.דרום)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaSouth));
            }
            if (hostingUnit.Area == Enums.Area.ירושלים)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaJerus));
            }
            if (hostingUnit.Area == Enums.Area.מרכז)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaCenter));
            }
            if (hostingUnit.Area == Enums.Area.צפון)
            {
                subArea.ItemsSource = Enum.GetValues(typeof(Enums.SubAreaNorth));
            }

        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                hostingUnit.Capacity = (int)Capacity.txtNum.Content;
                host = bl.getBLHost(hostKey);
                hostingUnit.Owner = host;
                bl.addHostUnit(hostingUnit);
                addHostingGrid.Visibility = Visibility.Collapsed;
                hostingGrid.Visibility = Visibility.Visible;
                hostingUnit = new HostingUnit();
                HostingName.Text = "";
                hostingAddress.Text = "";
                hostingPrice.Text = "";
                PoolN.IsChecked = false;
                PoolY.IsChecked = false;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region updateHosting
        private void updateHosting_Click(object sender, RoutedEventArgs e)
        {
            host = bl.getBLHost(hostKey);
            Hello.Text = "שלום " + host.PrivateName + "!";
            this.hostingUnitDataGrid.ItemsSource = bl.allHostingUnitsOfHost(hostKey);
            updateHostingGrid.Visibility = Visibility.Visible;
            hostingGrid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region removeHosting
        private void removeHosting_Click(object sender, RoutedEventArgs e)
        {
            host = bl.getBLHost(hostKey);
            Hello.Text = "שלום " + host.PrivateName + "!";
            this.removeHostingUnitDataGrid.ItemsSource = bl.allHostingUnitsOfHost(hostKey);
            removeHostingGrid.Visibility = Visibility.Visible;
            hostingGrid.Visibility = Visibility.Collapsed;


        }


        private void remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show("בטוח שברצונך למחוק יחידה זו?");
                if (MessageBox.Show("בטוח שברצונך למחוק יחידה זו?", "מחיקה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    int num = Convert.ToInt32(numToRemove.Text);
                    bl.removeHostUnit(bl.getBLHostingUnit(num));
                }
                else
                {
                    remove.IsEnabled = false;
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw ex;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            remove.IsEnabled = true;
        }
        #endregion

        #region bake
        private void back4_Click(object sender, RoutedEventArgs e)
        {
            removeHostingGrid.Visibility = Visibility.Collapsed;
            hostingGrid.Visibility = Visibility.Visible;
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            addHostingGrid.Visibility = Visibility.Collapsed;
            hostingGrid.Visibility = Visibility.Visible;
        }

        private void back1_Click(object sender, RoutedEventArgs e)
        {
            new HostWindow().Show();
            this.Close();
        }
        private void back2_Click(object sender, RoutedEventArgs e)
        {
            updateHostingGrid.Visibility = Visibility.Collapsed;
            hostingGrid.Visibility = Visibility.Visible;
        }

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        #endregion
    }
}
