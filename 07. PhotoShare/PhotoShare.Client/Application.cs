namespace PhotoShare.Client
{
    using System;

    using Core;
    using Data;
    using Microsoft.Extensions.DependencyInjection;
    using Services;
    using Services.Contracts;

    public class Application
    {
        public static void Main()
        {
            ResetDatabase();

            IServiceProvider servicesProvider = ConigureServices();
            CommandDispatcher commandDispatcher = new CommandDispatcher(servicesProvider);
            Engine engine = new Engine(commandDispatcher, servicesProvider);
            engine.Run();
        }

        private static IServiceProvider ConigureServices()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<PhotoShareContext>();

            serviceCollection.AddTransient<IAlbumService, AlbumService>();
            serviceCollection.AddTransient<ITagService, TagService>();
            serviceCollection.AddTransient<ITownService, TownService>();
            serviceCollection.AddTransient<IUserService, UserService>();

            ServiceProvider provider = serviceCollection.BuildServiceProvider();

            return provider;
        }

        private static void ResetDatabase()
        {
            using (var db = new PhotoShareContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
