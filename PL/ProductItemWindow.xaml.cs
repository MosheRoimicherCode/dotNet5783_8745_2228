namespace PL;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BlApi;


/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductItemWindow : Window
{
    IBl? p = BlApi.Factory.Get();

    IEnumerable<BO.ProductItem> productItem;
    IEnumerable<BO.ProductForList> productForList;
    private List<BO.Enums.Category> ListOfCategories = new();
    BO.Enums.Category all = new();

    public ProductItemWindow()
    {
        InitializeComponent();

        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
        {
            ListOfCategories.Add(item);
        }

        productForList = p.Product.GetList();
        ProductListview.ItemsSource = p.Product.GetList();
        CategorySelector.ItemsSource = ListOfCategories;
        CategorySelector.SelectedIndex = 3;
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
    {

        if (CategorySelector.SelectedItem is BO.Enums.Category categorySelected)
        {
            if (categorySelected == BO.Enums.Category.all) ProductListview.ItemsSource = productForList;

            else ProductListview.ItemsSource = productForList.Where(x => x.Category == categorySelected);

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

        new ProductWindow("add", 0).Show();
        this.Close();
    }

    private void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        BO.ProductForList n = new();
        int id = ((BO.ProductForList)ProductListview.SelectedItem).ID;
        new ProductWindow("update", id).Show();
        this.Close();
    }
}