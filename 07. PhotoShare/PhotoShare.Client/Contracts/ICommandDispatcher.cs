namespace PhotoShare.Client.Contracts
{
    public interface ICommandDispatcher
    {
        string DispatchCommand(string commandName, params string[] commandParameters);
    }
}
