using ConsoleApplication.Utils;
using ConsoleApplication.Models;
using System.Text.RegularExpressions;
using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    public class AddEmployeeCommand : Command
    {
        public override string CommandName => "add";
        public override string Description => "Добавить нового сотрудника";
        private readonly Regex _onlyNoLetters = new(@"[^a-zA-Zа-яА-ЯёЁ]");
        private readonly Regex _onlyNoDigits = new(@"[^0-9\.,]");

        public override void Execute(IList<Employee> employees, params string[] parameters)
        {
            try
            {

                if (parameters.Length != 3)
                    throw new ArgumentException("Указано недостаточно параметров");

                var id = employees.Any() ? employees.Max(x => x.Id) + 1 : 1;
                var firstName = ParameterUtils.TryGetParamValue(parameters[0], "FirstName");
                var lastName = ParameterUtils.TryGetParamValue(parameters[1], "LastName");
                var salaryPerHour = ParameterUtils.TryGetParamValue(parameters[2], "SalaryPerHour").Replace('.', ',');

                var asdasd = _onlyNoLetters.Match(firstName);
                if (_onlyNoLetters.IsMatch(firstName) || _onlyNoLetters.IsMatch(lastName))
                    throw new ArgumentException("Имя или фамилия не могут содержать цифры");

                if (_onlyNoDigits.IsMatch(salaryPerHour))
                    throw new ArgumentException("Зарплата не может содержать буквы");

                employees.Add(new Employee(
                    id: id,
                    firstName: firstName,
                    lastName: lastName,
                    salaryPerHour: decimal.TryParse(salaryPerHour, out decimal result) 
                                    ? result 
                                    : throw new ArgumentException("Значение заплаты слишком мало или велико")
                ));
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
