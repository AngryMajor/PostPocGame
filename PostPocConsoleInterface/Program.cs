using System;
using System.Threading;

using PostPocBackend;

namespace PostPocConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Class1.Test());

            Console.Title = "test";
            Console.WriteLine("Hello World!");
            Thread.Sleep(3000);
        }
    }
}
