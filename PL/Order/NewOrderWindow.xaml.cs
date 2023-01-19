namespace PL;
using BlApi;
using PL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
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

public partial class NewOrderWindow : Window 
{
    static readonly IBl bl = Factory.Get();

    private BO.Cart currentCart = new();

    public static readonly DependencyProperty ProductsDep = DependencyProperty.Register(nameof(products),
                                                                                        typeof(IEnumerable<BO.ProductItem>),
                                                                                        typeof(NewOrderWindow));
    private IEnumerable<BO.ProductItem> products
    {
        get => (IEnumerable<BO.ProductItem>)GetValue(ProductsDep);
        set => SetValue(ProductsDep, value);
    }

    public static readonly DependencyProperty CategoryDep = DependencyProperty.Register(nameof(Category),
                                                                                        typeof(BO.Category),
                                                                                        typeof(NewOrderWindow));
    private BO.Category Category
    {
        get => (BO.Category)GetValue(CategoryDep);
        set => SetValue(CategoryDep, value);
    }

    public NewOrderWindow(BO.Cart? cart = null)
    {
        Category = BO.Category.all;
        if (cart != null)
        {
            currentCart = cart;
        }
        
        products = bl.Product.GetListOfItems(currentCart);
        InitializeComponent();
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e) => onChange();

    private void onChange()
    {
        if (Category == BO.Category.all) products = bl.Product.GetListOfItems(currentCart);
        else products = bl.Product.GetListOfItems(currentCart, x => x!.Category == Category);
        
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.ProductItem select = ((BO.ProductItem)listOfProducts.SelectedItem);
        //BO.ProductItem select = (BO.ProductItem)(sender as ListViewItem)!.DataContext;
        int id = select.ID;

        currentCart.CustomerName = User_name.Text;
        currentCart.CustomeAdress = User_adress.Text;
        currentCart.CustomerEmail = User_email.Text;
        currentCart.TotalPrice = 0;
        
        new ProductItemWindow(id, currentCart, onChange).Show();
        Close();
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new GroupingWindow().Show();
        this.Close();
    }

    private void cart_Button_Click(object sender, RoutedEventArgs e)
    {
        new CartWindow(currentCart, onChange).Show();
        this.Close();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
    private void btnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }
}
