using BlApi;
using Microsoft.Exchange.WebServices.Data;
using System.ComponentModel;

namespace Sim; 
static public class Simulator
{
    //Observer Pathern

    //Notify() events
    public delegate void SimulatorStopEventHandler(EventArgs e);
    public delegate void SimulatorUpdatedEventHandler(object sender, ProgressChangedEventArgs e);
    public delegate void FinisgTimeEventHandler(object sender, ProgressChangedEventArgs e);

    public static event SimulatorStopEventHandler? StopEvent;
    public static event SimulatorUpdatedEventHandler? UpdateEvent;
    public static event FinisgTimeEventHandler? TimeEvent;


    static readonly IBl bl = BlApi.Factory.Get();                                                             //connection to database

    static readonly Thread simulator = new(RunSimulator);                                                   //create Thread that run simulator
    static public void StartSimulator() => simulator.Start();                                                    //public method to start simulator
    static void RunSimulator()                                                                                //body simulator
    {
        flagForStopSimulator = true;
        while (flagForStopSimulator)
        {
            int? id = bl.Order.ReturnOrderForManage();
            if (id is not null)
            {
                int delay = random.Next(3, 11); //between 3 to 10
                DateTime finishTime = DateTime.Now + (new TimeSpan(0, 0, 0, delay, 0));

                //if (TimeEvent != null) TimeEvent.Invoke(finishTime);

                BO.Order orderUpdated = bl.Order.UpdateStatus((int)id);

                if (UpdateEvent != null) UpdateEvent.Invoke(orderUpdated,  EventArgs.Empty);
                
                Thread.Sleep(delay * 1000); //simulate store work time

                StopEvent?.Invoke(EventArgs.Empty);
            }
        }
    }



    static volatile bool flagForStopSimulator;                                                               //volatile to stop Thread
    static public void StopSimulator(bool flag) { }                                                          //public method to stop simulator

    static readonly Random random = new();                                                                   //for pauses - not important

    //Registration
    public static void RegisterToStopEvent(SimulatorStopEventHandler func) => StopEvent += func;
    public static void RegisterToUpdateEvent(SimulatorUpdatedEventHandler func) => UpdateEvent += func;
    public static void RegisterToTimeEvent(FinisgTimeEventHandler func) => TimeEvent += func;

    //Cancel Registration
    static void CancelRegisterToStopEvent(SimulatorStopEventHandler func) => StopEvent -= func;
    static void CancelRegisterToUpdateEvent(SimulatorUpdatedEventHandler func) => UpdateEvent -= func;
    public static void CancelRegisterToTimeEvent(FinisgTimeEventHandler func) => TimeEvent -= func;



}
