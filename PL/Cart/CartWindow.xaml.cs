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
        readonly Action change;

        public IEnumerable<BO.ProductItem> productItemcartListUpdate
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

        public CartWindow(BO.Cart currentCart, Action? action = null)
        {
            InitializeComponent();
            productItemcartListUpdate = p.Product.GetListOfItemsInCart(currentCart);
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
            this.DataContext = productItemcartListUpdate;

            change = action;
            cart = currentCart;
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
            try
            {
                cart.CustomerName = UserName.Text;
                cart.CustomeAdress = UserAddress.Text;
                cart.CustomerEmail = UserEmail.Text;
                p.Cart.ConfirmCart(cart, cart.CustomerName!, cart.CustomerEmail!, cart.CustomeAdress!);
                this.Close();
            }
            catch (Exception s)
            {
                new ERRORWindow(this, s.Message).Show();
            }
            
            //change();
            //new MainWindow().Show();
            //new OrderForListWindow().Show();
            IEnumerable<BO.OrderTracking> orderTracking = p.Order.GetListOfTruckings();
            
           
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

        private void BtnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void BtnExit_Click(object sender, RoutedEventArgs e) { this.Close(); new NewOrderWindow(cart).Show(); }

        private void totalPrice_TextChanged()
        {

        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

