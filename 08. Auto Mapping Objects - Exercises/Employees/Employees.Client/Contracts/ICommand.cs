﻿namespace Employees.Client.Contracts
{
    public interface ICommand
    {
        string Execute(string[] data);
    }
}
