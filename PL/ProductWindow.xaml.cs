using BlImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        Bl p = new Bl();

        private List<BO.Enums.Category> ListOfCategories = new();

        public ProductWindow()
        {
            InitializeComponent();
            foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
            {
                ListOfCategories.Add(item);
                //ListOfCategoriesString.Add(item.ToString());
            }
            Category2.ItemsSource = ListOfCategories;
            ListOfCategories.Remove(BO.Enums.Category.all);
            Category2.SelectedIndex = -1;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string productID = ProducId.Text;
            string productName = ProductName.Text;
            string productPrice = ProductPrice.Text;
            string productCategory = Category2.SelectedItem?.ToString() ?? "null";
            string productInStock = ProductInStock.Text;

            BO.Product newProduct = new()
            {
                ID = int.Parse(productID),
                Name = productName,
                Price = double.Parse(productPrice),
                Category = null,
                InStock = int.Parse(productInStock),
            };
            //insert value to category
            foreach (var item in ListOfCategories)
            {
                if (productCategory == item.ToString())
                    newProduct.Category = (BO.Enums.Category)Category2.SelectedItem;
            }

            p.Product.Add(newProduct);

            new ProductForListWindow().Show();
            this.Close();
        }


    }
}