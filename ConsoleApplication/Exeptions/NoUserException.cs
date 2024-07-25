namespace ConsoleApplication.Exeptions
{
    public class NoUserException : Exception
    {
        private const string exeptionMessage = "Сотрудника с таким Id не существует";
        public NoUserException(string message) : base(message) { }
        public NoUserException() : base(exeptionMessage)
        {

        }
    }
}
