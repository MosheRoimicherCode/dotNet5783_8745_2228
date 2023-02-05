namespace PL;

using Sim;
using System.ComponentModel;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window 
{
    bool sim = true;
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Manager_Button_Click(object sender, RoutedEventArgs e) { new ManagerWindow().Show(); }
    private void Add_Order_Button_Click(object sender, RoutedEventArgs e) { new NewOrderWindow().Show(); }
    private void Order_Tracking_Button_Click(object sender, RoutedEventArgs e) { new OrderTrackingWindow().Show(); }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized;}
    private void BtnExit_Click(object sender, RoutedEventArgs e) { this.Close(); }
    private void SimulatorClick(object sender, RoutedEventArgs e)
    {
        if (!sim)
        {
            new ERRORWindow("The simulator cannot be run again!").Show();
        }
        else
        {
            new SimulatorWindow().Show();
            sim = false;
        }
    }

}