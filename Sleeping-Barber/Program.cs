//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Sleeping_Barber
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//        }
//    }
//}

using System;
using System.Threading;
class Program
{
    // Create a Random Number Generator
    static Random Rand = new Random();
    // Define the maximum number of customers and the maximum number of chairs.
    const int MaxCustomers = 25;
    const int NumChairs = 5;
    static Semaphore waitingRoom = new Semaphore(NumChairs, NumChairs);
    static Semaphore barberChair = new Semaphore(1, 1);
    static Semaphore barberPillow = new Semaphore(0, 1);
    static Semaphore seatBelt = new Semaphore(0, 1);
    // Are we finished?
    static bool AllDone = false;
    static void Barber()
    {
        while (!AllDone)
        {
            Console.WriteLine("The barber is sleeping.");
            barberPillow.WaitOne();
            if (!AllDone)
            {
                // Cutting hair for a random amount of time.
                Console.WriteLine("The Barber is cutting the hair.");
                Thread.Sleep(Rand.Next(1, 3) * 1000);
                Console.WriteLine("The barber's finished cutting hair.");
                seatBelt.Release();
            }
            else
            {
                Console.WriteLine("The barber's going home.");
            }
        }
        return;
    }
    static void Customer(Object number)
    {
        int Number = (int)number;
        Console.WriteLine("Customer {0} leaves for the barber shop", Number);
        Thread.Sleep(Rand.Next(1, 5) * 1000);
        Console.WriteLine("Customer {0} has arrived.", Number);
        waitingRoom.WaitOne();
        Console.WriteLine("Customer {0} entering waiting room", Number);
        barberChair.WaitOne();
        waitingRoom.Release();
        Console.WriteLine("Barber, customer {0} wishes to wake you up!", Number);
        barberPillow.Release();
        seatBelt.WaitOne();
        barberChair.Release();
        Console.WriteLine("Customer {0} leaves the barber shop.", Number);
    }
    static void Main()
    {
        Thread BarberThread = new Thread(Barber);
        BarberThread.Start();
        Thread[] Customers = new Thread[MaxCustomers];
        for (int i = 0; i < MaxCustomers; i++)
        {
            Customers[i] = new Thread(new ParameterizedThreadStart(Customer));
            Customers[i].Start(i);
        }
        for (int i = 0; i < MaxCustomers; i++)
        {
            Customers[i].Join();
        }
        AllDone = true;
        barberPillow.Release();
        // Wait for the Barber's thread to finish before exiting.
        BarberThread.Join();
        Console.WriteLine("End of demonstration. Thanks for watching. This should always be the last line displayed.");
    }
}
