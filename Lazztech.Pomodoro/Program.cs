using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Lazztech.Pomodoro
{
    class Program
    {
        public static Settings Settings { get; set; }

        static void Main(string[] args)
        {
            var settings = Program.Settings;
            if (args.Any())
            {
                if (args[0].ToLower() == "start")
                {}

                if (args[0].ToLower() == "stop")
                {}

                if (args[0].ToLower() == "pause")
                {}

                if (args[0] == "-h")
                {}
            }

            //Show status with brief intro and description
            Console.WriteLine("Lazztech.Pomodoro");
            System.Console.WriteLine("The arg -h opens help.");
            /* If settings.json not supplied
             */

            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var binFiles = Directory.GetFiles(assemblyPath);
            var settingsExist = binFiles.Where(x => x.ToLower().Contains("settings.json")).ToList().Any();
            if (settingsExist == false)
            {
                System.Console.WriteLine("No settings file found.");
                System.Console.WriteLine("Generating default settings. Edit the values in settings.json.");
                settings= new Settings();
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(assemblyPath, json);
            }
            else{
                var settingsFilePath = binFiles.Where(x => x.ToLower().Contains("settings.json")).FirstOrDefault();
                settings = (Settings)JsonConvert.DeserializeObject(settingsFilePath);
            }

            System.Console.Write("What would you like to do: ");
            var result = Console.Read();

            //DESERIALIZE LAST 20 POMS FOR REPORT

            System.Console.WriteLine(result);
        }
    }
}
