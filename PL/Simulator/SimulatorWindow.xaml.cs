namespace PL;

using BlApi;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
public partial class SimulatorWindow : Window
{
    private Stopwatch stopWatch;
    private bool isTimerRun;
    BackgroundWorker timerworker;

    static readonly IBl bl = BlApi.Factory.Get();
    public SimulatorWindow()
    {
        InitializeComponent();
        stopWatch = new Stopwatch();
        timerworker = new BackgroundWorker();
        timerworker.DoWork += Worker_DoWork;
        timerworker.ProgressChanged += Worker_ProgressChanged;
        timerworker.WorkerReportsProgress = true;
    }

    private void StartSimulation(object sender, RoutedEventArgs e)
    {
        if (!isTimerRun)
        {
            stopWatch.Start();
            isTimerRun = true;
            timerworker.RunWorkerAsync();



        }
    }

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        string timerText = stopWatch.Elapsed.ToString();
        timerText = timerText.Substring(0, 8);
        this.timerTextBlock.Text = timerText;
    }

    private void StopSimulation(object sender, RoutedEventArgs e)
    {
        if (isTimerRun)
        {
            stopWatch.Stop();
            isTimerRun = false;
        }
    }

    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        while (isTimerRun)
        {
            timerworker.ReportProgress(231);
            Thread.Sleep(1000);
        }
    }
}
