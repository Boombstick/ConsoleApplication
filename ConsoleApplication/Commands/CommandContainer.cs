using ConsoleApplication.Abstractions;

namespace ConsoleApplication.Commands
{
    internal class CommandContainer
    {
        public Dictionary<string, Command> CommandsByName { get; private set; } = new Dictionary<string, Command>();
        public CommandContainer(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
                CommandsByName.Add(command.CommandName, command);
        }
    }
}
