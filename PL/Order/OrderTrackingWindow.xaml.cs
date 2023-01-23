namespace PL;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using BlApi;


/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class OrderTrackingWindow : Window
{

    public static readonly DependencyProperty ordersDep = DependencyProperty.Register(nameof(orders),
                                                                                       typeof(IEnumerable<BO.OrderTracking>),
                                                                                       typeof(OrderTrackingWindow));

    private IEnumerable<BO.OrderTracking> orders
    {
        get => (IEnumerable<BO.OrderTracking>)GetValue(ordersDep);
        set => SetValue(ordersDep, value);
    }


    IBl p = BlApi.Factory.Get();

    IEnumerable<BO.OrderTracking>? orderTracking;

    public OrderTrackingWindow()
    {
        InitializeComponent();

        orders = p.Order.GetListOfTruckings();

        
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.OrderForList orderForList = new();
        BO.OrderTracking orderTracking = ((BO.OrderTracking)orderTrackingView.SelectedItem);
        new TruckingWindow(orderTracking).Show();
        //this.Close();
    }

}