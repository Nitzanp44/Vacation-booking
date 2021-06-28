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
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for newHost.xaml
    /// </summary>
    public partial class newHost : Window
    {
        IBL bl = FactoryBL.GetBl();
        Host host;

        public newHost()
        {
            InitializeComponent();
            host = new Host()
            {
                BankAccuontHost = new BankAccount()
            };

            this.hostGrid.DataContext = host;
            List<string> namesbanks = new List<string>();
            foreach (var item in bl.allBanks())
            {
                if (!(namesbanks.Contains(item.BankName)))
                    namesbanks.Add(item.BankName);
            }
            nameBank.ItemsSource = namesbanks;


           
        }
    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource hostViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("hostViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // hostViewSource.Source = [generic data source]
        }

        private void continue_Click(object sender, RoutedEventArgs e)
        {
            host.HostKey = bl.addHost(host);
            if(collectionClearanceCheckBox.IsChecked==false)
            {
                if (MessageBox.Show("ללא הרשאת גישה לחשבון לא ניתן לשלוח הזמנות ללקוחות. האם תרצה להמשיך?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    new hostWindowHosting().Show();
                    this.Close();
                }
            }
            else
            {
                hostWindowHosting hostWindowHosting = new hostWindowHosting();
                hostWindowHosting.hostKey = host.HostKey;
                hostWindowHosting.Show();
                this.Close();
            }

            

        }

        private void back1_Click(object sender, RoutedEventArgs e)
        {
            new HostWindow().Show();
            this.Close();
        }

        private void nameBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = 0;
            List<int> numsbranchs = new List<int>();
            foreach (var item in bl.allBanks())
            {
                if ((string)nameBank.SelectedValue == item.BankName)
                {
                    numsbranchs.Add(item.BranchNumber);
                    if (i == 0)
                    {
                        numBank.Text = Convert.ToString(item.BankNumber);
                        i++;
                    }
                }
            }

            numsbranchs.Sort();
            numBranch.ItemsSource = numsbranchs;
            host.BankAccuontHost.BankName = (string)nameBank.SelectedValue;

        }

        private void numBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in bl.allBanks())
            {
                if ((string)nameBank.SelectedValue == item.BankName && (int)numBranch.SelectedValue==item.BranchNumber)
                {
                    addressBranch.Text = Convert.ToString(item.BranchAddress);
                    city.Text= Convert.ToString(item.BranchCity);
                }
            }

            host.BankAccuontHost.BankNumber = Convert.ToInt32(numBank.Text);
            host.BankAccuontHost.BranchAddress = addressBranch.Text;
            host.BankAccuontHost.BranchCity = city.Text;
            host.BankAccuontHost.BranchNumber = (int)numBranch.SelectedValue;
        }

        private void collectionClearanceCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (collectionClearanceCheckBox.IsChecked == true)
            {
                host.CollectionClearance = true;
            }
            else
            {
                host.CollectionClearance = false;
            }
        }
    }
}
