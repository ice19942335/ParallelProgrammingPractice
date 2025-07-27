namespace CriticalSections;

class Program
{
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
            Interlocked.Add(ref _balance, amount);
            //Balance += amount;
        }

        public void Withdraw(int amount)
        {
            //Balance -= amount;
            Interlocked.Add(ref _balance, -amount);
            
            // 1:
            // 2:
            Interlocked.MemoryBarrier();
            // 3: Operation 3 can't be executed after the "Interlocked.MemoryBarrier();" line
        }
    }

    static void Main(string[] args)
    {
        var tasks = new List<Task>();
        var ba = new BankAccount();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    ba.Deposit(100);
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < 1000; j++)
                {
                    ba.Withdraw(100);
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine($"Balance is {ba.Balance}.");
    }
}