using BlApi;

static public class Simulator
{
    //Notify() events
    public delegate void SimulatorStopEventHandler(EventArgs e);
    public delegate void SimulatorUpdatedEventHandler(object source,EventArgs e);

    public static event SimulatorStopEventHandler? StopEvent;
    public static event SimulatorUpdatedEventHandler? UpdateEvent;

    //Notify()
    static void OnStopEvent()
    {
        if (StopEvent != null)
            StopEvent(EventArgs.Empty);
    }
    static void OnUpdateEvent(object source)
    {
        if (UpdateEvent != null)
            UpdateEvent(source, EventArgs.Empty);
    }

    //Registration
    static void RegisterToStopEvent(SimulatorStopEventHandler func) => StopEvent += func;
    static void RegisterToUpdateEvent(SimulatorUpdatedEventHandler func) => UpdateEvent += func;

    //Cancel Registration
    static void CancelRegisterToStopEvent(SimulatorStopEventHandler func) => StopEvent -= func;
    static void CancelRegisterToUpdateEvent(SimulatorUpdatedEventHandler func) => UpdateEvent -= func;

    static readonly IBl bl = BlApi.Factory.Get();                                                           //conection to database

    static readonly Thread simulator = new(RunSimulator);                                                   //create Thread that run simulator
    static public void StartSimulator() => simulator.Start();                                               //public method to start simulator
    static void RunSimulator()                                                                              //body simulator
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
                OnUpdateEvent(orderUpdated);
                Thread.Sleep(delay * 1000); //simulate store work time
                OnUpdateEvent("go to next order");
            }
        }
        OnStopEvent();
    }

    static volatile bool flagForStopSimulator;                                                               //volatile to stop Thread
    static public void StopSimulator(bool flag) { }                                                          //public method to stop simulator

    static readonly Random random = new();                                                                  //for pauses - not important
}
