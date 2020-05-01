using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<SQLEmployeeRepository> logger;

        public SQLEmployeeRepository(AppDbContext dbContext, ILogger<SQLEmployeeRepository> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public Employee Add(Employee newEmployee)
        {
            dbContext.Employees.Add(newEmployee);
            dbContext.SaveChanges();
            return newEmployee;
        }

        public Employee Delete(int Id)
        {
            Employee employee =  dbContext.Employees.Find(Id);
            if(employee!=null)
            {
                dbContext.Employees.Remove(employee);
                dbContext.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return dbContext.Employees;
        }

        public Employee GetEmployee(int Id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical error Log");

            return dbContext.Employees.Find(Id);
        }

        public Employee Update(Employee employeeEdit)
        {
            var employee = dbContext.Employees.Attach(employeeEdit);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return employeeEdit;
        }
    }
}
