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
public partial class OrderForListWindow : Window, INotifyPropertyChanged
{
    IBl p = BlApi.Factory.Get();

    IEnumerable<BO.OrderForList> orderForList;

    public IEnumerable<BO.OrderForList> orderForListForUpdate
    {
        get { return orderForList; }
        set
        {
            orderForList = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("orderForListForUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public OrderForListWindow()
    {
        InitializeComponent();

        orderForListForUpdate = p.Order.GetList();
        this.DataContext = orderForListForUpdate;
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.OrderForList orderForList = new();
        int OrderId = ((BO.OrderForList)OrderListview.SelectedItem).ID;
        new OrderWindow(OrderId).Show();
        this.Close();
    }

}