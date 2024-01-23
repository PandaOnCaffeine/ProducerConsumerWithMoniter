using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumerWithMoniter
{
    internal class Program
    {
        static Queue<object> _buffer = new Queue<object>();

        static void Main(string[] args)
        {
            Thread ProducerThread = new Thread(() => Producer());
            Thread ConsumerThread = new Thread(() => Consumer());

            ProducerThread.Start();
            ConsumerThread.Start();

            ProducerThread.Join();
            ConsumerThread.Join();

            Console.ReadLine();
        }
        static void Producer()
        {
            while (true)
            {
                lock (_buffer)
                {
                    if (_buffer.Count >= 5)
                    {
                        Monitor.PulseAll(_buffer);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(_buffer.Count + " Count | Producer waiting");

                        Monitor.Wait(_buffer);
                    }
                    else
                    {
                        _buffer.Enqueue(1);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(_buffer.Count + " Count | produced");
                    }
                    Thread.Sleep(new Random().Next(100, 500));
                }
            }
        }
        static void Consumer()
        {
            while (true)
            {
                lock (_buffer)
                {
                    if (_buffer.Count == 0)
                    {
                        Monitor.PulseAll(_buffer);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(_buffer.Count + " Count | Consumer waiting");

                        Monitor.Wait(_buffer);
                    }
                    else
                    {
                        _buffer.Dequeue();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(_buffer.Count + " Count | Consumed");
                    }
                    Thread.Sleep(new Random().Next(100, 500));
                }
            }
        }
    }
}
