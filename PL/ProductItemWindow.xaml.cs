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
    BO.ProductForList productItem;
    BO.Cart cart1;
    int id;
    int AmountItems;
    public ProductItemWindow(int ID, BO.Cart cart)
    {
        InitializeComponent();
        productItem = p.Product.GetList(x => x.Value.ID == ID).First();
        
        Name2.Content = productItem.Name;
        ID2.Content = productItem.ID;
        Price2.Content = productItem.Price;
        Category2.Content = productItem.Category;
        //IsInStock2.Content = productItem.IsInStock;
        //AmontInCart2.Content = productItem.AmontInCart;

        cart1 = cart;
        id = ID;
        AmountItems = int.Parse(Amount.Text);
    }

    private void add_Button_Click(object sender, RoutedEventArgs e)
    {
        /*for (int i = 0; i < AmountItems; i++) */
        cart1 = p?.Cart.Add(cart1, id)!;
        new CartWindow(cart1,id).Show();
        this.Close();
    }
}
