namespace PL;

using BlApi;
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
    public OrderWindow(int id, OrderForListWindow window)
    {
        InitializeComponent();
        ID = id;
        orderForListWindow = window;
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
    private void BtnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
    private void BtnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }

}