namespace Employees.Client.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Client.Contracts;

    public class Engine : IEngine
    {
        private readonly IDispatcher dispatcher;
        private readonly IReader reader;
        private readonly IWriter writer;

        public Engine(IDispatcher dispatcher, IReader reader, IWriter writer)
        {
            this.dispatcher = dispatcher;
            this.reader = reader;
            this.writer = writer;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    this.writer.Write("Enter a command: ");
                    List<string> commandArgs = this.reader.Read()
                        .Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    string commandName = commandArgs[0];
                    commandArgs.RemoveAt(0);

                    string[] commandParams = commandArgs.ToArray();

                    string result = this.dispatcher.DispatchCommand(commandName, commandParams);

                    this.writer.WriteLine(result);
                }
                catch (Exception ex)
                {
                    this.writer.WriteLine(ex.Message);
                }
            }
        }
    }
}
