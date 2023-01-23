using BlApi;
using BO;
using Microsoft.Exchange.WebServices.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace Sim; 
static public class Simulator
{
    //Observer Pathern

    //Notify() events



    private static event Action<int>? Bar;
    public static void RegisterBar(Action<int> action) => Bar += action;
    public static void CalcelRegisterBar(Action<int> action) => Bar -= action;


    private static event Action<Tuple<int, DateTime, DateTime, string, string>>? ChangeOrder;
    public static void RegisterChangeOrder(Action<Tuple<int, DateTime, DateTime, string, string>> action) => ChangeOrder += action;
    public static void CalcelRegisterChangeOrder(Action<Tuple<int, DateTime, DateTime, string, string>> action) => ChangeOrder -= action;


    private static event Action? CompletedSimulation;
    public static void RegisterCompletedSimulation(Action action) => CompletedSimulation += action;
    public static void CalcelRegisterCompletedSimulation(Action action) => CompletedSimulation -= action;



    static readonly IBl bl = BlApi.Factory.Get();                                                             //connection to database
    static public void StartSimulator()                                                                                //body simulator
    {
        new Thread(() =>
        {
            
            flagForStopSimulator = true;

            while (flagForStopSimulator)
            {
                int id = bl.Order.ReturnOrderForManage() ?? 0;
                string oldstatus = bl.Order.Get(id).OrderStatus.ToString();
                if (id != 0)
                {
                    int delay = random.Next(3, 11); //between 3 to 100
                    Bar.Invoke(delay);
                    DateTime finishTime = DateTime.Now + (new TimeSpan(0, 0, 0, delay, 0));
                    
                    BO.Order? orderUpdated = bl.Order.UpdateStatus((int)id);

                    Tuple<int, DateTime, DateTime, string, string> tuple= new(orderUpdated.ID, DateTime.Now, finishTime, oldstatus, orderUpdated.OrderStatus.ToString());

                    ChangeOrder?.Invoke(tuple);

                    Thread.Sleep(delay * 1000); //simulate store work time
                }
                else
                {
                    flagForStopSimulator = false;
                    CompletedSimulation.Invoke();
                }
            }
        }).Start();
    }



    static volatile bool flagForStopSimulator;                                                               //volatile to stop Thread

    static readonly Random random = new();                                                                   //for pauses - not important
}
