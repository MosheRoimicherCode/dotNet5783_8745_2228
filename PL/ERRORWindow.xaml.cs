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

namespace PL
{
    /// <summary>
    /// Interaction logic for ERRORWindow.xaml
    /// </summary>
    public partial class ERRORWindow : Window
    {
        ProductWindow productWindow;

        public ERRORWindow(ProductWindow Window, string s)
        {
            InitializeComponent();
            label1.Content = s;
            productWindow = Window;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            productWindow.Show();
            this.Close();
        }
    }
}
