using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployee(int Id);
        Employee Add(Employee newEmployee);
        Employee Update(Employee employeeEdit);
        Employee Delete(int Id);
    }
}
