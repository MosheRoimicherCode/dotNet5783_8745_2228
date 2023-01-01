namespace PL;

using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void Manager_Button_Click(object sender, RoutedEventArgs e) { new ManagerWindow().Show(); }
    private void Add_Order_Button_Click(object sender, RoutedEventArgs e) { new NewOrderWindow().Show(); }
    private void Order_Tracking_Button_Click(object sender, RoutedEventArgs e) { new OrderTrackingWindow().Show(); }

}