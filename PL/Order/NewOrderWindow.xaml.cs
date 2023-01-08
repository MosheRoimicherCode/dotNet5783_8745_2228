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

public partial class NewOrderWindow : Window ,INotifyPropertyChanged
{
    IBl? p = BlApi.Factory.Get();
    private BO.Cart currentCart = new();

    private List<BO.Enums.Category> ListOfCategories = new();

    private List<BO.ProductItem> productItems;
    public List<BO.ProductItem> productItemsForUpdate
    {
        get { return productItems; }
        set
        {
            productItems = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("productItemsForUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public NewOrderWindow(BO.Cart? cart = null)
    {
        InitializeComponent();
        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category))) ListOfCategories.Add(item);
        if (cart != null) currentCart = cart;
        productItemsForUpdate = new List<BO.ProductItem>(p.Product.GetListOfItems(currentCart));
        DataContext = productItemsForUpdate;
        CategorySelector.ItemsSource = ListOfCategories;
        CategorySelector.SelectedIndex = 3;
        ProductItemView.ItemsSource = productItemsForUpdate;
    }


    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
    {

        if (CategorySelector.SelectedItem is BO.Enums.Category categorySelected)
        {
            if (categorySelected == BO.Enums.Category.all) ProductItemView.ItemsSource = new List<BO.ProductItem>(p.Product.GetListOfItems(currentCart));

            else ProductItemView.ItemsSource = new List<BO.ProductItem>(p!.Product.GetListOfItems(currentCart)).Where(x => x.Category == categorySelected);

            for (int i = 0; i < ListOfCategories.Count; i++)
                if (ListOfCategories[i].Equals(categorySelected)) ListOfCategories.Remove(ListOfCategories[i]);


            this.CategorySelector.ItemsSource = new ObservableCollection<BO.Enums.Category>(ListOfCategories);
            ListOfCategories.Clear();
            foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
                ListOfCategories.Add(item);
        }
    }


    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int id = ((BO.ProductItem)ProductItemView.SelectedItem).ID;
        currentCart.CustomerName = User_name.Text;
        currentCart.CustomeAdress = User_adress.Text;
        currentCart.CustomerEmail = User_email.Text;
        currentCart.TotalPrice = 0;
        
        new ProductItemWindow(id, currentCart).Show();
        Close();
    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new GroupingWindow().Show();
        this.Close();
    }

    private void cart_Button_Click(object sender, RoutedEventArgs e)
    {
        new CartWindow(currentCart).Show();
        this.Close();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
    private void btnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }
}
