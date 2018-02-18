namespace P03_FootballBetting.App
{
    using Data;

    public class StartUp
    {
        public static void Main()
        {
            using (FootballBettingContext context = new FootballBettingContext())
            {
                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();
            }
        }
    }
}
