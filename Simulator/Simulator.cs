using BlApi;

static public class Simulator
{
    static readonly IBl bl = BlApi.Factory.Get();

    static readonly Thread simulator = new(RunSimulator); //create Thread that run simulator
    static volatile bool flagForStopSimulator;
    static void RunSimulator()  //body simulator
    {
        flagForStopSimulator = true;
        while (flagForStopSimulator)
        {
            Console.WriteLine(Thread.CurrentThread.Name);
            Thread.Sleep(1000);
            NextEvent?.Invoke(null, EventArgs.Empty); // send progress event
        }
        StopThread?.Invoke(null,EventArgs.Empty); //send stop thread
    }
    
    static public void StartSimulator() => simulator.Start(); //public method to start simulator
    static public void StopSimulator(bool flag) => flagForStopSimulator = flag; //public method to stop simulator

    static event EventHandler? StopThread;  // event of stop simulator
    static event EventHandler? NextEvent;   // event of progress of simulator

    static public void AssignEventOfStop() => StopThread(null,EventArgs.Empty);
    static public void AssignEventOfNext() => NextEvent(null, EventArgs.Empty);
}
