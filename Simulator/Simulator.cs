namespace Simulator;

static public class Simulator
{
    private static volatile bool flag;
    private static void RunSimulator()
    {
        flag = true;
        while (flag)
        {
            Console.WriteLine(Thread.CurrentThread.Name);
            Thread.Sleep(1000);
            if (somethingHapended()) flag = false;
            
        }
    }
    static bool somethingHapended() => true;


    static Thread simulator = new(RunSimulator); //create Thread that make something
    public static void ActivatorSimulator() => simulator.Start(); //public method to start simulator

    static private event EventHandler StopThread;
    static private event EventHandler NextEvent;

    public static void Stop() { }
    public static void NextItem() { }

}
