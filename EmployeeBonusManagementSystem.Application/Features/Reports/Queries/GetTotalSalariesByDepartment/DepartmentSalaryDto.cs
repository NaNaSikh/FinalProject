namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;

public class DepartmentSalaryDto
{
    public required string Name { get; set; }
    public decimal TotalSalary { get; set; }
}
