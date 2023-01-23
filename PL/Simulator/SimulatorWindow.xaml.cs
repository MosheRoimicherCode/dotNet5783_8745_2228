namespace PL;

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
    BackgroundWorker worker;


    public static readonly DependencyProperty OrderDep = 
        DependencyProperty.Register(nameof(Order),typeof(IEnumerable<BO.Order>),typeof(SimulatorWindow));
    private BO.Order Order
    {
        get => (BO.Order)GetValue(OrderDep);
        set => SetValue(OrderDep, value);
    }
    

    public static readonly DependencyProperty MyClockProperty =
     DependencyProperty.Register(nameof(Clock), typeof(string), typeof(SimulatorWindow));
    public string Clock
    {
        get => (string)GetValue(MyClockProperty);
        set =>SetValue(MyClockProperty, value); 
    }
    

    public static readonly DependencyProperty MyProgressBarValueProperty =
        DependencyProperty.Register(nameof(ProgressBarValue), typeof(double), typeof(SimulatorWindow));

    public double ProgressBarValue
    {
        get { return (double)GetValue(MyProgressBarValueProperty); }
        set { SetValue(MyProgressBarValueProperty, value); }
    }




    public SimulatorWindow()
    {
        InitializeComponent();

        stopWatch = new Stopwatch();
        stopWatch.Start();
        Clock = stopWatch.Elapsed.ToString().Substring(0, 8);

        worker = new BackgroundWorker()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        worker.DoWork += Worker_DoWork;
        worker.ProgressChanged += Worker_ProgressChanged;
        //worker.RunWorkerCompleted += worker_RunWorkerCompleted;

        int a = 0;
    }

    //button command
    private void StartSimulation(object sender, RoutedEventArgs e)
    {
        if (!isTimerRun)
        {
            stopWatch.Start();
            isTimerRun = true;
            worker.RunWorkerAsync(); //call DOWork function
        }
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        Simulator.RegisterChangeOrder(UpdateWindow);
        //Simulator.RegisterEstimatedTime(UpdateEstimatedTime);
        //Simulator.RegisterCompletedSimulation(FinishSimulator);

        Simulator.StartSimulator(); //start order simulator

        while (isTimerRun)
        {
            worker.ReportProgress(0);
            Thread.Sleep(1000);
        }
    }
    public void UpdateWindow(BO.Order order)
    {
        DO.Order n = new()
        {
            ID = 999
        };
        if (order != null)
            worker.ReportProgress(1, (object?)order);
        else
            worker.ReportProgress(1, (object?)n);
    }

    //public void UpdateEstimatedTime(DateTime time)
    //{
    //    worker.ReportProgress(2, time);
    //}

    //public void FinishSimulator()
    //{
    //    worker.CancellationPending();
    //}

    
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        switch (e.ProgressPercentage)
        {    
            case 0:
                Clock = stopWatch.Elapsed.ToString().Substring(0, 8);
                //ProgressBarValue = (e.ProgressPercentage * 100) + a++;
                break;
            case 1:
                
                Order = (e.UserState as BO.Order)!;
                IDOrderInProgress.Content = Order.ID;
                break;
            case 2:
                try { StopTime.Content = e.UserState; }
                catch { }
                break;
            default:
                break;
        }  
    }


    private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        //Simulator.DeRegisterToUpdtes(updateProgres);
        //Simulator.DeRegisterToStop(stopWorker);
        //Simulator.DeRegisterToComplete(UpdateComplete);
        MessageBox.Show("Simulation Stoped");
        Close();
    }

    private void StopSimulation(object sender, RoutedEventArgs e)
    {
        if (isTimerRun)
        {
            stopWatch.Stop();
            isTimerRun = false;
        }
    }

    //private void FinishSimalation(EventArgs e)
    //{
    //    this.Dispatcher.Invoke(() =>
    //    {
    //        IDOrderInProgress.Content = "Finish Orders";
    //        OldStatus.Content = "Finish Orders";
    //        StartTime.Content = "Finish Orders";
    //        FutureStatus.Content = "Finish Orders";
    //        StopTime.Content = "Finish Orders";
    //    }); 
    //}
}
