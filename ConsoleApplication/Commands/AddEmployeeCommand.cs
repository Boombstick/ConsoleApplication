using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    public class AddEmployeeCommand : Command
    {
        public override string CommandName => "add";
        public override string Description => "Добавить нового сотрудника";

        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            try
            {
                if (parameters.Length != 3)
                    throw new ArgumentException("Указано недостаточно или слишком много параметров");

                var id = employees.Any() ? employees.Max(x => x.Id) + 1 : 1;
                var firstName = ParameterUtils.TryGetParamValue(parameters[0]);
                var lastName = ParameterUtils.TryGetParamValue(parameters[1]);
                var salaryPerHour = ParameterUtils.TryGetParamValue(parameters[2]);

                if (!string.IsNullOrEmpty(firstName.Error)
                    || !string.IsNullOrEmpty(lastName.Error)
                    || !string.IsNullOrEmpty(salaryPerHour.Error))
                {
                    throw new ArgumentException(string.Join(',', firstName.Error, lastName.Error, salaryPerHour.Error));
                }

                employees.Add(new Employee(
                    id: id,
                    firstName: firstName.Value,
                    lastName: lastName.Value,
                    salaryPerHour: decimal.TryParse(salaryPerHour.Value, out decimal result)
                                    ? result
                                    : throw new InvalidCastException("Значение зарплаты слишком велико")
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
