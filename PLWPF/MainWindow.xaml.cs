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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using BE;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IBL bl = BL.FactoryBL.GetBl();
            InitializeComponent();
        }

        private void Buser_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().Show();
            this.Close();
        }

        private void BHost_Click(object sender, RoutedEventArgs e)
        {
            new HostWindow().Show();
            this.Close();
        }

        private void Bmanager_Click(object sender, RoutedEventArgs e)
        {
            new ManagWindow().Show();
            this.Close();
        }
    }
}
