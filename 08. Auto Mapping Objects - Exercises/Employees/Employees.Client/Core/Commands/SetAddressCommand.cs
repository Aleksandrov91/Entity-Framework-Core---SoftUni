namespace Employees.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Client.Contracts;
    using Services.Contracts;

    internal class SetAddressCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public SetAddressCommand(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            if (data.Length < 2)
            {
                throw new InvalidOperationException("Invalid parameters");
            }

            int employeeId = int.Parse(data[0]);
            string address = string.Join(" ", data.Skip(1));

            string employeeName = this.employeeService.SetAddress(employeeId, address);

            return $"Employee {employeeName} address has been set to {address}";
        }
    }
}
