namespace Employees.Client.Contracts
{
    public interface IDispatcher
    {
        string DispatchCommand(string commandName, string[] commandParameters);
    }
}
