using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>() {
                new Employee() {Id=1,Name="John",Email="Test1@mail.com", Department=Dept.Finance},
                new Employee() {Id=2,Name="Jason",Email="Test2@mail.com",Department=Dept.IT},
                new Employee() {Id=3,Name="John",Email="Test1@mail.com", Department=Dept.HR},
            };
        }

        public Employee Add(Employee newEmployee)
        {
           newEmployee.Id =  _employeeList.Max(e => e.Id + 1);
             _employeeList.Add(newEmployee);
            return newEmployee;
        }

        public Employee Delete(int Id)
        {
             Employee emp = _employeeList.FirstOrDefault(e => e.Id == Id);
            if (emp != null)
            {
                _employeeList.Remove(emp);
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e=>e.Id==Id);
        }

        public Employee Update(Employee employeeEdit)
        {
            Employee emp = _employeeList.FirstOrDefault(e => e.Id == employeeEdit.Id);
            if (emp != null)
            {
                emp.Name = employeeEdit.Name;
                emp.Email = employeeEdit.Email;
                emp.Department = employeeEdit.Department;
            }
            return emp;
        }
    }
}
