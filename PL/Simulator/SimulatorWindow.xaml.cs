namespace PL;

using BlApi;
using System.Windows;
public partial class SimulatorWindow : Window
{
    static readonly IBl bl = BlApi.Factory.Get();
    public SimulatorWindow()
    {
        InitializeComponent();
    }

    private void StartSimulation(object sender, RoutedEventArgs e)
    {
        
    }
}
