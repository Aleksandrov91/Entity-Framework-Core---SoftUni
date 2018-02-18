namespace Employees.Services.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IEmployeeService
    {
        TModel ById<TModel>(int id);

        TModel Add<TModel>(string firstName, string lastName, decimal salary);

        string SetBirthDay(int employeeId, DateTime birthday);

        string SetAddress(int employeeId, string address);

        void SetManager(int employeeId, int managerId);

        TModel MangerInfo<TModel>(int employeeId);

        ICollection<TModel> EmployeesByFilter<TModel>(int age);
    }
}
