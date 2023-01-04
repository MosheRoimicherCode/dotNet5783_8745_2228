﻿using System;
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
public partial class ProductInCartWindow : Window
{
    IBl? p = BlApi.Factory.Get();
    BO.ProductItem productItem;
    BO.Cart currentCart;
    int id;
    int AmountItems;
    public ProductInCartWindow(int ID, BO.Cart cart)
    {
        InitializeComponent();
        productItem = p.Product.Get(ID, cart);

        Name2.Content = productItem.Name;
        ID2.Content = productItem.ID;
        Price2.Content = productItem.Price;
        Category2.Content = productItem.Category;
        IsInStock2.Content = productItem.IsInStock;
        AmontInCart2.Content = productItem.AmontInCart;

        currentCart = cart;
        id = ID;

    }

    private void Change_button(object sender, RoutedEventArgs e)
    {
        AmountItems = int.Parse(Amount.Text);
        p.Cart.UpdateAmount(currentCart, id, AmountItems);
        new CartWindow(currentCart).Show();
        //new NewOrderWindow(currentCart).Show();
        this.Close();
    }

    private void Remove_button(object sender, RoutedEventArgs e)
    {
        p.Cart.UpdateAmount(currentCart, id, 0);
        new CartWindow(currentCart).Show();
        //new NewOrderWindow(currentCart).Show();
        this.Close();
    }
}
