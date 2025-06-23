namespace WaitingForTimeToPass;

class Program
{
    static void Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var t = new Task(() =>
        {
            Console.WriteLine("Press any key to disarm\ndyou have 5 seconds.");
            bool cancelled = token.WaitHandle.WaitOne(5000);
            Console.WriteLine(cancelled ? "\ndisarmed" : "boom!");
        }, token);
        t.Start();
        
        Console.ReadKey();
        cts.Cancel();
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}