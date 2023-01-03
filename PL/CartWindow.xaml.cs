namespace PL; 
using BlApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;


/// <summary>
/// Interaction logic for CartWindow.xaml
/// </summary>
public partial class CartWindow : Window
{
    public static IBl p = Factory.Get();
    IEnumerable<BO.OrderItem>? orderItemFromCart;
    public IEnumerable<BO.OrderItem> orderItemFromCartUpdate
    {
        get => orderItemFromCart;
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
        string CustomerName  = UserName.Text;
        string CustomeAdress = UserAddress.Text;
        string CustomerEmail = UserEmail.Text;

        cart.CustomerName = CustomerName;
        cart.CustomeAdress = CustomeAdress;
        cart.CustomerEmail = CustomerEmail;
        _ = p.Cart.Add(cart, Productid);

        orderItemFromCartUpdate = cart.Details.Select(x => x)!; /*?? new ERRORWindow(this, "EMpry cart");*/
        this.DataContext = orderItemFromCartUpdate;
    }
}
