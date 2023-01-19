namespace PL;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlApi;


/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class OrderForListWindow : Window
{
    static readonly IBl bl = Factory.Get();

    public static readonly DependencyProperty ordersDep = DependencyProperty.Register(nameof(orders),
                                                                                        typeof(IEnumerable<BO.OrderForList>),
                                                                                        typeof(OrderForListWindow));

    private IEnumerable<BO.OrderForList> orders
    {
        get => (IEnumerable<BO.OrderForList>)GetValue(ordersDep);
        set => SetValue(ordersDep, value);
    }

    public OrderForListWindow()
    {
        orders = bl.Order.GetList();
        InitializeComponent();
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int id = ((BO.OrderForList)(sender as ListViewItem)!.DataContext).ID;
        new OrderWindow(id).Show();
    }


    private void btnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
    private void btnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }
}
