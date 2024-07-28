using System.Text.Json;
using ConsoleApplication.Models;
using ConsoleApplication.Commands;
using ConsoleApplication.Extensions;
using ConsoleApplication.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApplication
{
    internal class Program
    {
        public static ServiceProvider Services { get; private set; }

        static void Main(string[] args)
        {
            InitServices();
            FileStreamOptions options = new FileStreamOptions()
            {
                Mode = FileMode.OpenOrCreate,
            };
            // чтение данных
            List<Employee> employees = new();
            using (StreamReader reader = new StreamReader("Employees.json", options))
            {
                var jsonData = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(jsonData))
                    employees = JsonSerializer.Deserialize<List<Employee>>(jsonData);
            }
            var request = string.Join(" ", args).Split("-");
            var commands = request.Skip(1)
                .Select(x => x.Trim().Split(" ")
                .ToList())
                .Select(y => new Request(y.First(), y.Skip(1)))
                .ToList();

            var commandContainer = Services.GetRequiredService<CommandContainer>();
            bool operationsIsSuccess = false;
            foreach (var command in commands)
            {

                string consoleText = $"Операция {command.Command} {string.Join(" ", command.Arguments)} выполнена";
                try
                {
                    if (commandContainer.CommandsByName.ContainsKey(command.Command))
                        commandContainer.CommandsByName[command.Command].Execute(employees, command.Arguments.ToArray());
                    else
                        throw new InvalidOperationException($"Команды {command.Command} нет");
                    operationsIsSuccess = true;
                }
                catch (Exception ex)
                {
                    consoleText = $"Операция {command.Command} {string.Join(" ", command.Arguments)} не выполнена";
                    if (!string.IsNullOrEmpty(ex.Message))
                        Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine(consoleText);
                    if (operationsIsSuccess is false)
                    {
                        Console.WriteLine("Изменения отменены");
                        Environment.Exit(0);
                    }

                }
            }
            if (operationsIsSuccess)
            {
                // запись данных
                using (StreamWriter writer = new StreamWriter("Employees.json", false))
                {
                    JsonSerializer.Serialize<List<Employee>>(writer.BaseStream, employees);
                    Console.WriteLine("Изменения сохранены");
                }
            }
        }
        private record Request(string Command, IEnumerable<string> Arguments);
        private static void InitServices()
        {
            Services = new ServiceCollection()
                .AddChildClassesAsSingletone(typeof(Command))
                .AddSingleton<CommandContainer>()
                .BuildServiceProvider();
        }
    }
}
