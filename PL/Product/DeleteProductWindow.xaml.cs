using BlApi;
using System.Windows;

namespace PL.Product;


/// <summary>
/// Interaction logic for deleteOrderWindow.xaml
/// </summary>
public partial class DeleteProductWindow : Window
{
    IBl p = Factory.Get();
    int id;
    ProductForListWindow productForListWindow;
    ProductWindow productWindow;
    public DeleteProductWindow(int ID, ProductForListWindow window1, ProductWindow window2)
    {
        InitializeComponent();
        id = ID;
        productForListWindow = window1;
        productWindow = window2;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (p.Product.ProductExistInsideOrders(id) == false)
        {
            p.Product.Delete(id);
            productForListWindow.Close();
            productWindow.Close();
            this.Close();
            new ProductForListWindow().Show();
        }
        else
        {
            MessageBox.Show("product inside older order. cant delete product.");
            productForListWindow.Close();
            productWindow.Close();
            this.Close();
        }
    }
}


