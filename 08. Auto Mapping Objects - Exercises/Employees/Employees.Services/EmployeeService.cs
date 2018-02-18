namespace Employees.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Model;
    using Services.Contracts;

    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeContext context;

        public EmployeeService(EmployeeContext context)
        {
            this.context = context;
        }

        public TModel Add<TModel>(string firstName, string lastName, decimal salary)
        {
            Employee employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            };

            TModel model = Mapper.Map<TModel>(employee);

            this.context.Employees.Add(employee);
            this.context.SaveChanges();

            return model;
        }

        public TModel ById<TModel>(int id)
        {
            TModel user = this.context.Employees
                .AsNoTracking()
                .Where(e => e.EmployeeId == id)
                .ProjectTo<TModel>()
                .SingleOrDefault();

            return user;
        }

        public string SetBirthDay(int employeeId, DateTime birthday)
        {
            Employee employee = this.context.Employees.Find(employeeId);
            employee.Birthday = birthday;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            Employee employee = this.context.Employees.Find(employeeId);
            employee.Address = address;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public void SetManager(int employeeId, int managerId)
        {
            Employee employee = this.context.Employees.Find(employeeId);
            employee.ManagerId = managerId;

            this.context.SaveChanges();
        }

        public TModel MangerInfo<TModel>(int employeeId)
        {
            TModel model = this.context.Employees
                .Where(e => e.EmployeeId == employeeId)
                .ProjectTo<TModel>()
                .SingleOrDefault();

            return model;
        }

        public ICollection<TModel> EmployeesByFilter<TModel>(int age)
        {
            ICollection<TModel> employeesByFilter = this.context.Employees
                .Where(e => (DateTime.Now.Year - e.Birthday.Value.Year) > age)
                .OrderByDescending(e => e.Salary)
                .ProjectTo<TModel>()
                .ToArray();

            return employeesByFilter;
        }
    }
}
