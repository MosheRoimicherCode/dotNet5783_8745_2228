namespace PL;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BlApi;


/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class OrderForListWindow : Window
{
    IBl? p = BlApi.Factory.Get();

    IEnumerable<BO.OrderForList?> orderForList;

    public OrderForListWindow()
    {
        InitializeComponent();

        orderForList = p.Order.GetList();
        
        OrderListview.ItemsSource = p.Order.GetList();
    }

    private void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.OrderForList orderForList = new();
        int OrderId = ((BO.OrderForList)OrderListview.SelectedItem).ID;
        new OrderWindow(OrderId).Show();
        this.Close();
    }

}