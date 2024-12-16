using System;
using System.Threading;

namespace StopwatchApp
{
    class Program
    {
        // Stopwatch class defined inside the same file
        public class Stopwatch
        {
            public TimeSpan TimeElapsed { get; private set; }
            public bool IsRunning { get; private set; }

            // Declare delegate and events
            public delegate void StopwatchEventHandler(string message);
            public event StopwatchEventHandler OnStarted;
            public event StopwatchEventHandler OnStopped;
            public event StopwatchEventHandler OnReset;

            private Timer timer;

            public void Start()
            {
                if (IsRunning) return;

                IsRunning = true;
                OnStarted?.Invoke("Stopwatch Started!");
                timer = new Timer(Tick, null, 0, 1000); // Tick every 1 second
            }

            public void Stop()
            {
                if (!IsRunning) return;

                IsRunning = false;
                timer?.Dispose();
                OnStopped?.Invoke("Stopwatch Stopped!");
            }

            public void Reset()
            {
                Stop();
                TimeElapsed = TimeSpan.Zero;
                OnReset?.Invoke("Stopwatch Reset!");
            }

            private void Tick(object state)
            {
                TimeElapsed = TimeElapsed.Add(TimeSpan.FromSeconds(1));
            }
        }

        static void Main(string[] args)
        {
            // Create stopwatch instance
            Stopwatch stopwatch = new Stopwatch();

            // Subscribe to events
            stopwatch.OnStarted += (message) => Console.WriteLine(message);
            stopwatch.OnStopped += (message) => Console.WriteLine(message);
            stopwatch.OnReset += (message) => Console.WriteLine(message);

            bool running = true;

            while (running)
            {
                // Display UI
                Console.Clear();
                Console.WriteLine("Stopwatch Console Application");
                Console.WriteLine("Elapsed Time: " + stopwatch.TimeElapsed.ToString(@"hh\:mm\:ss"));
                Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit");

                var input = Console.ReadKey(intercept: true).Key;

                switch (input)
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
                        stopwatch.Stop();
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input! Press S, T, R, or Q.");
                        break;
                }

                Thread.Sleep(1000); // Sleep for a second to simulate the tick
            }
        }
    }
}
