namespace PL;
using BlApi;
using PL.Order;
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

public partial class AddProductsWindow : Window
{
    static readonly IBl bl = Factory.Get();

    private BO.Cart currentCart = new();

    public static readonly DependencyProperty Products = DependencyProperty.Register(nameof(items),
                                                                                        typeof(IEnumerable<BO.ProductItem>),
                                                                                        typeof(AddProductsWindow));
    private IEnumerable<BO.ProductItem> items
    {
        get => (IEnumerable<BO.ProductItem>)GetValue(Products);
        set => SetValue(Products, value);
    }

    public static readonly DependencyProperty CategoryDep = DependencyProperty.Register(nameof(Category),
                                                                                        typeof(BO.Category),
                                                                                        typeof(AddProductsWindow));
    private BO.Category Category
    {
        get => (BO.Category)GetValue(CategoryDep);
        set => SetValue(CategoryDep, value);
    }

    public AddProductsWindow(BO.Order order)
    {
        Category = BO.Category.all;
        items = bl.Product.GetListOfItems(currentCart);
        InitializeComponent();
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e) => onChange();

    private void onChange()
    {
        if (Category == BO.Category.all) items = bl.Product.GetListOfItems(currentCart);
        else items = bl.Product.GetListOfItems(currentCart, x => x!.Category == Category);

    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.ProductItem select = ((BO.ProductItem)listOfProducts.SelectedItem);
        int id = select.ID;
        currentCart.TotalPrice = 0;

        new AddWindow(id, currentCart, onChange).Show();

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
