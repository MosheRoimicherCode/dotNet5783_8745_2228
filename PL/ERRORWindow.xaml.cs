namespace PL; 

using System.Windows;

/// <summary>
/// Interaction logic for ERRORWindow.xaml
/// </summary>
public partial class ERRORWindow : Window
{
    ProductWindow productWindow;
    OrderWindow orderWindow;
    string s;
    public ERRORWindow(ProductWindow Window, string s)
    {
        InitializeComponent();
        label.Content = s;
        productWindow = Window;
        s = "productWindow";
    }

    public ERRORWindow(OrderWindow Window, string s)
    {
        InitializeComponent();
        label.Content = s;
        orderWindow = Window;
        s = "orderWindow";
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (s == "productWindow")
        {
            productWindow.Show();
            this.Close();
        }
        else orderWindow.Show();
    }
}
