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
    /// Interaction logic for ProductForListWindow.xaml
    /// </summary>
    public partial class ProductForListWindow : Window
    { 
        Bl p = new Bl();
        //Func<BO.Enums.Category, bool> CheckCategory = (p) => p.Category == DO.Enums.Category.footwear;
        
        public ProductForListWindow()
        {
            InitializeComponent();
            
            
            ProductListview.ItemsSource = p.BoProduct.GetList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Enums.Category));

            //ProductListview.ItemsSource = p.BoProduct.GetList(CategorySelector.ItemsSource, true);
            //CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.BoEnums));


        }


    }
}
