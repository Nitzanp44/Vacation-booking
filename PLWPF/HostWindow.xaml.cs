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
    /// Interaction logic for HostWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        IBL bl = FactoryBL.GetBl();
        Host host = new Host();
        public HostWindow()
        {
            InitializeComponent();
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Return)
            {
                if (bl.getBLHost(Convert.ToInt32(Password.Password)) != null)
                {
                    Pass.Visibility = Visibility.Collapsed;
                    gridHost.Visibility = Visibility.Visible;
                }

                else
                {
                    MessageBox.Show("לא קיים במערכת, יש להירשם כמארח");
                }
            }
        }//תקינות סיסמא

        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }//חזרה למסך הראשי

        //אפשרויות המארח
        private void hostingButton_Click(object sender, RoutedEventArgs e)
        {
            hostWindowHosting hostWindowHosting = new hostWindowHosting();
            string str = this.Password.Password;
            hostWindowHosting.hostKey = Convert.ToInt32(str);
            hostWindowHosting.Show();
            this.Close();
        }

        private void requestButton_Click(object sender, RoutedEventArgs e)
        {
            hostWindowRequest hostWindowRequest = new hostWindowRequest();
            string str = this.Password.Password;
            hostWindowRequest.hostKey = Convert.ToInt32(str);
            hostWindowRequest.Show();
            this.Close();
        }

        private void newHost_Click(object sender, RoutedEventArgs e)
        {
            new newHost().Show();
            this.Close();
        }
    }
}
