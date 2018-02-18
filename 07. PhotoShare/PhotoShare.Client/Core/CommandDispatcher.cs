namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Client.Contracts;

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string DispatchCommand(string commandName, params string[] commandParameters)
        {
            Type commandType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .SingleOrDefault(n => n.Name.ToLower() == $"{commandName}Command".ToLower());

            if (commandType == null)
            {
                throw new ArgumentException("Invalid Command!");
            }

            var ctor = commandType.GetConstructors().First();

            Type[] ctorParams = ctor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var services = ctorParams.Select(this.serviceProvider.GetService)
                .ToArray();

            ICommand command = (ICommand)ctor.Invoke(services);

            return command.Execute(commandParameters);
        }
    }
}
