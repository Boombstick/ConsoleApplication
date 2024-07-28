using ConsoleApplication.Models;
using System.Text.RegularExpressions;

namespace ConsoleApplication.Utils
{
    public static class ParameterUtils
    {
        private static readonly Regex _onlyNoLetters = new(@"[^a-zA-Zа-яА-ЯёЁ]");
        private static readonly Regex _onlyNoDigits = new(@"[^0-9,]");
        private static readonly char[] _trimChars = ['.', ',', '?', '/', '\\', '\'', ';', ':', '~', '`',
            '\"', '[', ']', '{', '}', '(', ')', '<', '>', '|', '=', '+', '-',];
        private static readonly string[] _employeeProportiesName = typeof(Employee).GetProperties().Select(x => x.Name).ToArray();

        public static (string Value, string Error) TryGetParamValue(string parameter, out string paramName)
        {
            var keyValuePair = parameter.Split(':');
            if (keyValuePair.Length != 2)
                throw new ArgumentException($"Неуказано название или значение параметра в {parameter}");

            var parameterName = _employeeProportiesName.FirstOrDefault(x => x.Equals(keyValuePair[0]));
            if (parameterName is null)
                throw new ArgumentException($"Параметр {keyValuePair[0].Trim(_trimChars)} указан неправильно");

            var value = GetParamValue(keyValuePair[1].Trim(_trimChars), keyValuePair[0].Trim(_trimChars));
            paramName = parameterName;
            if (!string.IsNullOrEmpty(value.Error))
                return (string.Empty, value.Error);

            return (value.Value.Trim(_trimChars), string.Empty);

        }
        public static (string Value, string Error) TryGetParamValue(string parameter) => TryGetParamValue(parameter, out string paramName);
        private static (string Value, string Error) GetParamValue(string paramValue, string paramName)
        {
            if (paramName.Equals(nameof(Employee.Id)))
            {
                if (_onlyNoDigits.IsMatch(paramValue) || Regex.Matches(paramValue, @",").Count > 0)
                    return (string.Empty, "Id должен состоять только из цифр");
            }
            else if (!paramName.Equals(nameof(Employee.SalaryPerHour)))
            {
                if (_onlyNoLetters.IsMatch(paramValue))
                    return (string.Empty, "Имя и/или фамилия не могут содержать цифры");
            }
            else
            {
                if (_onlyNoDigits.IsMatch(paramValue) || Regex.Matches(paramValue, @",").Count > 1)
                    return (string.Empty, "Зарплата неккоректна, используйте шаблон - 1234,5678");
            }
            return (paramValue, string.Empty);
        }

    }
}
