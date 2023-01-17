﻿namespace PL; 

using System.Windows;

/// <summary>
/// Interaction logic for ERRORWindow.xaml
/// </summary>
public partial class ERRORWindow : Window
{
    ProductWindow? productWindow;
    OrderWindow? orderWindow;
    readonly string s;
    public ERRORWindow(ProductWindow Window, string s)
    {
        InitializeComponent();
        label.Content = s;
        productWindow = Window;
        this.s = "productWindow";
    }

    public ERRORWindow(OrderWindow Window, string s)
    {
        InitializeComponent();
        label.Content = s;
        orderWindow = Window;
        this.s = "orderWindow";
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        this.Close();
        if (s == "productWindow")
        {
            productWindow?.Show();
        }
        else orderWindow?.Show(); 
    }
}
