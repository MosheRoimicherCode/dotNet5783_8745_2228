namespace PL;
using BlApi;
using PL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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


public partial class NewOrderWindow : Window
{
    IBl? p = BlApi.Factory.Get();
    BO.Cart cart = new BO.Cart();

    private List<BO.Enums.Category> ListOfCategories = new();

    private List<BO.ProductItem> productItems;
    public List<BO.ProductItem> productItemsForUpdate
    {
        get { return productItems; }
        set
        {
            productItems = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("productForListUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public NewOrderWindow()
    {
        InitializeComponent();
        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
        {
            ListOfCategories.Add(item);
        }

        productItems = new List<BO.ProductItem>(p.Product.GetListOfItems(cart));
        DataContext = productItems;
        //ProductListview.ItemsSource = p.Product.GetList();
        CategorySelector.ItemsSource = ListOfCategories;
        CategorySelector.SelectedIndex = 3;
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
    {

        if (CategorySelector.SelectedItem is BO.Enums.Category categorySelected)
        {
            if (categorySelected == BO.Enums.Category.all) ProductListview.ItemsSource = new ObservableCollection<BO.ProductForList>(p.Product.GetList());

            else ProductListview.ItemsSource = new ObservableCollection<BO.ProductForList>(p.Product.GetList()).Where(x => x.Category == categorySelected);

            for (int i = 0; i < ListOfCategories.Count; i++)
                if (ListOfCategories[i].Equals(categorySelected)) ListOfCategories.Remove(ListOfCategories[i]);


            this.CategorySelector.ItemsSource = new ObservableCollection<BO.Enums.Category>(ListOfCategories);
            ListOfCategories.Clear();
            foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
                ListOfCategories.Add(item);
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //new ProductWindow("add", 0).Show();
        this.Close();
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int? id = ((BO.ProductForList)ProductListview.SelectedItem).ID;
        //new ProductWindow("update", (int)id).Show();
        this.Close();
    }
}
