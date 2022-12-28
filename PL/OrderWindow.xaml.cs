namespace PL;

using BlApi;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    IBl p = Factory.Get();
   
    public OrderWindow(int id)
    {
        InitializeComponent();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var btn = e.OriginalSource as Button;
            if (ID.Text == "") throw new Exception("ID can not be null!");
            string OrderID = ID.Text;
            if (CustomerName.Text == "") throw new Exception("name can not be null!");
            string OrderCustomerName = CustomerName.Text;
            if (OrderStatus.Text == "") throw new Exception("status can not be null!");
            string orderStatus = OrderStatus.Text;
            if (Amount.Text == "") throw new Exception("amount can not be null!");
            string OrderAmount = Amount.Text;
            if (TotalPrice.Text == "") throw new Exception("total price can not be null!");
            string OrderTotalPrice = TotalPrice.Text;

            BO.Order newOrder = new()
            {
                ID = int.Parse(OrderID),
                CustomerName = OrderCustomerName,
                //OrderStatus = orderStatus,
                Amount = OrderAmount,
                InStock = int.Parse(productInStock),
            };

           
            new ProductForListWindow().Show();
            this.Close();
        }

        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
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

        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }
        