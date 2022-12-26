namespace PL;

using BlApi;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    IBl p = Factory.Get();
    string situation;
    private List<BO.Enums.Category> ListOfCategories = new();

    public ProductWindow(string str, int id)
    {
        InitializeComponent();

        BO.Product? productGeted = new();

        if (str == "update")
        {
            productGeted = p.Product.Get(id);

            buttonProductWindows.Content = str;
            ProducId.Text = (productGeted.ID).ToString();
            ProducId.IsReadOnly = true;
            ProducId.IsEnabled = false;
            ProductName.Text = productGeted.Name;
            ProductPrice.Text = productGeted.Price.ToString();
            ProductInStock.Text = productGeted.InStock.ToString();
            Category2.Text = productGeted.Category.ToString();
        }
        
        situation = str;
        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
        {
            ListOfCategories.Add(item);
            //ListOfCategoriesString.Add(item.ToString());
        }
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

                //insert value to category
            foreach (var item in ListOfCategories)
            {
                if (productCategory == item.ToString())
                    newProduct.Category = (BO.Enums.Category)Category2.SelectedItem;
            }
            if (btn.Name == "buttonProductWindows") //else just go out - cancel button
            {
                if (situation == "add") p.Product.Add(newProduct);
                else if (situation == "update") p.Product.Update(newProduct);
            }
            new ProductForListWindow().Show();
            this.Close();
        }
        
        catch(Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }
        
        
    }
    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        new ProductForListWindow().Show();
        this.Close();
    }
}