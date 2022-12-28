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
}