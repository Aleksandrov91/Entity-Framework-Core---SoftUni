namespace Employees.Client.Core.Commands
{
    using System;
    using System.Text;

    using Client.Contracts;
    using Client.Models;
    using Services.Contracts;

    internal class EmployeeInfoCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public EmployeeInfoCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length != 1)
            {
                throw new InvalidOperationException("Invalid parameters");
            }

            int employeeId = int.Parse(data[0]);

            EmployeeDetailsDto employee = this.employeeService.ById<EmployeeDetailsDto>(employeeId);

            StringBuilder employeeInfo = new StringBuilder();
            employeeInfo.AppendLine($"ID: {employee.EmployeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");

            string employeeBirthdate = employee.Birthday == null ? "No information" : employee.Birthday.Value.ToString("dd-MM-yyyy");
            string employeeAddress = employee.Address ?? "No information";

            employeeInfo.AppendLine($"Birthday: {employeeBirthdate}");
            employeeInfo.AppendLine($"Address: {employeeAddress}");

            return employeeInfo.ToString().Trim();
        }
    }
}
