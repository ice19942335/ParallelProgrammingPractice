namespace ExceptionHandling;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Test();
        }
        catch (AggregateException e)
        {
            foreach (var exception in e.InnerExceptions)
                Console.WriteLine($"Handled elswhere: {exception.GetType()}");
        }


        Console.WriteLine("Main programming done.");
        Console.ReadKey();
    }

    private static void Test()
    {
        var t = Task.Factory.StartNew(() =>
        {
            throw new InvalidOperationException("Can't do this") { Source = "t" };
        });
        
        var t2 = Task.Factory.StartNew(() =>
        {
            throw new AccessViolationException("Čan't access this") { Source = "t2"};
        });

        try
        {
            Task.WaitAll(t, t2);
        }
        catch (AggregateException ex)
        {
            ex.Handle(e =>
            {
                if (e is InvalidOperationException)
                {
                    Console.WriteLine("Invalid operation!");
                    return true;
                }

                return false;
            });
        }
    }
}