namespace PL;

using BlApi;
using PL.Order;
using PL.Product;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    IBl p = Factory.Get();
    int ID;
    OrderForListWindow orderForListWindow;
    BO.Order order;

    public OrderWindow(int id, OrderForListWindow window)
    {
        InitializeComponent();
        ID = id;
        orderForListWindow = window;
        
        order = p.Order.Get(id);
        List<BO.OrderItem>? orders = order.Details;
        OrderList.DataContext = orders;
    }

    private void update_shiping_Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            p.Order.UpdateShipping(ID);
            new OrderForListWindow().Show();
            orderForListWindow.Close();
            this.Close();
        }
        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
            Close();
        }

    }

    private void update_providing_Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            p.Order.UpdateProviding(ID);
            new OrderForListWindow().Show();
            orderForListWindow.Close();
            this.Close();
        }
        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
            Close();
        }
    }

    private void add_products_Button_Click(object sender, RoutedEventArgs e)
    {
        new AddProductsWindow(order).Show();
        //new NewOrderWindow(order).Show();
    }

    private void update_order_Button_Click(object sender, RoutedEventArgs e)
    {
        new AddProductsWindow(order).Show();
    }

    private void delete_order_Button_Click(object sender, RoutedEventArgs e)
    {
        new deleteOrderWindow(ID, orderForListWindow, this).Show();
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
    private void BtnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }

}