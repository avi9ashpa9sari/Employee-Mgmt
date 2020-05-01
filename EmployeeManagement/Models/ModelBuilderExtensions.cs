using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mary",
                    Email = "TestMary@mail.com",
                    Department = Dept.Finance
                },
                new Employee
                {
                    Id = 2,
                    Name = "John",
                    Email = "TestJohn@mail.com",
                    Department = Dept.IT
                }
                );
        }
    }
}
