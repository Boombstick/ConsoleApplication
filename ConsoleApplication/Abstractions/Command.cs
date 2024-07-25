using ConsoleApplication.Models;

namespace ConsoleApplication.Abstractions
{
    public abstract class Command
    {
        public abstract string CommandName { get; }
        public abstract string Description { get; }
        public abstract void Execute(IList<Employee> employyes, params string[] parameters);
    }
}
