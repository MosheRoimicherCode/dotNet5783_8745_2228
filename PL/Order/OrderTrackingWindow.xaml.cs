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
public partial class OrderTrackingWindow : Window, INotifyPropertyChanged
{
    IBl p = BlApi.Factory.Get();

    IEnumerable<BO.OrderTracking>? orderTracking;

    public IEnumerable<BO.OrderTracking> orderTrackingUpdate
    {
        get { return orderTracking; }
        set
        {
            orderTracking = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("orderTrackingUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public OrderTrackingWindow()
    {
        InitializeComponent();
        this.DataContext = orderTrackingUpdate;

        orderTrackingUpdate = p.Order.GetListOfTruckings();
        orderTrackingView.ItemsSource = orderTrackingUpdate;

    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.OrderForList orderForList = new();
        BO.OrderTracking orderTracking = ((BO.OrderTracking)orderTrackingView.SelectedItem);
        new TruckingWindow(orderTracking).Show();
        //this.Close();
    }

}