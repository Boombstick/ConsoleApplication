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
            foreach (var arg in args)
                Console.WriteLine(arg);

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
            bool success = false;
            foreach (var command in commands)
            {

                string consoleText = $"Операция {command.Command} {string.Join(" ", command.Arguments)} выполнена";
                try
                {
                    if (commandContainer.CommandsByName.ContainsKey(command.Command))
                        commandContainer.CommandsByName[command.Command].Execute(employees, command.Arguments.ToArray());
                    else
                        throw new KeyNotFoundException($"Команда {command.Command} не найдена");
                    success = true;
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
                }
            }
            if (success)
            {
                // запись данных
                using (StreamWriter writer = new StreamWriter("Employees.json", false))
                {
                    JsonSerializer.Serialize<List<Employee>>(writer.BaseStream, employees);
                    Console.WriteLine("Данные сохранены");
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
