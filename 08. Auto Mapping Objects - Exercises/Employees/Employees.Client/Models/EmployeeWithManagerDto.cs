namespace Employees.Client.Models
{
    internal class EmployeeWithManagerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public ManagerNameDto Manager { get; set; }
    }
}
