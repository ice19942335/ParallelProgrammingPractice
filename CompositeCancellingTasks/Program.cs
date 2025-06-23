namespace CompositeCancellingTasks;

class Program
{
    static void Main(string[] args)
    {
        var planner = new CancellationTokenSource();
        var preventative = new CancellationTokenSource();
        var emergency = new CancellationTokenSource();
        
        var paranoid = CancellationTokenSource.CreateLinkedTokenSource(planner.Token, preventative.Token, emergency.Token);

        Task.Factory.StartNew(() =>
        {
            int i = 0;
            while (true)
            {
                paranoid.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"{i++}");
                Thread.Sleep(1000);
            }
        }, paranoid.Token);
        
        Console.ReadKey();
        emergency.Cancel();
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}