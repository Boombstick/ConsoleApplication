using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    public class GetAllCommand : Command
    {
        public override string CommandName => "getall";

        public override string Description => "Получить всех сотрудников";


        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            if (!employees.Any())
                throw new NoUserException("В списке еще нет сотрудников");
            
            foreach (var employee in employees)
            {
                var message = $"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}";
                Console.WriteLine(message);
            }
        }
    }
}

