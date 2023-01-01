namespace PL;

using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class ManagerWindow : Window
{
    public ManagerWindow()
    {
        InitializeComponent();
    }
    private void Product_List_Button_Click(object sender, RoutedEventArgs e) { new ProductForListWindow().Show(); }
    private void Order_List_Button_Click(object sender, RoutedEventArgs e) { new OrderForListWindow().Show();  }
   
}