namespace Employees.Client
{
    using System;

    using AutoMapper;
    using Client.Contracts;
    using Client.Core;
    using Client.Readers;
    using Client.Writers;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;

    public class Application
    {
        public static void Main(string[] args)
        {
            IServiceProvider services = ConfigureServices();

            IDispatcher dispatcher = new CommandDispatcher(services);
            IReader reader = new ConsoleReader();
            IWriter writer = new ConsoleWriter();

            IEngine engine = new Engine(dispatcher, reader, writer);
            engine.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeeContext>(option => option.UseSqlServer(Configuration.ConnectionString));
            serviceCollection.AddTransient<IEmployeeService, EmployeeService>();
            serviceCollection.AddTransient<IWriter, ConsoleWriter>();
            serviceCollection.AddAutoMapper();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
