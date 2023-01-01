namespace PL;

using BlApi;
using BO;
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
    public static IBl productIBL = Factory.Get();
    string situation;
    private List<BO.Enums.Category> ListOfCategories = new();
    Action change;
    public ProductWindow(string str, int id, Action action)
    {
        InitializeComponent();
        change = action;    
        situation = str;
        BO.Product? productGeted = new();

        if (str == "update")
        {
            productGeted = productIBL.Product.Get(id);

            buttonProductWindows.Content = str;
            ProducId.Text = (productGeted.ID).ToString();
            ProducId.IsReadOnly = true;
            ProducId.IsEnabled = false;
            ProductName.Text = productGeted.Name;
            ProductPrice.Text = productGeted.Price.ToString();
            ProductInStock.Text = productGeted.InStock.ToString();
            Category2.Text = productGeted.Category.ToString();
        }
        
        
        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category))) ListOfCategories.Add(item);

        Category2.ItemsSource = ListOfCategories;
        ListOfCategories.Remove(BO.Enums.Category.all);
        Category2.SelectedIndex = -1;

        if (str == "update")
        {
            //define defaul comboBox stand
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
                    newProduct.Category = (BO.Enums.Category)Category2.SelectedItem;
            }
            if (btn.Name == "buttonProductWindows") //else just jump to cancel button
            {
                if (situation == "add")
                {
                    productIBL.Product.Add(newProduct);
                }
                else if (situation == "update") productIBL.Product.Update(newProduct);
            }
            change();
            //new ProductForListWindow().Show();         
            this.Close();
        }
        
        catch(Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }
        
        
    }
    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        //new ProductForListWindow().Show();
        this.Close();
    }

    private void buttonProductWindows_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == Key.Enter) {Button_Click(sender, e);}
    }
}