namespace WaitingForTasks;
  
class Program
{
    static void Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        var t = new Task(() =>
        {
            Console.WriteLine("I take 5 seconds");

            for (int i = 0; i < 5; i++)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(1000);
            }
            
            Console.WriteLine("I'm m done");
        }, token);
        t.Start();
        
        var t2 = Task.Factory.StartNew(() => Thread.Sleep(3000), token);
        
        //Task.WaitAll(t, t2);
        Task.WaitAll([t, t2], 4000, token);
        
        Console.WriteLine($"Task t status is {t.Status}");
        Console.WriteLine($"Task t2 status is {t2.Status}");
        
        Console.WriteLine("Main programming done.");
        Console.ReadKey();
    }
}