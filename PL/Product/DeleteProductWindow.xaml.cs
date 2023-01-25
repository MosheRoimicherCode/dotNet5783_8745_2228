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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for deleteOrderWindow.xaml
    /// </summary>
    public partial class DeleteProductWindow : Window
    {
        IBl p = Factory.Get();
        int id;
        ProductForListWindow productForListWindow;
        ProductWindow productWindow;
        public DeleteProductWindow(int ID, ProductForListWindow window1, ProductWindow window2)
        {
            InitializeComponent();
            id = ID;
            productForListWindow = window1;
            productWindow = window2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            p.Product.Delete(id);
            productForListWindow.Close();
            productWindow.Close();
            this.Close();
            new ProductForListWindow().Show();
        }
    }


}


