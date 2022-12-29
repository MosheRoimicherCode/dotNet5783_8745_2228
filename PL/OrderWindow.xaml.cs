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

    private void update_shiping_Button_Click(object sender, RoutedEventArgs e)
    {

    }

    private void update_providing_Button_Click(object sender, RoutedEventArgs e)
    {

    }

}