using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    public class DeleteEmployeeCommand : Command
    {
        public override string CommandName => "delete";
        public override string Description => "Удалить сотрудника из списка";

        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            try
            {
                var id = ParameterUtils.TryGetParamValue(parameters.First());
                if (!string.IsNullOrEmpty(id.Error))
                    throw new ArgumentException(id.Error);
                var idValue = int.TryParse(id.Value, out int result)
                    ? result
                    : throw new ArgumentException("Введите Id первым аргументом");
                var employee = employees.FirstOrDefault(
                    x => x.Id.Equals(idValue));
                if (employee is null)
                    throw new NoUserException();
                employees.Remove(employee);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
