namespace Employees.Client.Core.Commands
{
    using System;
    using System.Globalization;

    using Client.Contracts;
    using Services.Contracts;

    internal class SetBirthdayCommand : ICommand
    {
        private readonly IEmployeeService employeeService;

        public SetBirthdayCommand(IEmployeeService employeeService)
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
            DateTime birthday = DateTime.ParseExact(data[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            string employeeName = this.employeeService.SetBirthDay(employeeId, birthday);

            return $"Employee {employeeName} birhdate has been set to: {birthday.ToString("dd-MM-yyyy")}";
        }
    }
}
