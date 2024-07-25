using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    internal class GetEmployeeCommand : Command
    {
        public override string CommandName => "get";
        public override string Description => "Получить одного сотрудника";

        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            var id = int.Parse(ParameterUtils.TryGetParamValue(parameters.First(), "Id"));
            var employee = employees.FirstOrDefault(
                x => x.Id.Equals(id));
            if (employee is null)
                throw new NoUserException();
            var message = $"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}";
            Console.WriteLine(message);
        }
    }
}
