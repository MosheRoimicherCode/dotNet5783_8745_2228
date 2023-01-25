using System;
using System.Collections.Generic;
using System.Linq;
namespace PL;

using BlApi;
using BO;
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


/// <summary>
/// Interaction logic for ProductItemWindow.xaml
/// </summary>
public partial class ProductItemWindow : Window
{
    IBl? p = BlApi.Factory.Get();
    BO.ProductItem productItem;
    BO.Cart currentCart;
    int id;
    int AmountItems;
    readonly Action change;
    NewOrderWindow? newOrderWindow;
    public ProductItemWindow(int ID, BO.Cart cart, Action? action = null, NewOrderWindow? window = null)
    {
        InitializeComponent();
        productItem = p.Product.Get(ID, cart);

        change = action;

        Name2.Content = productItem.Name;
        ID2.Content = productItem.ID;
        Price2.Content = productItem.Price;
        Category2.Content = productItem.Category;
        IsInStock2.Content = productItem.IsInStock;
        AmontInCart2.Content = productItem.AmontInCart;

        newOrderWindow = window;

        currentCart = cart;
        id = ID;
        this.newOrderWindow = newOrderWindow;
    }

    private void add_Button_Click(object sender, RoutedEventArgs e)
    {
        AmountItems = int.Parse(Amount.Text);
        try
        {
            for (int i = 0; i < AmountItems; i++)
            {
                currentCart = p?.Cart.Add(currentCart, id)!;
            }
            newOrderWindow?.Close();
            new NewOrderWindow(currentCart).Show();
        }
        catch (Exception s)
        {
            newOrderWindow?.Close();
            new ERRORWindow(this, s.Message, currentCart).Show();
        }
        //change();
        //new CartWindow(cart1,id).Show();
        
        this.Close();
    }
}
