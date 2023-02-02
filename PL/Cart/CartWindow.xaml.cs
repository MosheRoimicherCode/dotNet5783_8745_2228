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
        private int orderId;
        private IEnumerable<BO.ProductItem>?productItemcartList;
        readonly Action change;
        bool manager = false;

        public static readonly DependencyProperty ProductsDep = DependencyProperty.Register(nameof(productItemcartListUpdate2),
                                                                                       typeof(IEnumerable<BO.ProductItem>),
                                                                                       typeof(CartWindow));
        private IEnumerable<BO.ProductItem> productItemcartListUpdate2
        {
            get => (IEnumerable<BO.ProductItem>)GetValue(ProductsDep);
            set => SetValue(ProductsDep, value);
        }


        //public IEnumerable<BO.ProductItem> productItemcartListUpdate
        //{
        //    get => productItemcartList;
        //    set
        //    {
        //        productItemcartList = value;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("productItemcartListUpdate"));
        //        }
        //    }
        //}
        //public event PropertyChangedEventHandler? PropertyChanged;

        public CartWindow(BO.Cart currentCart, int id, Action? action = null)
        {
            InitializeComponent();
            productItemcartListUpdate2 = p.Product.GetListOfItemsInCart(currentCart);
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
            this.DataContext = productItemcartListUpdate2;

            change = action;
            cart = currentCart;
            TotalPriceCart.Content = currentCart.TotalPrice.ToString();
            orderId = id;

            if (currentCart.CustomerName != null) { UserName.Text = currentCart.CustomerName; }
            if (currentCart.CustomerEmail != null) { UserEmail.Text = currentCart.CustomerEmail; }
            if (currentCart.CustomeAdress != null) { UserAddress.Text = currentCart.CustomeAdress; }

            if (currentCart.CustomerName != null)
            {
                ConfirmOrder.Content = "change order";
                manager = true;
            }
        }


        private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int? id = ((BO.ProductItem)CartList.SelectedItem).ID;
            new ProductInCartWindow((int)id, cart, this).Show();
        }

        private void Confirm_Order(object sender, RoutedEventArgs e)
        {
            try
            {
                if (manager == true)
                {
                    p.Order.Delete(orderId);
                }
                cart.CustomerName = UserName.Text;
                cart.CustomeAdress = UserAddress.Text;
                cart.CustomerEmail = UserEmail.Text;
                p.Cart.ConfirmCart(cart, cart.CustomerName!, cart.CustomerEmail!, cart.CustomeAdress!, orderId);
                this.Close();
            }
            catch (Exception s)
            {
                new ERRORWindow(this, s.Message).Show();
            }
            
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


        private void UserName_MouseEnter(object sender, MouseEventArgs e)
        {
            if (UserName.Text == "User Name") UserName.Text = "";
        }

        private void UserName_MouseLeave(object sender, MouseEventArgs e)
        {
            if (UserName.Text == "") UserName.Text = "User Name";
        }


        private void UserName_MouseEnter1(object sender, MouseEventArgs e)
        {
            if (UserAddress.Text == "User Address")  UserAddress.Text = "";
        }

        private void UserName_MouseLeave1(object sender, MouseEventArgs e)
        {
            if (UserAddress.Text == "") UserAddress.Text = "User Address";
        }

        private void UserName_MouseEnter2(object sender, MouseEventArgs e)
        {
            if (UserEmail.Text == "User Email") UserEmail.Text = "";
        }

        private void UserName_MouseLeave2(object sender, MouseEventArgs e)
        {
            if (UserEmail.Text == "") UserEmail.Text = "User Email";
        }
    }
}

