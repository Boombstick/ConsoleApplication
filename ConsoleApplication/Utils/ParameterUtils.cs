namespace ConsoleApplication.Utils
{
    public static class ParameterUtils
    {
        public static (string Key, string Value) TryGetKeyValueFromParameter(string parameter)
        {
            var keyValuePair = parameter.Split(':');
            if (keyValuePair.Length != 2)
                throw new ArgumentException("Неуказано имя или значение параметра");
            return (keyValuePair[0].Trim('.'), keyValuePair[1].Trim('.'));
        }
        public static string TryGetParamValue(string parameter, string paramName)
        {
            try
            {
                var keyValuePair = TryGetKeyValueFromParameter(parameter);
                if (!keyValuePair.Key.Equals(paramName))
                    throw new ArgumentException($"Параметр {paramName} указан неправильно");

                return keyValuePair.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
