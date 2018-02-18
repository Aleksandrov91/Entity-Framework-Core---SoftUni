namespace Employees.Client.Core.Commands
{
    using System;

    using Client.Contracts;
    using Client.Models;
    using Services.Contracts;

    internal class SetManagerCommand : ICommand
    {
        private IEmployeeService employeeService;

        public SetManagerCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                throw new InvalidOperationException("Invalid parameters");
            }

            int employeeId = int.Parse(data[0]);
            EmployeeDto employee = this.employeeService.ById<EmployeeDto>(employeeId);

            if (employee == null)
            {
                throw new ArgumentException("Employee not found.");
            }

            int managerId = int.Parse(data[1]);
            ManagerInfoDto manager = this.employeeService.ById<ManagerInfoDto>(managerId);

            if (manager == null)
            {
                throw new ArgumentException("Manager not found.");
            }

            this.employeeService.SetManager(employeeId, managerId);

            return $"Manager {manager.FirstName} {manager.LastName} has been added to {employee.FirstName} {employee.LastName}";
        }
    }
}
