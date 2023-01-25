namespace PL;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BlApi;

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window
{
    static readonly IBl bl = Factory.Get();
    public static BO.Category[] ListOfCategories { get; } = (BO.Category[])Enum.GetValues(typeof(BO.Category));

    public static readonly DependencyProperty ProductsDep = DependencyProperty.Register(nameof(products),
                                                                                        typeof(IEnumerable<BO.ProductForList>),
                                                                                        typeof(ProductForListWindow));
    private IEnumerable<BO.ProductForList> products
    {
        get => (IEnumerable<BO.ProductForList>)GetValue(ProductsDep);
        set => SetValue(ProductsDep, value);
    }

    public static readonly DependencyProperty CategoryDep = DependencyProperty.Register(nameof(Category),
                                                                                        typeof(BO.Category),
                                                                                        typeof(ProductForListWindow));
    private BO.Category Category
    {
        get => (BO.Category)GetValue(CategoryDep);
        set => SetValue(CategoryDep, value);
    }

    public ProductForListWindow()
    {
        Category = BO.Category.all;
        products = bl.Product.GetList();
        InitializeComponent();
    }

    public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e) => onChange();

    private void onChange()
    {
        if (Category == BO.Category.all) products = bl.Product.GetList();
        else products = bl.Product.GetList(x => x!.Category == Category);
    }

    private void Button_Click(object sender, RoutedEventArgs e) => new ProductWindow("add", 0, onChange).Show();

    private new void MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        int id = ((BO.ProductForList)(sender as ListViewItem)!.DataContext).ID;
        new ProductWindow("update", id, onChange, this).Show();
    }
}