namespace Employees.Client.Readers
{
    using System;

    using Client.Contracts;

    public class ConsoleReader : IReader
    {
        public string Read()
        {
            return Console.ReadLine();
        }
    }
}
