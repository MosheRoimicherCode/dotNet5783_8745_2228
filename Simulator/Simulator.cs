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

    public static event SimulatorStopEventHandler? StopEvent;
    public static event SimulatorUpdatedEventHandler? UpdateEvent;


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

                BO.Order orderUpdated = bl.Order.UpdateStatus((int)id);
                //ProgressChangedEventArgs e = new(1,)
                
                //if (UpdateEvent != null) UpdateEvent.Invoke(orderUpdated, e);
                
                Thread.Sleep(delay * 1000); //simulate store work time

            }
            else
            {
                flagForStopSimulator = false;
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

    //Cancel Registration
    public static void CancelRegisterToStopEvent(SimulatorStopEventHandler func) => StopEvent -= func;
    public static void CancelRegisterToUpdateEvent(SimulatorUpdatedEventHandler func) => UpdateEvent -= func;



}
