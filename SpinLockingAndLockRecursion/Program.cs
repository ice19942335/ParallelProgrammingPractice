namespace CriticalSections;

public class BankAccount
{
    private int _balance;

    public int Balance
    {
        get => _balance;
        private set => _balance = value;
    }

    public void Deposit(int amount)
    {
        Balance += amount;
    }

    public void Withdraw(int amount)
    {
        Balance -= amount;
    }
}

class Program
{
    static SpinLock sl = new(true);

    static void Main(string[] args)
    {
        //Example_1_NoRecursion();
        Example_2_HasRecursion_ThrowsException();
    }

    private static void Example_1_NoRecursion()
    {
        var tasks = new List<Task>();
        var ba = new BankAccount();

        SpinLock sl = new SpinLock(); 

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    bool lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);;
                        ba.Deposit(100);
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            sl.Exit();
                        }
                    }
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    bool lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);;
                        ba.Withdraw(100);
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            sl.Exit();
                        }
                    }
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine($"Balance is {ba.Balance}.");
    }

    private static void Example_2_HasRecursion_ThrowsException()
    {
        LockRecursion(5);
    }

    private static void LockRecursion(int x)
    {
        bool lockTaken = false;

        try
        {
            sl.Enter(ref lockTaken);
        }
        catch (LockRecursionException e)
        {
            Console.WriteLine($"Recursion: {e}");
        }
        finally
        {
            if (lockTaken)
            {
                Console.WriteLine($"Took a lock, x = {x}");
                LockRecursion(x - 1);
                sl.Exit();
            }
            else
            {
                Console.WriteLine($"DFailed to take a lock, x = {x}");
            }
        }
    }
}