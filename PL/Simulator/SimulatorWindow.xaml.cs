﻿namespace PL;

using Sim;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

public partial class SimulatorWindow : Window
{
    private bool isTimerRun;
    BackgroundWorker worker;

    public static readonly DependencyProperty OrderDep =
        DependencyProperty.Register(nameof(Order), typeof(BO.Order), typeof(SimulatorWindow));
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
        set => SetValue(MyClockProperty, value);
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

        Clock = DateTime.Now.ToString("HH:mm:ss");

        worker = new BackgroundWorker()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        worker.DoWork += Worker_DoWork!;
        worker.ProgressChanged += Worker_ProgressChanged!;
        worker.RunWorkerCompleted += worker_RunWorkerCompleted;

    }

    //button command
    private void StartSimulation(object sender, RoutedEventArgs e)
    {
        if (!isTimerRun)
        {
            isTimerRun = true;
            worker.RunWorkerAsync(); //call DOWork function
        }
    }
    private void Worker_DoWork(object sender, DoWorkEventArgs e)
    {
        Simulator.RegisterChangeOrder(UpdateWindow);
        Simulator.RegisterCompletedSimulation(final);
        Simulator.RegisterBar(UpdateBar);

        Simulator.StartSimulator(); //start order simulator

        while (isTimerRun)
        {
            worker.ReportProgress(0);
            Thread.Sleep(1000);
        }
    }
    public void UpdateWindow(Tuple<int, DateTime, DateTime, string, string> tuple) => worker.ReportProgress(1, tuple);
    public void UpdateBar(int i) => worker.ReportProgress(3, i);

    DateTime delay;
    DateTime now;
    double progressPerSecond;
    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {


        switch (e.ProgressPercentage)
        {
            case 0:
                Clock = DateTime.Now.ToString("HH:mm:ss");
                ProgressBarValue += progressPerSecond;
                break;
            case 1:
                ProgressBarValue = 0;
                delay = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item3;
                IDOrderInProgress.Content = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item1;
                OldStatus.Content = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item4;
                StartTime.Content = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item2.ToString("HH:mm:ss");
                FutureStatus.Content = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item5;
                StopTime.Content = (e.UserState as Tuple<int, DateTime, DateTime, string, string>)!.Item3.ToString("HH:mm:ss");
                break;
            case 3:
                progressPerSecond = (double)(100 / (int)(e.UserState!));
                break;
            default:
                break;
        }
    }


    private void worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        //Simulator.CalcelRegisterChangeOrder(UpdateWindow);
        //Simulator.CalcelRegisterCompletedSimulation(final);


        MessageBox.Show("Simulation Stoped");
        Close();
    }

    private void StopSimulation(object sender, RoutedEventArgs e)  
    {
        if (isTimerRun)
        {
            isTimerRun = false;
            Simulator.StopSimulation();
        }
    }

    private void final()
    {
        worker.CancelAsync();
    }
}
