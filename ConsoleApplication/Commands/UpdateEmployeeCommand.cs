using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;


namespace ConsoleApplication.Commands
{
    public class UpdateEmployeeCommand : Command
    {
        public override string CommandName => "update";
        public override string Description => "Удалить сотрудника из списка";

        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            var id = int.Parse(ParameterUtils.TryGetParamValue(parameters.First(), "Id"));
            var employee = employees.FirstOrDefault(
                x => x.Id.Equals(id));
            if (employee is null)
                throw new NoUserException();

            foreach (var parameter in parameters.Skip(1))
            {
                var param = ParameterUtils.TryGetKeyValueFromParameter(parameter);
                try
                {
                    typeof(Employee).GetProperty(param.Key).SetValue(employee, param.Value);
                }
                catch (Exception)
                {
                    throw new ArgumentException($"Неправильно задан параметр {param.Key}");
                }
            }
        }
    }
}
