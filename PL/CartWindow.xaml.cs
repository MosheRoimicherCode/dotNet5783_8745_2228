namespace PL; 
using BlApi;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


/// <summary>
/// Interaction logic for CartWindow.xaml
/// </summary>
public partial class CartWindow : Window
{
    IBl p = Factory.Get();
    private BO.Cart CurrentCart = new();
    private IEnumerable<BO.OrderItem?> orderItemFromCart;

    public IEnumerable<BO.OrderItem> orderItemFromCartUpdate
    {
        get => orderItemFromCart!;
        set
        {
            orderItemFromCart = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("orderItemFromCartUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public CartWindow(BO.Cart cart, int Productid)
    {
        InitializeComponent();
        CurrentCart = cart;

        _ = p.Cart.Add(CurrentCart, Productid);

        orderItemFromCart = cart.Details.Select(x => x);
        orderItemFromCartUpdate = orderItemFromCart;
        this.DataContext = orderItemFromCartUpdate;
    }

    private void SaveCartClick(object sender, RoutedEventArgs e)
    {
        CurrentCart.CustomerName = UserName.Text;
        CurrentCart.CustomeAdress = UserAddress.Text; 
        CurrentCart.CustomerEmail = UserEmail.Text;

    }

}
