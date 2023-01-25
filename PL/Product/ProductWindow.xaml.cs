namespace PL;

using BlApi;
using BO;
using PL.Order;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    static readonly IBl bl = Factory.Get();
    readonly string situation;
    readonly List<BO.Category> ListOfCategories = new();
    readonly Action change;
    ProductForListWindow ProductForListWindow;
    int ID;
    public ProductWindow(string str, int id, Action action, ProductForListWindow? window = null)
    {
        InitializeComponent();
        change = action;    
        situation = str;
        BO.Product? productGeted = new();

        ProductForListWindow = window;
        ID = id;

        if (str == "update")
        {
            productGeted = bl.Product.Get(id);

            buttonProductWindows.Content = str;
            ProducId.Text = (productGeted.ID).ToString();
            ProducId.IsReadOnly = true;
            ProducId.IsEnabled = false;
            ProductName.Text = productGeted.Name;
            ProductPrice.Text = productGeted.Price.ToString();
            ProductInStock.Text = productGeted.InStock.ToString();
            Category2.Text = productGeted.Category.ToString();
        }
        
        
        foreach (BO.Category item in Enum.GetValues(typeof(BO.Category))) ListOfCategories.Add(item);

        Category2.ItemsSource = ListOfCategories;
        ListOfCategories.Remove(BO.Category.all);
        Category2.SelectedIndex = -1;

        if (str == "update")
        {
            //define default comboBox stand
            if (productGeted.Category.ToString() == "footwear") Category2.SelectedIndex = 0;
            if (productGeted.Category.ToString() == "outerwear") Category2.SelectedIndex = 1;
            if (productGeted.Category.ToString() == "business") Category2.SelectedIndex = 2;
        }

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var btn = e.OriginalSource as Button;
            if (ProducId.Text == "") throw new Exception("ID can not be null!");
            string productID = ProducId.Text;
            if (ProductName.Text == "") throw new Exception("name can not be null!");
            string productName = ProductName.Text;
            if (ProductPrice.Text == "") throw new Exception("price can not be null!");
            string productPrice = ProductPrice.Text;
            if (Category2.SelectedItem?.ToString() == null) throw new Exception("category can not be null!");
            string productCategory = Category2.SelectedItem?.ToString() ?? "null";
            if (ProductInStock.Text == "") throw new Exception("instock can not be null!");
            string productInStock = ProductInStock.Text;

            BO.Product newProduct = new()
            {
                ID = int.Parse(productID),
                Name = productName ?? null,
                Price = double.Parse(productPrice),
                Category = null,
                InStock = int.Parse(productInStock),
            };

            
            foreach (var item in ListOfCategories)//insert value to category
            {
                if (productCategory == item.ToString())
                    newProduct.Category = (BO.Category)Category2.SelectedItem!;
            }
            if (btn?.Name == "buttonProductWindows") //else just jump to cancel button
            {
                if (situation == "add") bl.Product.Add(newProduct);
                else if (situation == "update") bl.Product.Update(newProduct);
            }
            change();
            this.Close();
        }
        
        catch(Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }
    }
    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void delete_Button_Click(object sender, RoutedEventArgs e)
    {
        new DeleteProductWindow(ID, ProductForListWindow, this).Show();
    }

    private void buttonProductWindows_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter) {Button_Click(sender, e);}
    }
}