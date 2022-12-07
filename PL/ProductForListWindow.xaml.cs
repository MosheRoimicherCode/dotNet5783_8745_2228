using BlImplementation;
using BO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

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
        private List<BO.Enums.Category> ListOfCategories = new();
        //private List<string> ListOfCategoriesString = new();
        BO.Enums.Category all = new();
        
        public ProductForListWindow()
        {
            InitializeComponent();

            ProductListview.ItemsSource = p.Product.GetList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));

        }

        public void CategorySelector_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ProductListview.ItemsSource = p.Product.GetList(item => item.Category == (BO.Enums.Category)CategorySelector.SelectedValue);
        }

    }
}