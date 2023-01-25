using BlApi;
using BO;
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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for deleteOrderWindow.xaml
    /// </summary>
    public partial class deleteOrderWindow : Window
    {
        IBl p = Factory.Get();
        int id;
        OrderForListWindow orderForListWindow;
        OrderWindow orderWindow;
        public deleteOrderWindow(int ID, OrderForListWindow window1, OrderWindow window2)
        {
            InitializeComponent();
            id = ID;
            orderForListWindow = window1;
            orderWindow = window2;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            p.Order.Delete(id);
            orderForListWindow.Close();
            orderWindow.Close();
            this.Close();
            new OrderForListWindow().Show();
        }
    }

    
}

