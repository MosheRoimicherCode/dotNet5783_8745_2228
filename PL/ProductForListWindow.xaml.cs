namespace PL;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BlApi;

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window, INotifyPropertyChanged
{
    IBl? p = BlApi.Factory.Get();
    private List<BO.Enums.Category> ListOfCategories = new();
    private IEnumerable<BO.ProductForList> productForList;
    
    public IEnumerable<BO.ProductForList> productForListUpdate
    {
        get { return productForList; }
        set
        {
            productForList = value;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("productForListUpdate"));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ProductForListWindow()
    {
       
        InitializeComponent();
        this.DataContext = productForListUpdate;
        productForList = new List<BO.ProductForList>(p.Product.GetList());
        foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
        {
            ListOfCategories.Add(item);
        }

        CategorySelector.ItemsSource = ListOfCategories;
        CategorySelector.SelectedIndex = 3;
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
    {

        if (CategorySelector.SelectedItem is BO.Enums.Category categorySelected)
        {
            if (categorySelected == BO.Enums.Category.all) ProductListview.ItemsSource = productForListUpdate;

            else ProductListview.ItemsSource = productForListUpdate.Where(x => x.Category == categorySelected);

            for (int i = 0; i < ListOfCategories.Count; i++)
                if (ListOfCategories[i].Equals(categorySelected)) ListOfCategories.Remove(ListOfCategories[i]);


            this.CategorySelector.ItemsSource = new List<BO.Enums.Category>(ListOfCategories);
            ListOfCategories.Clear();
            foreach (BO.Enums.Category item in Enum.GetValues(typeof(BO.Enums.Category)))
                ListOfCategories.Add(item);
        }
    }
    private void onChange()
    {
        productForListUpdate = p.Product.GetList();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow("add", 0, onChange).Show();
        this.Close();
    }

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int? id = ((BO.ProductForList)ProductListview.SelectedItem).ID;
        new ProductWindow("update", (int)id, onChange).Show();
        this.Close();
    }
}