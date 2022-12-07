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
        string situation;
        private List<BO.Enums.Category> ListOfCategories = new();

        public ProductWindow(string str, int id)
        {
            InitializeComponent();

            BO.Product? productGeted = new();

            if (str == "update")
            {
                productGeted = p.Product.Get(id);

                buttonProductWindows.Content = str;
                ProducId.Text = (productGeted.ID).ToString();
                ProducId.IsReadOnly = true;
                ProducId.IsEnabled = false;
                ProductName.Text = productGeted.Name;
                ProductPrice.Text = productGeted.Price.ToString();
                ProductInStock.Text = productGeted.InStock.ToString();
                Category2.Text = productGeted.Category.ToString();
            }
            
            situation = str;
            foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
            {
                ListOfCategories.Add(item);
                //ListOfCategoriesString.Add(item.ToString());
            }
            Category2.ItemsSource = ListOfCategories;
            ListOfCategories.Remove(BO.Enums.Category.all);
            Category2.SelectedIndex = -1;

            if (str == "update")
            {
                //define defaul comboBox stand
                if (productGeted.Category.ToString() == "footwear") Category2.SelectedIndex = 0;
                if (productGeted.Category.ToString() == "outerwear") Category2.SelectedIndex = 1;
                if (productGeted.Category.ToString() == "business") Category2.SelectedIndex = 2;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            
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
            if (btn.Name == "buttonProductWindows")
            {
                if (situation == "add") p.Product.Add(newProduct);
                else if (situation == "update") p.Product.Update(newProduct);
            }
            new ProductForListWindow().Show();
            this.Close();
        }
    }
}