namespace CreatingAndStartingTasks;

class Program
{
    public static void Write(char c)
    {
        int i = 1000;
        while (i-- > 0)
        {
            Console.Write(c);
        }
    }
    
    public static void Write(object o)
    {
        int i = 1000;
        while (i-- > 0)
        {
            Console.Write(o);
        }
    }

    public static int TextLength(object o)
    {
        Console.WriteLine($"Task with id {Task.CurrentId} processing object {o}...");
        return o.ToString().Length;
    }
    
    static async Task Main(string[] args)
    {
        // There also an option to pass value inside a lambda function like this: new Task(() => TextLength("TEST"));
        string text1 = "testing", text2 = "this";
        var task1 = new Task<int>(TextLength, text1);
        task1.Start();
        Task<int> task2 = Task.Factory.StartNew(TextLength, text2);
        
        Console.WriteLine($"Length of '{text1}' is {task1.Result}");
        Console.WriteLine($"Length of '{text2}' is {await task2}");
        
        Console.WriteLine("Main programming done.");
        Console.ReadKey();
    }
}
