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
    /// Interaction logic for truckingWindow.xaml
    /// </summary>
    public partial class TruckingWindow : Window
    {
        //List<Tuple<DateTime?, string?>?>? TupleList;
        public TruckingWindow(BO.OrderTracking orderTracking)
        {
            InitializeComponent();
            TrackingView.ItemsSource = orderTracking.TupleList;
            this.DataContext = orderTracking.TupleList;
           
        }
        
    }
}
