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
    private void Product_Button_Click(object sender, RoutedEventArgs e) { new ProductForListWindow().Show(); this.Close(); }
    private void Order_Button_Click(object sender, RoutedEventArgs e) { new OrderForListWindow().Show(); this.Close(); }
}