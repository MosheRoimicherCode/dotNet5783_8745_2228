using BlImplementation;
using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;


namespace PL
{
    /// <summary>
    /// Interaction logic for ProductForListWindow.xaml
    /// </summary>
    public partial class ProductForListWindow : Window
    {
        Bl p = new Bl();
        //Func<BO.Enums.Category, bool> CheckCategory = (p) => p.Category == DO.Enums.Category.footwear;
        IEnumerable<BO.ProductForList> productForList;
        static int select = 0;

        public ProductForListWindow()
        {
            InitializeComponent();
            productForList = p.Product.GetList();
            ProductListview.ItemsSource = productForList;
            CategorySelector.Items.Add(BO.Enums.Category.footwear);
            CategorySelector.Items.Add(BO.Enums.Category.business);
            CategorySelector.Items.Add(BO.Enums.Category.outerwear);

            //CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));
            CategorySelector.SelectedIndex = -1;
        }

        public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (CategorySelector.SelectedItem is BO.Enums.Category c)
            {
                CategorySelector.Items.Clear();
                CategorySelector.Items.Add(BO.Enums.Category.footwear);
                CategorySelector.Items.Add(BO.Enums.Category.business);
                CategorySelector.Items.Add(BO.Enums.Category.outerwear);
                CategorySelector.Items.Add("all");
                ProductListview.ItemsSource = p.Product.GetList(item => item.Category == c);
                CategorySelector.Items.Remove(c);
            }
            if (CategorySelector.SelectedItem is "all")
            {
                CategorySelector.Items.Clear();
                CategorySelector.Items.Add(BO.Enums.Category.footwear);
                CategorySelector.Items.Add(BO.Enums.Category.business);
                CategorySelector.Items.Add(BO.Enums.Category.outerwear);     
                ProductListview.ItemsSource = p.Product.GetList();
                CategorySelector.Items.Remove("all");
            }

        }

    }
}