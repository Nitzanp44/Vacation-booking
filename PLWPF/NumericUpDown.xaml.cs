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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public int num { get; set; }
        public int max {get; set;}
        public int min { get; set; }
        public NumericUpDown()
        {
            InitializeComponent();
            max = 20;
            min = 0;
            num = 0;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (num > min)
            {
                num--;
                txtNum.Content = num;
            }
            else
                num = min;
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            if (num < max)
            {
                num++;
                txtNum.Content = num;
            }
            else

                num = max;
        }

        private void txtNum_TextInput(object sender, TextCompositionEventArgs e)
        {
            txtNum.Content = num;
        }
    }

    //    private float? num = null;
    //    public float? Value
    //    { get { return num; }
    //        set { if (value > MaxValue) num = MaxValue;
    //            else if (value < MinValue) num = MinValue;
    //            else num = value;
    //            txtNum.Content = num == null ? "" : num.ToString(); } }
    //    public int MinValue { get; set; }
    //    public int MaxValue { get; set; }
    //    public NumericUpDown()
    //    {
    //        InitializeComponent();
    //        MaxValue = 20;
    //    }

    //    private void cmdUp_Click(object sender, RoutedEventArgs e)
    //    {
    //        Value++;
    //    }

    //    private void cmdDown_Click(object sender, RoutedEventArgs e)
    //    {
    //        Value--;
    //    }
    //}
}
