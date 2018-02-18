namespace Employees.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Client.Contracts;
    using Client.Models;
    using Services.Contracts;

    internal class EmployeesOlderThanCommand : ICommand
    {
        private IEmployeeService employeeService;

        public EmployeesOlderThanCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Invalid parameters");
            }

            int age = int.Parse(data[0]);

            ICollection<EmployeeWithManagerDto> olderThanEmployees = this.employeeService.EmployeesByFilter<EmployeeWithManagerDto>(age);

            StringBuilder sb = new StringBuilder();

            foreach (var employee in olderThanEmployees)
            {
                string managerName = employee.Manager == null ? "No Information" : $"{employee.Manager.FirstName} {employee.Manager.LastName}";

                sb.AppendLine($"{employee.FirstName} {employee.LastName} - ${employee.Salary:F2} - Manager: {managerName}");
            }

            return sb.ToString().Trim();
        }
    }
}
