namespace Employees.Client.Core.Commands
{
    using System;

    using Client.Contracts;

    internal class ExitCommand : ICommand
    {
        private IWriter writer;

        public ExitCommand(IWriter writer)
        {
            this.writer = writer;
        }

        public string Execute(string[] data)
        {
            this.writer.WriteLine("Good Bye!");

            Environment.Exit(0);

            return string.Empty;
        }
    }
}
