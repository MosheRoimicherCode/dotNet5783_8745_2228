namespace PL; 

using System.Windows;

/// <summary>
/// Interaction logic for ERRORWindow.xaml
/// </summary>
public partial class ERRORWindow : Window
{
    ProductWindow productWindow;

    public ERRORWindow(ProductWindow Window, string s)
    {
        InitializeComponent();
        label.Content = s;
        productWindow = Window;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        productWindow.Show();
        this.Close();
    }
}
