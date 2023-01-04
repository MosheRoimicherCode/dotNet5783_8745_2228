using BlApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        IBl p = Factory.Get();
        private BO.Cart cart = new();
        private IEnumerable<BO.OrderItem>?orderItemcartList;
      
        public IEnumerable<BO.OrderItem?> orderItemcartListUpdate
        {
            get => orderItemcartList;
            set
            {
                orderItemcartList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("cartListUpdate"));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public Window1(BO.Cart currentCart)
        {
            InitializeComponent();
            UserName.Text = currentCart.CustomerName.ToString();
            UserAddress.Text = currentCart.CustomeAdress.ToString();
            UserEmail.Text = currentCart.CustomerEmail.ToString();
            orderItemcartListUpdate = currentCart.Details;
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
            this.DataContext = orderItemcartListUpdate;

            UserName.IsEnabled = false;
            UserAddress.IsEnabled = false;
            UserEmail.IsEnabled = false;
        }

        public void CreateUser()
        {
              
            

            
        }

        public void UpdateUserProperties()
        {
            UserName.IsEnabled = true;
            UserAddress.IsEnabled = true;
            UserEmail.IsEnabled = true;
        }
    }
}
