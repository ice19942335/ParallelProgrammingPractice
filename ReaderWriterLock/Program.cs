namespace ReaderWriterLock;

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

    public void Transfer(BankAccount where, int amount)
    {
        Balance -= amount;
        where.Balance += amount;
    }
}

class Program
{
    private static ReaderWriterLockSlim padlock = new();
    static Random random = new();
    
    static void Main(string[] args)
    {
        int x = 0;

        var tasks = new List<Task>();
        for (int i = 0; i < 6; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                //padlock.EnterReadLock();
                padlock.EnterUpgradeableReadLock();

                if (i % 2 == 0)
                {
                    padlock.EnterWriteLock();
                    x = random.Next(10);
                    Console.WriteLine($"Set x = {x}");
                    padlock.ExitWriteLock();
                }
                
                Console.WriteLine($"Entered read lock, x = {x}");
                Thread.Sleep(1000);
                //padlock.ExitReadLock();
                padlock.ExitUpgradeableReadLock();
                Console.WriteLine($"Exited read lock, x = {x}.");
            }));
        }

        try
        {
            Task.WaitAll(tasks.ToArray());
        }
        catch (AggregateException e)
        {
            e.Handle(x =>
            {
                Console.WriteLine(x);
                return true;
            });
        }

        while (true)
        {
            Console.ReadKey();
            padlock.EnterWriteLock();
            Console.WriteLine("Write lock acquired");
            int newValue = random.Next(10);
            x = newValue;
            Console.WriteLine($"Set x = {x}");
            padlock.ExitWriteLock();
            Console.WriteLine("Write lock released");
        }
    }
}