using System;
using System.Linq;

namespace Lazztech.Pomodoro
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].ToLower() == "start")
            {}

            if (args[0].ToLower() == "stop")
            {}

            if (args[0].ToLower() == "pause")
            {}

            if (args[0] == "-h")
            {}

            //Show status with brief intro and description
            Console.WriteLine("Lazztech.Pomodoro");
            System.Console.WriteLine("The arg -h opens help.");
            System.Console.Write("What would you like to do: ");
            var result = Console.Read();

            System.Console.WriteLine(result);
        }
    }
}
