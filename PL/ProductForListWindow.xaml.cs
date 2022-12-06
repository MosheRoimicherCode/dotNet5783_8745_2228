using BlImplementation;
using System;
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