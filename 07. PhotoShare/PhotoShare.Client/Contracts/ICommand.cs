namespace PhotoShare.Client.Contracts
{
    public interface ICommand
    {
        string Execute(params string[] data);
    }
}
