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
            try
            {
                var id = int.Parse(ParameterUtils.TryGetParamValue(parameters.First(), nameof(Employee.Id)));
                var employee = employees.FirstOrDefault(
                    x => x.Id.Equals(id));
                if (employee is null)
                    throw new NoUserException();

                foreach (var parameter in parameters.Skip(1))
                {
                    var param = ParameterUtils.TryGetKeyValueFromParameter(parameter);
                    try
                    {
                        if (!param.Key.Equals(nameof(Employee.SalaryPerHour)))
                            typeof(Employee).GetProperty(param.Key).SetValue(employee, param.Value);
                        else
                            typeof(Employee).GetProperty(param.Key).SetValue(employee, decimal.TryParse(param.Value.Replace('.', ','), out decimal result)
                                                                                                        ? result
                                                                                                        : throw new ArgumentException("Значение заплаты слишком мало или велико"));
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Неправильно задан параметр {param.Key}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
