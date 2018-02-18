namespace Employees.Client.Models
{
    using System.Collections.Generic;

    internal class ManagerInfoDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ManagedEmployeesCount { get; set; }

        public ICollection<EmployeeDto> ManagedEmployees { get; set; }
    }
}
