namespace Employees.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Client.Contracts;

    public class CommandDispatcher : IDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string DispatchCommand(string commandName, string[] commandParameters)
        {
            string commandString = $"{commandName}Command".ToLower();

            Type commandType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .SingleOrDefault(n => n.Name.ToLower() == commandString);

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid Command!");
            }

            ConstructorInfo constructor = commandType.GetConstructors().First();

            Type[] ctorParams = constructor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            object[] services = ctorParams
                .Select(this.serviceProvider.GetService)
                .ToArray();

            ICommand commad = (ICommand)constructor.Invoke(services);

            return commad.Execute(commandParameters);
        }
    }
}
