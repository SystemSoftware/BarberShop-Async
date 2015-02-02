using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace Sleeping_Barber_AsyncAwait
{
    class Program
    {
        // Barber Shop
        static int MaxCustomers = 10;
        const int NumChairs = 5;

        static ConcurrentQueue<Customer> queue = new ConcurrentQueue<Customer>();
        static void Main(string[] args)
        {
            Console.Write("Ready?");
            Console.ReadKey();
            Task BarberTask = Task.Run(() => BarberAsync());
            Console.WriteLine("\nBarberTask has started.");
            for (int i = 0; i < MaxCustomers; i++)
            {
                Task CustomerTask = Task.Run(() => CustomerAsync(i));
                Console.WriteLine("CustomerTask {0} has started.", i);
            }
            while (!BarberTask.IsCompleted)
            {
                // Do Busy Waiting
            }
        }

        private static async Task BarberAsync()
        {
            int hairCutsDone = 0;
            Boolean maySleepAgain = false;
            Console.WriteLine("Barber is now sleeping. zZzZzZzZzZzZzZz...");
            while (!hairCutsDone.Equals(MaxCustomers))
            {
                if (!queue.IsEmpty)
                {
                    // There is a customer, so lets cut some hairs!
                    Customer c;
                    queue.TryDequeue(out c);
                    Console.WriteLine("Cutting {0}, done: {1}, queued: {2}", c.Id, hairCutsDone, queue.Count);
                    await Task.Delay(1500);
                    hairCutsDone++;
                    maySleepAgain = true;
                }
                else
                {
                    // There is no customer, so continue sleeping.
                    if (maySleepAgain)
                    {
                        Console.WriteLine("There is currently no customer, so back to sleep!");
                        maySleepAgain = false;
                    }
                    //await Task.Delay(1000);
                }
            }
            // All customers served.
            Console.WriteLine("All customers have been served, the Barber goes home.");
        }

        private static async Task CustomerAsync(int Id)
        {
            await Task.Delay(new Random().Next(1, 5) * 500);
            Console.WriteLine("Customer {0} arrives.", Id);

            Customer c = new Customer(Id);
            if (!queue.Contains(c))
            {
                if (queue.Count < NumChairs)
                {
                    // Customer will be served
                    queue.Enqueue(c);
                    Console.WriteLine("Customer {0} is now waiting, queue length: {1}.", Id, queue.Count);
                }
                else
                {
                    // Customer can't wait to be served
                    Console.WriteLine("Customer {0} cannot be served and leaves the Barber-Shop unsatisfied.", Id);
                    MaxCustomers--;
                }
            }

        }
    }

    class Customer
    {
        public int Id { get; set; }
        public Customer(int id)
        {
            this.Id = id;
        }
    }
}
