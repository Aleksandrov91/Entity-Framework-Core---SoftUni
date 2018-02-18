namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Client.Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(params string[] data)
        {
            Console.WriteLine("Good Bye!");

            Environment.Exit(0);

            return string.Empty;
        }
    }
}
