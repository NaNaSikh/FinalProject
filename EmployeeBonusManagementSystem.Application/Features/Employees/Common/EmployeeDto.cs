using EmployeeBonusManagementSystem.Domain.Entities;
using System;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Common
{
    public class EmployeeDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public string UserName { get; set; }
        public int DepartmentId { get; set; }
        public int? RecommenderEmployeeId { get; set; }
        public int CreateByUserId { get; set; }
        public String Role { get; set; }
    }
}
