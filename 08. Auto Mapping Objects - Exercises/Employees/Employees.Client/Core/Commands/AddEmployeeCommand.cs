namespace Employees.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Models;
    using Services.Contracts;

    internal class AddEmployeeCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public AddEmployeeCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                throw new InvalidOperationException("Invalid parameters");
            }

            string firstName = data[0];
            string lastName = data[1];
            decimal salary = decimal.Parse(data[2]);

            EmployeeDto employee = this.employeeService.Add<EmployeeDto>(firstName, lastName, salary);

            return $"Employee {employee.FirstName} {employee.LastName} was added successfully.";
        }
    }
}
