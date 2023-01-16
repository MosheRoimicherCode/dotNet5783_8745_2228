namespace Simulator;

static public class Simulator
{
    private static volatile bool flagForStopSimulator;
    private static void RunSimulator()
    {
        flagForStopSimulator = true;
        while (flagForStopSimulator)
        {
            Console.WriteLine(Thread.CurrentThread.Name);
            Thread.Sleep(1000);
        }
    }
    static bool somethingHapended() => true;
    static Thread simulator = new(RunSimulator); //create Thread that make something
    
    public static void ActivatorSimulator() => simulator.Start(); //public method to start simulator
    public static void StopSimulator(bool flag) => flagForStopSimulator = flag; //public method to stop simulator


    static private event EventHandler StopThread;
    static private event EventHandler NextEvent;

    public static void Stop() { }
    public static void NextItem() { }

}
