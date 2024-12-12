using System;
using System.Threading;

namespace StopwatchApp
{
    public class Stopwatch
    {
        private TimeSpan timeElapsed;
        private bool isRunning;

        public delegate void StopwatchEventHandler(string message);
        public event StopwatchEventHandler? OnStarted;
        public event StopwatchEventHandler? OnStopped;
        public event StopwatchEventHandler? OnReset;

        public Stopwatch()
        {
            timeElapsed = TimeSpan.Zero;
            isRunning = false;
        }

        public void Start()
        {
            if (isRunning)
            {
                Console.WriteLine("Stopwatch is already running.");
                return;
            }

            isRunning = true;
            Console.WriteLine();
            OnStarted?.Invoke("Stopwatch Started!");

            new Thread(() =>
            {
                while (isRunning)
                {
                    Thread.Sleep(1000);
                    Tick();
                }
            }).Start();
        }

        public void Stop()
        {
            if (!isRunning)
            {
                Console.WriteLine("Stopwatch is not running.");
                return;
            }

            isRunning = false;
            Console.WriteLine();
            OnStopped?.Invoke("Stopwatch Stopped!");
        }

        public void Reset()
        {
            isRunning = false;
            timeElapsed = TimeSpan.Zero;
            Console.WriteLine();
            OnReset?.Invoke("Stopwatch Reset!");
        }

        private void Tick()
        {
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            DisplayTime();
        }

        private void DisplayTime()
        {
            Console.SetCursorPosition(0, Console.CursorTop); // Move cursor to the start of the current line
            Console.Write($"Time Elapsed: {timeElapsed}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.OnStarted += message => 
            {
                Console.WriteLine(message);
            };
            stopwatch.OnStopped += message => 
            {
                Console.WriteLine(message);
            };
            stopwatch.OnReset += message =>
            {
                Console.WriteLine(message);
                Console.SetCursorPosition(0, Console.CursorTop); 
            };

            Console.WriteLine("Stopwatch Application");
            Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit");

            bool exit = false;

            while (!exit)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    switch (key)
                    {
                        case ConsoleKey.S:
                            stopwatch.Start();
                            break;

                        case ConsoleKey.T:
                            stopwatch.Stop();
                            break;

                        case ConsoleKey.R:
                            stopwatch.Reset();
                            break;

                        case ConsoleKey.Q:
                            exit = true;
                            Console.WriteLine("\nExiting application. Goodbye!");
                            break;

                        default:
                            Console.WriteLine("\nInvalid input. Use S, T, R, or Q.");
                            break;
                    }
                }
                Thread.Sleep(100); 
            }
        }
    }
}
