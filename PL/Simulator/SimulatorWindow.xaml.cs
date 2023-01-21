namespace PL;

using BlApi;
using Sim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;

public partial class SimulatorWindow : Window
{
    private Stopwatch stopWatch;
    private bool isTimerRun;
    BackgroundWorker timerworker;

    public static readonly DependencyProperty OrderDep = DependencyProperty.Register(nameof(Order),
                                                                                        typeof(IEnumerable<BO.Order>),
                                                                                        typeof(Window));
    private BO.Order Order
    {
        get => (BO.Order)GetValue(OrderDep);
        set => SetValue(OrderDep, value);
    }

    public SimulatorWindow()
    {
        Simulator.RegisterToUpdateEvent(OrderChanged);
        Simulator.RegisterToStopEvent(StopSimalation);
        Simulator.RegisterToTimeEvent(TimeChange);

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


            Simulator.StartSimulator(); //start order simulator
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

    private void OrderChanged(BO.Order order)
    {
        Dispatcher.Invoke(() => { Order = order; } );
    }

    private void StopSimalation(EventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            IDOrderInProgress.Content = "Finish Orders";
            OldStatus.Content = "Finish Orders";
            StartTime.Content = "Finish Orders";
            FutureStatus.Content = "Finish Orders";
            StopTime.Content = "Finish Orders";
        }); 
    }
    private void TimeChange(DateTime date)
    {
        Dispatcher.Invoke(() => StopTime.Content = date.ToString());
    }
}
