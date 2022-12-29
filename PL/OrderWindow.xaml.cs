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
    public OrderWindow(int id)
    {
        InitializeComponent();
        ID = id;
    }

    private void update_shiping_Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            p.Order.UpdateShipping(ID);
            new OrderForListWindow().Show();
            this.Close();
        }
        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }

    }

    private void update_providing_Button_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            p.Order.UpdateProviding(ID);
            new OrderForListWindow().Show();
            this.Close();
        }
        catch (Exception s)
        {
            new ERRORWindow(this, s.Message).Show();
        }
    }

}