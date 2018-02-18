namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;

    using Client.Contracts;

    public class Engine
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IServiceProvider serviceProvider;

        public Engine(ICommandDispatcher commandDispatcher, IServiceProvider serviceProvider)
        {
            this.commandDispatcher = commandDispatcher;
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    Console.Write("Enter command: ");
                    string[] input = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string commandName = input.First();
                    string[] commandArgs = input.Skip(1).ToArray();

                    string result = this.commandDispatcher.DispatchCommand(commandName, commandArgs);
                    Console.WriteLine(result);
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine(ae.Message);
                }
                catch (InvalidOperationException io)
                {
                    Console.WriteLine(io.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
