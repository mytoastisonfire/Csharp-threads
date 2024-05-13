using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


/// <summary>
/// Øvelse0 creates a class called threprog which calls the WorkThreadFunction in the program
/// class which runs concurrently with the method Main
/// </summary>
namespace Øvelse0
{
    class program
    {
        public void WorkThreadFunction()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Simple Thread");
            }
        }
    }
    class threprog
    {
        public static void Main()
        {
            program pg = new program();
            Thread thread = new Thread(new ThreadStart(pg.WorkThreadFunction));
            thread.Start();
            Console.Read();
        }
    }
}


/// <summary>
/// The Main method creates a new thread which runs the TextLoop method concurrently with the Main thread.
/// </summary>
namespace Øvelse1
{
    class threadprogram
    {
        //Creates a program object to run on a seperate thread
        public static void Main()
        {
            program pg = new program();
            Thread thread = new Thread(new ThreadStart(pg.TextLoop));
            thread.Start();
            Console.Read();
        }
    }
    class program
    {
        public void TextLoop()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("C#-trådning er nemt!");
            }
        }
    }
}


/// <summary>
/// The Main method creates a new thread which runs the TextLoop method concurrently with the Main thread.
/// </summary>
namespace Øvelse2
{
    class threadprogram
    {
        // Creates 2 threads to run concurrently with each other and the Main thread
        public static void Main()
        {
            program pg = new program();
            // thread1 writes "C#-trådning [...]" to the console
            Thread thread1 = new Thread(new ThreadStart(pg.TextLoop1));
            // thread2 writes "Også med [...]" to the console
            Thread thread2 = new Thread(new ThreadStart(pg.TextLoop2));
            thread1.Start();
            thread2.Start();
            Console.Read();
        }
    }

    // Contains the loops for writing to the console
    class program
    {
        public void TextLoop1()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("C#-trådning er nemt!");
                Thread.Sleep(1000);
            }
        }
        public void TextLoop2()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Også med flere tråde ...");
                Thread.Sleep(1000);
            }
        }
    }
    /* Som svar på spørgsmålet om det er smart at bruge Thread.Sleep, så vil jeg sige nej for
     * man kan ikke være sikker på at det antal millisekunder man angiver er 100% korrekt
     * da det skal passe sammen med clock ticks på den pågældende maskine man prøver at køre koden på.
     * Men udover det så har vi ikke lært nogle andre måder at suspendere tråde på endnu, så jeg har ikke
     * en anden måde at gøre det på
     */
}


/// <summary>
/// The Main method creates a new thread which runs the temperatureReadWrite method to measure the temperatures
/// and count the alarms
/// </summary>

namespace Opgave3
{
    class threadprogram
    {
        /* Creates the new thread to run the temperatureReadWrite method.
         * Checks in a while(true) loop to see if the other thread is alive every 10 seconds
         * and kills the mainThread if it isn't.
         */
        public static void Main()
        {
            program pg = new program();
            Thread thread = new Thread(new ThreadStart(pg.temperatureReadWrite));
            thread.Start();
            while (true)
            {
                if (thread.IsAlive == false)
                {
                    Console.WriteLine("Alarm-tråd termineret!");
                    Thread.CurrentThread.Abort();
                }
                Thread.Sleep(10000);
            }
        }
    }

    /* Every 2 seconds it takes the temperature and checks wether or not it is inside the 
     * allowed temps.
     * If the temp is outside the parameters tempAlarm increases by one.
     * When tempAlarm reaches 3, the thread is killed
     */

    class program
    {
        Random rng = new Random();
        int tempAlarm = 0;
        public void temperatureReadWrite()
        {
            while (true)
            {
                int temperature = rng.Next(-20, 121);
                Console.WriteLine($"The temperature is {temperature}");
                if (temperature < 0)
                {
                    Console.WriteLine("Temperature too low!");
                    tempAlarm++;
                }
                else if (temperature > 100)
                {
                    Console.WriteLine("Temperature too high!");
                    tempAlarm++;
                }
                if (tempAlarm >= 3)
                {
                    Console.WriteLine("Too many alarms!");
                    break;
                }
                Thread.Sleep(2000);
            }
        }
    }
}



/// <summary>
/// The Main method creates 2 threads to run concurrently, a reader and a printer.
/// The printer continuously prints a character to the console and the reader waits for user input
/// and changes the character the printer prints.
/// </summary>
namespace Opgave4
{
    class threadprogram
    {
        public static void Main()
        {
            program pg = new program();
            Thread printer = new Thread(new ThreadStart(pg.Output));
            Thread reader = new Thread(new ThreadStart(pg.Input));
            printer.Start();
            reader.Start();
        }
    }

    class program
    {
        char ch = '*';
        char newCh;

        //Writes the char ch every 0.05 seconds to the console
        public void Output()
        {
            while (true)
            {
                Console.Write(ch);
                Thread.Sleep(50);
            }
        }

        /* Constantly checks for user input, if its different from the current char and different from enter
         * it will store the char in newCh and wait for the enter key to be pressed before changing ch
         */
        public void Input()
        {
            while (true)
            {
                char inputCh = Convert.ToChar(Console.Read());
                if (inputCh != ch && inputCh != '\r')
                {
                    newCh = inputCh;
                }
                else if (inputCh == '\r')
                {
                    ch = newCh;
                }
            }
        }
    }
}