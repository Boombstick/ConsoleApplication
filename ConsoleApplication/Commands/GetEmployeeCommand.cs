using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    internal class GetEmployeeCommand : Command
    {
        public override string CommandName => "get";
        public override string Description => "Получить cотрудника из списка";
        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            try
            {
                var id = ParameterUtils.TryGetParamValue(parameters.First());
                if (!string.IsNullOrEmpty(id.Error))
                    throw new ArgumentException(id.Error);
                var idValue = int.TryParse(id.Value,out int result)
                    ? result
                    : throw new ArgumentException("Введите Id первым аргументом");
                var employee = employees.FirstOrDefault(
                    x => x.Id.Equals(idValue));
                if (employee is null)
                    throw new NoUserException();
                var message = $"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}";
                Console.WriteLine(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
