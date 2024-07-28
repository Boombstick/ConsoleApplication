using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using ConsoleApplication.Exeptions;
using ConsoleApplication.Abstractions;


namespace ConsoleApplication.Commands
{
    public class UpdateEmployeeCommand : Command
    {
        public override string CommandName => "update";
        public override string Description => "Обновить данные сотрудника";

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

                foreach (var parameter in parameters.Skip(1))
                {
                    var param = ParameterUtils.TryGetParamValue(parameter, out string parameterName);
                    try
                    {
                        if (!parameterName.Equals(nameof(Employee.SalaryPerHour)))
                            typeof(Employee).GetProperty(parameterName).SetValue(employee, param.Value);
                        else
                        {
                            typeof(Employee).GetProperty(parameterName).SetValue(employee, decimal.TryParse(param.Value, out decimal salary) ? salary
                                                                                                        : throw new InvalidCastException("Значение зарплаты слишком велико"));
                        }
                    }
                    catch (InvalidCastException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Неправильно задан параметр {parameterName}");
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
