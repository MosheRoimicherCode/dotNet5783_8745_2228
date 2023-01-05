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
    public partial class CartWindow : Window
    {
        IBl p = Factory.Get();
        private BO.Cart cart = new();
        private IEnumerable<BO.ProductItem>?productItemcartList;
      
        public IEnumerable<BO.ProductItem?> productItemcartListUpdate
        {
            get => productItemcartList;
            set
            {
                productItemcartList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("productItemcartListUpdate"));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public CartWindow(BO.Cart currentCart)
        {
            InitializeComponent();
            UserName.Text = currentCart?.CustomerName?.ToString();
            UserAddress.Text = currentCart?.CustomeAdress?.ToString();
            UserEmail.Text = currentCart?.CustomerEmail?.ToString();
            productItemcartListUpdate = p.Product.GetListOfItemsInCart(currentCart);
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
            this.DataContext = productItemcartListUpdate;

            cart = currentCart;
            UserName.IsEnabled = false;
            UserAddress.IsEnabled = false;
            UserEmail.IsEnabled = false;
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
        }


        private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int? id = ((BO.ProductItem)CartList.SelectedItem).ID;
            new ProductInCartWindow((int)id, cart).Show();
            this.Close();
        }

        private void Confirm_Order(object sender, RoutedEventArgs e)
        {
            p.Cart.ConfirmCart(cart, cart.CustomerName!, cart.CustomerEmail!, cart.CustomeAdress!);
            
            //new MainWindow().Show();
            //new OrderForListWindow().Show();
            this.Close();
           
        }

        private void UpdateUserProperties()
        {
            UserName.IsEnabled = true;
            UserAddress.IsEnabled = true;
            UserEmail.IsEnabled = true;
        }

        private void CartList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
