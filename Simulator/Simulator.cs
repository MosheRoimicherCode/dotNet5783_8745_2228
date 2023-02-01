using BlApi;
using BO;
using Microsoft.Exchange.WebServices.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace Sim;
static public class Simulator
{
    //This class implements the observer pattern using events. 
    //It allows other classes to register for notifications of specific events, such as a change in order status or the completion of a simulation.


    //ProgressBar event, notified with delay time
    private static event Action<int>? Bar;
    public static void RegisterBar(Action<int> action) => Bar += action;
    public static void CalcelRegisterBar(Action<int> action) => Bar -= action;

    //ChangeOrder event, notified with (order ID, start time, finish time, old status, new status)
    private static event Action<Tuple<int, DateTime, DateTime, string, string>>? ChangeOrder;
    public static void RegisterChangeOrder(Action<Tuple<int, DateTime, DateTime, string, string>> action) => ChangeOrder += action;
    public static void CalcelRegisterChangeOrder(Action<Tuple<int, DateTime, DateTime, string, string>> action) => ChangeOrder -= action;

    //CompletedSimulation event, notified when the simulation is completed.
    private static event Action? CompletedSimulation;
    public static void RegisterCompletedSimulation(Action action) => CompletedSimulation += action;
    public static void CalcelRegisterCompletedSimulation(Action action) => CompletedSimulation -= action;

    //connection to database
    static readonly IBl bl = BlApi.Factory.Get();
    
    //body simulator
    static public void StartSimulator()                                                                                
    {
        //create e new thred to simulater order management
        new Thread(() =>
        {

            flagForStopSimulator = true;

            while (flagForStopSimulator)
            {
                int? id = bl.Order.ReturnOrderForManage();

                if (id != null)
                {
                    string oldstatus = bl.Order.Get((int)id).OrderStatus.ToString();
                    int delay = random.Next(3,11); //between 3 to 10
                    //bar progress update
                    Bar.Invoke(delay);

                    //finish time to end order
                    DateTime finishTime = DateTime.Now + (new TimeSpan(0, 0, 0, delay, 0));

                    //send furute status to UI
                    BO.Order? orderUpdated = bl.Order.UpdateStatus((int)id);

                    //update UI fields
                    Tuple<int, DateTime, DateTime, string, string> tuple = new(orderUpdated.ID, DateTime.Now, finishTime, oldstatus, orderUpdated.OrderStatus.ToString());
                    ChangeOrder?.Invoke(tuple);

                    //simulate store work time
                    Thread.Sleep(delay * 1000); 
                }
                //id have no more order to update, finish simulation
                else
                {
                    flagForStopSimulator = false;
                    //CompletedSimulation.Invoke();
                }
            }
        }).Start();
    }

    //volatile to stop Thread
    static volatile bool flagForStopSimulator;

    //for simulate order management with ThreadSleep function
    static readonly Random random = new();                                                                   
}
