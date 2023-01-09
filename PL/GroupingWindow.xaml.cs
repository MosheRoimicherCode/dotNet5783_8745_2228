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

public partial class GroupingWindow : Window
{
    static readonly IBl bl = BlApi.Factory.Get();
    BO.Cart cart = new BO.Cart();

    private List<BO.Category> ListOfCategories = new();

    private List<BO.ProductItem> productItems;

    public List<BO.ProductItem> productItemsForUpdate
    {
        get { return productItems; }
        set
        {
            productItems = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("productItemsForUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    public GroupingWindow()
    {
        InitializeComponent();
        foreach (BO.Category item in Enum.GetValues(typeof(BO.Category)))
        {
            ListOfCategories.Add(item);
        }

        productItems = new List<BO.ProductItem>(bl.Product.GetListOfItems(cart));

        DataContext = productItems;

        CategorySelector.ItemsSource = ListOfCategories;
        CategorySelector.SelectedIndex = 3;

        ProductItemView.ItemsSource = productItems;

    }


    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
    {

        if (CategorySelector.SelectedItem is BO.Category categorySelected)
        {
            if (categorySelected == BO.Category.all) ProductItemView.ItemsSource = new List<BO.ProductItem>(bl.Product.GetListOfItems(cart));

            else ProductItemView.ItemsSource = new List<BO.ProductItem>(bl.Product.GetListOfItems(cart)).Where(x => x.Category == categorySelected);

            for (int i = 0; i < ListOfCategories.Count; i++)
                if (ListOfCategories[i].Equals(categorySelected)) ListOfCategories.Remove(ListOfCategories[i]);


            this.CategorySelector.ItemsSource = new ObservableCollection<BO.Category>(ListOfCategories);
            ListOfCategories.Clear();
            foreach (BO.Category item in Enum.GetValues(typeof(BO.Category)))
                ListOfCategories.Add(item);
        }
    }


    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int? id = ((BO.ProductItem)ProductItemView.SelectedItem).ID;
        new ProductItemWindow((int)id, cart).Show();

    }


    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new NewOrderWindow().Show();
        this.Close();
    }
}
