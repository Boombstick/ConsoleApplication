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
                var id = int.Parse(ParameterUtils.TryGetParamValue(parameters.First(), nameof(Employee.Id)));
                var employee = employees.FirstOrDefault(
                    x => x.Id.Equals(id));
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
