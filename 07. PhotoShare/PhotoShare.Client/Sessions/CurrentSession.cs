namespace PhotoShare.Client.Sessions
{
    using Models;

    public static class CurrentSession
    {
        public static User LoggedUser { get; set; }

        public static bool IsAuthorised => LoggedUser != null;
    }
}
